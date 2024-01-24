using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using OcrWebService.Data;
using OcrWebService.Data.Entity;
using OcrWebService.Data.Model;
using System.Diagnostics;
using Tesseract;

namespace OcrWebService.Services;
public class OcrService
{
    private readonly ILogger<OcrService> _logger;
    private readonly FileManagementService _fileManagemenet;
    private readonly DbFileContext _dbContext;

    public OcrService(
        ILogger<OcrService> logger,
        FileManagementService fileManagemenet,
        DbFileContext dbFileContext)
    {
        _logger = logger;
        _fileManagemenet = fileManagemenet;
        _dbContext = dbFileContext;
    }

    //ACCEPTED EXTENSIONS TO OCR
    private readonly List<string> _acceptedExtensions = new List<string>
    {
        ".jpg",
        ".png"
    };

    public async Task OcrFileFromUpload(IFormFile file)
    {
        try
        {
            var ocrResult = OcrUploadedFile(file);

            var ocrResultEntity = new OcrResultEntity
            {
                BlobId = ocrResult.BlobId,
                Content = ocrResult.Text,
                InputExtension = ocrResult.InputExtension,
                Name = file.FileName,
                OutputExtension = ocrResult.OutputExtension,
                Confidence = ocrResult.Confidence
            };

            _fileManagemenet.CreateOutputFile(ocrResult.Text ?? string.Empty, ocrResultEntity);
            _dbContext.Add(ocrResultEntity);
        }
        catch (Exception e)
        {
            Trace.TraceError(e.ToString());
            Console.WriteLine("Unexpected Error: " + e.Message);
            Console.WriteLine("Details: ");
            Console.WriteLine(e.ToString());
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task OcrFilesFromPath()
    {
        var testBundlePath = "./TestBundle/";
        string[] files = Directory.GetFiles(testBundlePath);

        _logger.LogInformation("Found '{count}' files in directory.", files.Length);

        foreach (var inputFilePath in files)
        {
            try
            {
                var ocrResult = OcrResultIFilesInPath(inputFilePath);

                var ocrResultEntity = new OcrResultEntity
                {
                    BlobId = ocrResult.BlobId,
                    Content = ocrResult.Text,
                    InputExtension = ocrResult.InputExtension,
                    Name = Path.GetFileNameWithoutExtension(inputFilePath),
                    OutputExtension = ocrResult.OutputExtension,
                    Confidence = ocrResult.Confidence
                };

                _fileManagemenet.CreateOutputFile(ocrResult.Text ?? string.Empty, ocrResultEntity);
                _dbContext.Add(ocrResultEntity);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    public OcrResult OcrUploadedFile(IFormFile file)
    {
        var blobId = Guid.NewGuid();
        var inputExtension = Path.GetExtension(file.FileName);
        var filePath = string.Empty;

        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();

            filePath = _fileManagemenet.SaveInputFileInOutputPath(file.FileName, blobId, fileBytes); //in OutputPath in order to have all files (input => output)
        }

        if (!_acceptedExtensions.Exists(x => x.Equals(inputExtension)))
        {
            switch (inputExtension)
            {
                case ".pdf":
                    filePath = _fileManagemenet.ConvertPdfToPng(filePath, blobId);
                    break;
                default:
                    throw new InvalidOperationException("Failed to convert to destination extension");
            }
        }

        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

        using var img = Pix.LoadFromFile(filePath);
        using var page = engine.Process(img);

        return new OcrResult
        {
            Text = page.GetText(),
            Confidence = page.GetMeanConfidence(),
            BlobId = blobId,
            InputExtension = inputExtension,
            OutputExtension = Path.GetExtension(filePath)
        };

    }

    public OcrResult OcrResultIFilesInPath(string inputFilePath)
    {
        var blobId = Guid.NewGuid();
        var inputExtension = Path.GetExtension(inputFilePath);
        var finalFilePath = inputFilePath;

        _fileManagemenet.SaveInputFileInOutputPath(inputFilePath, blobId); //in OutputPath in order to have all files (input => output)

        _logger.LogInformation("File with '{extension}' is now being processed.", inputExtension);

        if (!_acceptedExtensions.Exists(x => x.Equals(inputExtension)))
        {
            switch (inputExtension)
            {
                case ".pdf":
                    finalFilePath = _fileManagemenet.ConvertPdfToPng(inputFilePath, blobId);
                    break;
                default:
                    throw new InvalidOperationException("Failed to convert to destination extension");
            }
        }

        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);

        using var img = Pix.LoadFromFile(finalFilePath);
        using var page = engine.Process(img);

        return new OcrResult
        {
            Text = page.GetText(),
            Confidence = page.GetMeanConfidence(),
            BlobId = blobId,
            InputExtension = inputExtension,
            OutputExtension = Path.GetExtension(finalFilePath)
        };
    }
}