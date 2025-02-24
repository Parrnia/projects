using Onyx.Application.Common.Models.Files;
using Onyx.Domain.Enums;

namespace Onyx.Application.Common.Interfaces;
public interface IFileStore
{
    Task<FileUploadMetadataDto> SaveTempFile(Stream stream, string uploadName);
    Task<FileUploadMetadataDto?> GetTempMetadata(Guid fileId);
    Task<Stream?> LoadTempFile(Guid fileId);
    Task<bool> RemoveTempFile(Guid fileId);


    Task<StoredFileDto?> GetStoredFileInfo(Guid fileId, CancellationToken cancellationToken);
    Task SaveStoredFile(Guid tempFileId, string folder, FileCategory category, Guid? owner,
        bool overwrite, CancellationToken cancellationToken);
    Task<Stream?> LoadStoredFile(Guid fileId, CancellationToken cancellationToken);
    Task<bool> RemoveStoredFile(Guid fileId, CancellationToken cancellationToken);

}

