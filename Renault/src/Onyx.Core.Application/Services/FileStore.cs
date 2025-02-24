using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Models.Files;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.FilesCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Services;
public class FileStore : IFileStore
{
    private readonly ApplicationSettings _applicationSettings;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FileStore(IOptions<ApplicationSettings> applicationSettings,
        IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<FileUploadMetadataDto> SaveTempFile(Stream stream, string uploadName)
    {
        var metadata = new FileUploadMetadataDto()
        {
            Extension = Path.GetExtension(uploadName),
            FileSize = (int)stream.Length,
            UploadName = uploadName,
            FileId = Guid.NewGuid()
        };

        Directory.CreateDirectory(_applicationSettings.UploadTempFolder);
        var filePath = Path.Combine(_applicationSettings.UploadTempFolder, metadata.FileId.ToString());
        var metaPath = Path.Combine(_applicationSettings.UploadTempFolder, $"{metadata.FileId}.meta");
        await using var file = File.Open(filePath, FileMode.Create, FileAccess.Write);
        await stream.CopyToAsync(file);
        // write metadata
        await File.WriteAllTextAsync(metaPath, JsonSerializer.Serialize(metadata));
        return metadata;
    }

    public async Task<FileUploadMetadataDto?> GetTempMetadata(Guid fileId)
    {
        var metaPath = Path.Combine(_applicationSettings.UploadTempFolder, $"{fileId}.meta");

        if (!File.Exists(metaPath))
            return null;

        var metaContent = await File.ReadAllTextAsync(metaPath);
        return JsonSerializer.Deserialize<FileUploadMetadataDto?>(metaContent);
    }

    public async Task<Stream?> LoadTempFile(Guid fileId)
    {
        var filePath = Path.Combine(_applicationSettings.UploadTempFolder, fileId.ToString());

        if (!File.Exists(filePath))
            return null;

        return File.Open(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<bool> RemoveTempFile(Guid fileId)
    {
        var metaPath = Path.Combine(_applicationSettings.UploadTempFolder, $"{fileId}.meta");
        var filePath = Path.Combine(_applicationSettings.UploadTempFolder, fileId.ToString());

        var result = true;
        if (File.Exists(metaPath))
        {
            try
            {
                File.Delete(metaPath);
            }
            catch
            {
                // ignore
            }
        }

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                result = false;
            }
        }

        return result;
    }




    public async Task SaveStoredFile(Guid tempFileId, string folder, FileCategory category, Guid? owner,
        bool overwrite, CancellationToken cancellationToken)
    {
        var meta = await GetTempMetadata(tempFileId);
        var content = await LoadTempFile(tempFileId);

        if (meta is null || content is null)
            throw new ServiceException("فایل یافت نشد.");

        var fileInfo = new StoredFile(tempFileId, meta.Extension, meta.FileSize, folder,
            category, meta.UploadName, DateTime.Now, owner);

        await _context.StoredFiles.AddAsync(fileInfo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var filePath = CreateFullDirectoryPath(_applicationSettings.PersistFolder, folder);
        filePath = Path.Combine(filePath, tempFileId.ToString());

        if (File.Exists(filePath))
        {
            if (overwrite)
                File.Delete(filePath);
            else
                throw new ServiceException("نام فایل وارد شده تکراری می باشد.");
        }

        await using (content)
        await using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
            await content.CopyToAsync(file, cancellationToken);

        await RemoveTempFile(tempFileId);
    }

    public async Task<StoredFileDto?> GetStoredFileInfo(Guid fileId,
        CancellationToken cancellationToken)
    {
        var fileInfo = await _context.StoredFiles.FirstOrDefaultAsync(i => i.FileId == fileId,
            cancellationToken: cancellationToken);

        return _mapper.Map<StoredFileDto>(fileInfo);
    }

    public async Task<Stream?> LoadStoredFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileInfo = await _context.StoredFiles.FirstOrDefaultAsync(i => i.FileId == fileId,
            cancellationToken: cancellationToken);
        if (fileInfo is null)
            throw new ServiceException("اطلاعات فایل درخواست شده یافت نشد.");

        var filePath = Path.Combine(_applicationSettings.PersistFolder, fileInfo.Folder, fileInfo.FileId.ToString());
        if (!File.Exists(filePath))
            return null;

        return File.Open(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<bool> RemoveStoredFile(Guid fileId, CancellationToken cancellationToken)
    {
        var fileInfo = await _context.StoredFiles.FirstOrDefaultAsync(i => i.FileId == fileId,
            cancellationToken: cancellationToken);

        if (fileInfo is null)
            throw new ServiceException("اطلاعات فایل درخواست شده یافت نشد.");

        var filePath = Path.Combine(_applicationSettings.PersistFolder, fileInfo.Folder, fileInfo.FileId.ToString());

        _context.StoredFiles.Remove(fileInfo);

        var result = true;
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                result = false;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }


    private string CreateFullDirectoryPath(string basePath, string relativePath)
    {
        var pathParts = relativePath.Split(new[] { '/', '\\' },
            StringSplitOptions.RemoveEmptyEntries);

        var currentPath = basePath;
        Directory.CreateDirectory(currentPath);
        foreach (var part in pathParts)
        {
            currentPath = Path.Combine(currentPath, part);
            Directory.CreateDirectory(currentPath);
        }

        return currentPath;
    }

}

