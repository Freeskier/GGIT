using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OcrWebService.Controllers;
using OcrWebService.Data.Entity;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace OcrWebService.Services;

public class FileManagementService
{
    private readonly ILogger<FileManagementService> _logger;
    private readonly IConfiguration _configuration;

    public FileManagementService(ILogger<FileManagementService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public string ConvertPdfToPng(string path, Guid blobId)
    {
        _logger.LogInformation("Started processing '{ConvertPdfToPng}' conversion from PDF to PNG.", nameof(ConvertPdfToPng));

        try
        {
            var outputPath = Path.Combine(_configuration["OutputFilePath"], blobId + ".png");
            var bytes = ConvertFileToBytes(path);

#pragma warning disable CA1416 // Validate platform compatibility
            PDFtoImage.Conversion.SavePng(outputPath, bytes);
            _logger.LogInformation("Successfully converted and saved file in '{outputPath}' with PNG extension.", outputPath);

            return outputPath;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to render PDF to PNG: '{ex}'.", ex);
            return string.Empty;
        }
    }

    public void CreateOutputFile(string text, OcrResultEntity ocrResult)
    { 
        //todo check if directory exist
        _logger.LogInformation("Saving output file '{0}' in output path.", ocrResult.BlobId);

        var outputPath = $"./OutputPath/{ocrResult.BlobId}.txt";

        try
        {
            using FileStream fs = File.Create(outputPath);
            byte[] info = new UTF8Encoding(true).GetBytes(text);
            fs.Write(info, 0, info.Length);

            ocrResult.Size = info.Length;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public string SaveInputFileInOutputPath(string path, Guid blobId, byte[]? data = null)
    { 
        //todo check if directory exist
        _logger.LogInformation("Saving input file '{0}' in output path.", path);

        var outputPath = $"./OutputPath/{blobId}.{Path.GetExtension(path)}";

        try
        {
            // Create the file, or overwrite if the file exists.
            using FileStream fs = File.Create(outputPath);

            if (data == null)
                data = ConvertFileToBytes(path);

            fs.Write(data, 0, data.Length);

            return outputPath;
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return string.Empty;
        }
    }

    private static byte[] ConvertFileToBytes(string path)
    {
        var fileAsBytes = File.ReadAllBytes(path);

        return fileAsBytes;
    }
}
