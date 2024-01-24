using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using OcrWebService.Data;
using OcrWebService.Data.Entity;
using OcrWebService.Services;
using System.Diagnostics;
using System.IO;
using Tesseract;

namespace OcrWebService.Controllers;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly OcrService _ocr;

    public OcrController(ILogger<OcrController> logger, OcrService ocr)
    {
        _logger = logger;
        _ocr = ocr;
    }

    [HttpGet()]
    public async Task<ActionResult<string>> OcrFilesFromDirectory()
    {
        _logger.LogInformation("Started processing '{0}' GET.", nameof(OcrFilesFromDirectory));

        await _ocr.OcrFilesFromPath();

        return Ok("elo");
    }

    [HttpPost()]
    public async Task<ActionResult<string>> OcrUploadedFile(IFormFile file)
    {
        _logger.LogInformation("Started processing '{0}' POST.", nameof(OcrUploadedFile));

        await _ocr.OcrFileFromUpload(file);

        return Ok("elo");
    }
}