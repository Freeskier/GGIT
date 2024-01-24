namespace OcrWebService.Data.Entity;

public class OcrResultEntity
{
    public Guid Id { get; set; }
    public Guid BlobId { get; set; }
    public string? Name { get; set; }
    public string? InputExtension { get; set; }
    public string? OutputExtension { get; set; }
    public long? Size { get; set; }
    public float? Confidence { get; set; }
    public string? Content { get; set; }
}
