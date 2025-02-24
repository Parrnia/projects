namespace Onyx.Domain.Entities.FilesCluster;

public class StoredFile : BaseAuditableEntity
{
    public StoredFile()
    {
    }

    public StoredFile(Guid fileId, string extension, long fileSize, string folder,
        FileCategory category, string uploadName, DateTime uploadDate, Guid? owner)
    {
        FileId = fileId;
        Extension = extension;
        FileSize = fileSize;
        Folder = folder;
        Category = category;
        UploadName = uploadName;
        UploadDate = uploadDate;
        Owner = owner;
    }
    /// <summary>
    /// شناسه فایل
    /// </summary>
    public Guid FileId { get; set; }
    /// <summary>
    /// پسوند فایل
    /// </summary>
    public string Extension { get; set; } = null!;
    /// <summary>
    /// نام فایل
    /// </summary>
    public string FileName => $"{FileId}{Extension}";
    /// <summary>
    /// اندازه فایل
    /// </summary>
    public long FileSize { get; set; }
    /// <summary>
    /// مسیر پوشه فایل
    /// </summary>
    public string Folder { get; set; } = null!;
    /// <summary>
    /// دسته بندی فایل
    /// </summary>
    public FileCategory Category { get; set; }
    /// <summary>
    /// نام اصلی فایل آپلود شده
    /// </summary>
    public string UploadName { get; set; } = null!;
    /// <summary>
    /// زمان آپلود
    /// </summary>
    public DateTime UploadDate { get; set; }
    /// <summary>
    /// آپلود کننده فایل
    /// </summary>
    public Guid? Owner { get; set; }

}
