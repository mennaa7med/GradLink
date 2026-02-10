using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace GradLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatProxyController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatProxyController> _logger;
    private static string? _cachedWorkingModel = null; // Cache the working model

    public ChatProxyController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ChatProxyController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> TestApiKey()
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GEMINI_API_KEY_HERE")
        {
            return Ok(new { 
                success = false,
                message = "API key not configured",
                availableModels = Array.Empty<string>()
            });
        }

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);

        var diagnostics = new List<string>();
        var availableModels = new List<string>();

        // Test 1: List models
        diagnostics.Add("Testing ListModels API...");
        var listUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
        
        try
        {
            var listResponse = await httpClient.GetAsync(listUrl);
            var listContent = await listResponse.Content.ReadAsStringAsync();
            
            diagnostics.Add($"ListModels Status: {listResponse.StatusCode}");
            diagnostics.Add($"Response: {listContent.Substring(0, Math.Min(500, listContent.Length))}...");

            if (listResponse.IsSuccessStatusCode)
            {
                var listResult = JsonSerializer.Deserialize<JsonElement>(listContent);
                
                if (listResult.TryGetProperty("models", out var models))
                {
                    foreach (var model in models.EnumerateArray())
                    {
                        if (model.TryGetProperty("name", out var nameProp) && 
                            model.TryGetProperty("supportedGenerationMethods", out var methods))
                        {
                            var modelName = nameProp.GetString();
                            var methodsList = new List<string>();
                            
                            foreach (var method in methods.EnumerateArray())
                            {
                                methodsList.Add(method.GetString() ?? "");
                            }
                            
                            if (methodsList.Contains("generateContent"))
                            {
                                availableModels.Add(modelName ?? "unknown");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            diagnostics.Add($"ListModels Error: {ex.Message}");
        }

        // Test 2: Try a simple request with common models (updated for 2025)
        var testModels = new[] { "gemini-2.5-flash", "gemini-2.0-flash", "gemini-flash-latest" };
        
        foreach (var testModel in testModels)
        {
            diagnostics.Add($"\nTesting model: {testModel}...");
            var testUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{testModel}:generateContent?key={apiKey}";
            
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = "Say 'Hello'" } }
                    }
                }
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(testUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                diagnostics.Add($"  Status: {response.StatusCode}");
                diagnostics.Add($"  Response: {responseContent.Substring(0, Math.Min(200, responseContent.Length))}...");
                
                if (response.IsSuccessStatusCode)
                {
                    diagnostics.Add($"  ✅ {testModel} WORKS!");
                }
            }
            catch (Exception ex)
            {
                diagnostics.Add($"  Error: {ex.Message}");
            }
        }

        return Ok(new
        {
            success = availableModels.Count > 0,
            availableModels = availableModels,
            diagnostics = diagnostics,
            suggestion = availableModels.Count == 0 
                ? "No working models found. Try creating a new API key at https://aistudio.google.com/app/apikey" 
                : $"Found {availableModels.Count} working model(s)"
        });
    }

    [AllowAnonymous]
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { text = "Message is required" });
            }

            var apiKey = _configuration["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_GEMINI_API_KEY_HERE")
            {
                return Ok(new { text = "⚠️ API key not configured. Please add a valid Gemini API key to appsettings.Development.json" });
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // If we don't have a cached working model, discover available models
            if (_cachedWorkingModel == null)
            {
                _logger.LogInformation("Discovering available Gemini models...");
                _cachedWorkingModel = await DiscoverWorkingModel(httpClient, apiKey);
                
                if (_cachedWorkingModel == null)
                {
                    return Ok(new { 
                        text = "❌ Could not find any working Gemini models for your API key.\n\n" +
                               "This might mean:\n" +
                               "• Your API key is restricted or invalid\n" +
                               "• You need to enable the Generative Language API\n" +
                               "• Your region doesn't have access to Gemini models yet\n\n" +
                               "Try creating a new API key at: https://aistudio.google.com/app/apikey\n" +
                               "Make sure to select 'Create API key in new project'"
                    });
                }
                
                _logger.LogInformation("✅ Found working model: {Model}", _cachedWorkingModel);
            }

            // Use the cached working model
            var response = await SendToGemini(httpClient, apiKey, _cachedWorkingModel, request.Message);
            
            if (response != null)
            {
                return Ok(new { text = response });
            }
            
            // If cached model stopped working, try to discover again
            _logger.LogWarning("Cached model stopped working, rediscovering...");
            _cachedWorkingModel = null;
            _cachedWorkingModel = await DiscoverWorkingModel(httpClient, apiKey);
            
            if (_cachedWorkingModel != null)
            {
                response = await SendToGemini(httpClient, apiKey, _cachedWorkingModel, request.Message);
                if (response != null)
                {
                    return Ok(new { text = response });
                }
            }

            // Fallback: provide a helpful mock response
            _logger.LogWarning("All Gemini attempts failed, using fallback response");
            return Ok(new { 
                text = $"I received your message: \"{request.Message}\"\n\n" +
                       "⚠️ I'm currently in fallback mode because I can't connect to the Gemini API.\n\n" +
                       "To diagnose the issue, visit: http://localhost:5000/api/ChatProxy/test\n\n" +
                       "This will show you which models are available for your API key."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Chat endpoint");
            return Ok(new { text = $"❌ Error: {ex.Message}" });
        }
    }

    private async Task<string?> DiscoverWorkingModel(HttpClient httpClient, string apiKey)
    {
        // Try to list available models
        var listUrl = $"https://generativelanguage.googleapis.com/v1beta/models?key={apiKey}";
        
        try
        {
            var listResponse = await httpClient.GetAsync(listUrl);
            if (listResponse.IsSuccessStatusCode)
            {
                var listContent = await listResponse.Content.ReadAsStringAsync();
                var listResult = JsonSerializer.Deserialize<JsonElement>(listContent);
                
                if (listResult.TryGetProperty("models", out var models))
                {
                    // Find first model that supports generateContent
                    foreach (var model in models.EnumerateArray())
                    {
                        if (model.TryGetProperty("name", out var nameProp) && 
                            model.TryGetProperty("supportedGenerationMethods", out var methods))
                        {
                            var modelName = nameProp.GetString();
                            var supportsGenerate = false;
                            
                            foreach (var method in methods.EnumerateArray())
                            {
                                if (method.GetString()?.Contains("generateContent") == true)
                                {
                                    supportsGenerate = true;
                                    break;
                                }
                            }
                            
                            if (supportsGenerate && modelName != null)
                            {
                                _logger.LogInformation("Found supported model: {Model}", modelName);
                                return modelName;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not list models, trying fallback models");
        }

        // Fallback: try common models directly (updated for 2025)
        var fallbackModels = new[]
        {
            "models/gemini-2.5-flash",
            "models/gemini-2.0-flash",
            "models/gemini-2.0-flash-lite",
            "models/gemini-flash-latest",
            "models/gemini-2.5-pro",
        };

        foreach (var model in fallbackModels)
        {
            var testMessage = "Hi";
            var response = await SendToGemini(httpClient, apiKey, model, testMessage);
            if (response != null)
            {
                _logger.LogInformation("Fallback model works: {Model}", model);
                return model;
            }
        }

        return null;
    }

    private async Task<string?> SendToGemini(HttpClient httpClient, string apiKey, string modelName, string message)
    {
        try
        {
            // Build the URL - if modelName already contains "models/", use it directly
            var modelPath = modelName.StartsWith("models/") ? modelName : $"models/{modelName}";
            var url = $"https://generativelanguage.googleapis.com/v1beta/{modelPath}:generateContent?key={apiKey}";
            
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = message }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                
                if (result.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var candidate = candidates[0];
                    if (candidate.TryGetProperty("content", out var contentObj))
                    {
                        if (contentObj.TryGetProperty("parts", out var responseParts) && responseParts.GetArrayLength() > 0)
                        {
                            var part = responseParts[0];
                            if (part.TryGetProperty("text", out var textProp))
                            {
                                return textProp.GetString();
                            }
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("Model {Model} returned error: {Error}", modelName, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error sending to model: {Model}", modelName);
        }

        return null;
    }
}

public class ChatRequest
{
    public string? Message { get; set; }
}
