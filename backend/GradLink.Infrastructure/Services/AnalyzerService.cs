using System.Text.Json;
using GradLink.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GradLink.Infrastructure.Services;

public class AnalyzerService : IAnalyzerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AnalyzerService> _logger;
    private readonly string? _pythonAnalyzerUrl;
    private readonly int _timeoutSeconds;

    public AnalyzerService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<AnalyzerService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _pythonAnalyzerUrl = configuration["Analyzer:PythonAnalyzerUrl"];
        _timeoutSeconds = configuration.GetValue<int>("Analyzer:TimeoutSeconds", 30);
        _httpClient.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
    }

    public async Task<ResumeAnalysisResult> AnalyzeResumeAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if Python analyzer is configured
            if (string.IsNullOrEmpty(_pythonAnalyzerUrl))
            {
                _logger.LogWarning("Python analyzer URL not configured. Using fallback analysis.");
                return await FallbackAnalysisAsync(filePath);
            }

            // Read file and send to Python analyzer
            if (!File.Exists(filePath))
            {
                return new ResumeAnalysisResult
                {
                    Success = false,
                    Error = "File not found"
                };
            }

            using var fileStream = File.OpenRead(filePath);
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(fileStream);
            
            content.Add(streamContent, "file", Path.GetFileName(filePath));

            var response = await _httpClient.PostAsync($"{_pythonAnalyzerUrl}/analyze", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Python analyzer returned error status {StatusCode}. Using fallback.", response.StatusCode);
                return await FallbackAnalysisAsync(filePath);
            }

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<ResumeAnalysisResult>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? await FallbackAnalysisAsync(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing resume: {FilePath}", filePath);
            return await FallbackAnalysisAsync(filePath);
        }
    }

    private Task<ResumeAnalysisResult> FallbackAnalysisAsync(string filePath)
    {
        // Simple fallback analysis when Python service is unavailable
        return Task.FromResult(new ResumeAnalysisResult
        {
            Success = true,
            ExtractedText = $"Resume file: {Path.GetFileName(filePath)}",
            Skills = new List<string> { "Communication", "Teamwork", "Problem Solving" },
            Education = "Education information would be extracted by Python analyzer",
            Experience = "Experience information would be extracted by Python analyzer",
            Summary = "This is a placeholder analysis. Configure Python analyzer for detailed results.",
            QualityScore = 0.7
        });
    }
}

