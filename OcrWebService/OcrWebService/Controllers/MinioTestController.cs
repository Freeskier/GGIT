using Microsoft.AspNetCore.Mvc;

namespace OcrWebService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MinioTestController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly Minio.GgitMinio _ggitMinio;

    public MinioTestController(ILogger<OcrController> logger, Minio.GgitMinio ggitMinio)
    {
        _logger = logger;
        _ggitMinio = ggitMinio;
    }

    [HttpPost()]
    public async Task<ActionResult<string>> OcrUploadedFile(IFormFile file,
        [FromQuery] Minio.GgitMinio.MinioUploadDto request)
    {
        _logger.LogInformation("Started processing '{0}' POST.", nameof(OcrUploadedFile));

        await _ggitMinio.UploadFile(file.OpenReadStream(), request);

        return Ok("elo");
    }
}