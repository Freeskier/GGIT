using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.Exceptions;

namespace OcrWebService.Minio;

public class GiitMinio
{
    private readonly IMinioClient _minio;
    private readonly ILogger<GiitMinio> _logger;

    public GiitMinio(IMinioClient minio, ILogger<GiitMinio> logger)
    {
        this._minio = minio;
        _logger = logger;
    }

    public record MinioUploadDto(
        string objectName,
        // string filePath,
        string? contentType,
        string bucketName
    );

    public async Task<PutObjectResponse> UploadFile(Stream stream, MinioUploadDto data)
    {
        _logger.LogInformation("Uploading file to Minio. ObjectName: {0}, BucketName: {1}, ContentType: {2}",
            data.objectName, data.bucketName, data.contentType);
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(data.bucketName)
            .WithObject(data.objectName)
            .WithStreamData(stream)
            .WithObjectSize(-1)
            .WithContentType(data.contentType);
        return await DoUpload(putObjectArgs, data.bucketName);
    }

    public async Task<PutObjectResponse> UploadFile(string filePath, MinioUploadDto data)
    {
        _logger.LogInformation(
            "Uploading file to Minio. FilePath: {filePath} ObjectName: {1}, BucketName: {2}, ContentType: {3}",
            filePath, data.objectName, data.bucketName, data.contentType);
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(data.bucketName)
            .WithObject(data.objectName)
            .WithFileName(filePath)
            .WithObjectSize(-1)
            .WithContentType(data.contentType);
        return await DoUpload(putObjectArgs, data.bucketName);
    }

    private async Task<PutObjectResponse> DoUpload(PutObjectArgs putObjectArgs, string dataBucketName)
    {
        // Make a bucket on the server, if not already present.
        var beArgs = new BucketExistsArgs()
            .WithBucket(dataBucketName);
        bool found = await _minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(dataBucketName);
            await _minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }

        return await _minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
    }
}