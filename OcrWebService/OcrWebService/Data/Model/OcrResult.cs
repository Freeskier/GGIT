
namespace OcrWebService.Data.Model;

public class OcrResult
{
    public string? Text { get; set; }
    public float? Confidence { get; set; }
    public Guid BlobId { get; set; }
    public string? InputExtension { get; set; }
    public string? OutputExtension { get; set; }
}
