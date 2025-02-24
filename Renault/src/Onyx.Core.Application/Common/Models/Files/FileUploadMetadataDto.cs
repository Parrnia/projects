namespace Onyx.Application.Common.Models.Files;

public class FileUploadMetadataDto
{
    public Guid FileId { get; set; }
    public string UploadName { get; set; } = null!;
    public long FileSize { get; set; }
    public string Extension { get; set; } = null!;
}