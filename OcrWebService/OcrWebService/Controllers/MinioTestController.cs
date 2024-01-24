using Microsoft.AspNetCore.Mvc;

namespace OcrWebService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MinioTestController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly Minio.GiitMinio _giitMinio;

    public MinioTestController(ILogger<OcrController> logger, Minio.GiitMinio giitMinio)
    {
        _logger = logger;
        _giitMinio = giitMinio;
    }

    [HttpPost()]
    public async Task<ActionResult<string>> OcrUploadedFile(IFormFile file,
        [FromQuery] Minio.GiitMinio.MinioUploadDto request)
    {
        _logger.LogInformation("Started processing '{0}' POST.", nameof(OcrUploadedFile));

        await _giitMinio.UploadFile(file.OpenReadStream(), request);

        return Ok("elo");
    }
}