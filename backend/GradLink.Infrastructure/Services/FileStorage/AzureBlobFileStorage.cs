using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using GradLink.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GradLink.Infrastructure.Services.FileStorage;

public class AzureBlobFileStorage : IFileStorage
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly ILogger<AzureBlobFileStorage> _logger;

    public AzureBlobFileStorage(IConfiguration configuration, ILogger<AzureBlobFileStorage> logger)
    {
        var connectionString = configuration["FileStorage:AzureBlobConnectionString"];
        _containerName = configuration["FileStorage:AzureBlobContainerName"] ?? "gradlink-files";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = logger;
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var blobPath = $"{folder}/{uniqueFileName}";
        var blobClient = containerClient.GetBlobClient(blobPath);

        await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken);

        _logger.LogInformation("File uploaded to Azure Blob: {Path}", blobPath);

        return blobPath;
    }

    public async Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(filePath);

        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            throw new FileNotFoundException("Blob not found", filePath);
        }

        var response = await blobClient.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(filePath);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        _logger.LogInformation("Blob deleted: {Path}", filePath);
    }

    public async Task<bool> FileExistsAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(filePath);

        return await blobClient.ExistsAsync(cancellationToken);
    }
}

