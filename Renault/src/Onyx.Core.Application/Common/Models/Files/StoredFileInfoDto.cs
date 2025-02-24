using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.FilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Common.Models.Files;

public class StoredFileDto : IMapFrom<StoredFile>
{
    public int Id { get; set; }
    public Guid FileId { get; set; }
    public string Extension { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public long FileSize { get; set; }
    public string Folder { get; set; } = null!;
    public FileCategory Category { get; set; }
    public string UploadName { get; set; } = null!;
    public DateTime UploadDate { get; set; }
    public Guid? Owner { get; set; }

}