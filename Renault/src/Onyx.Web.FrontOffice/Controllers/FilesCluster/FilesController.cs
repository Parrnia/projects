using Microsoft.AspNetCore.Mvc;
using Onyx.Application;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Models.Files;
using Onyx.Web.FrontOffice.Authorization;
using OnyxAuth.Shared.Attributes;
using OnyxAuth.Shared;

namespace Onyx.Web.FrontOffice.Controllers.FilesCluster;

public class FilesController : ApiControllerBase
{
    private readonly IFileStore _fileStore;

    public FilesController(IFileStore fileStore)
    {
        _fileStore = fileStore;
    }

    [HttpPost("temp")]
    [PermissionAuthorize]
    [RequestSizeLimit(AppConstants.MaxRequestSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = AppConstants.MaxRequestSize)]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<FileUploadMetadataDto>> UploadTemp(IFormFile file)
    {
        await using var fileStream = file.OpenReadStream();
        var fileName = file.FileName;
        var result = await _fileStore.SaveTempFile(fileStream, fileName);
        return result;
    }

    [HttpGet("temp/{fileId}")]
    public async Task<IActionResult> DownloadTemp(Guid fileId)
    {
        var metaData = await _fileStore.GetTempMetadata(fileId);
        var fileStream = await _fileStore.LoadTempFile(fileId);

        if (metaData is null || fileStream is null)
            return NotFound();
        var mimeType = GetMimeType(metaData.UploadName);
        return File(fileStream, mimeType, metaData.UploadName);
    }


    [HttpGet("temp/{fileId}/info")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<FileUploadMetadataDto?>> GetTempInfo(Guid fileId)
    {
        var metaData = await _fileStore.GetTempMetadata(fileId);
        return metaData;
    }


    [HttpDelete("temp/{fileId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<bool>> DeleteTemp(Guid fileId)
    {
        var result = await _fileStore.RemoveTempFile(fileId);
        return result;
    }



    [HttpGet("store/{fileId}")]
    public async Task<IActionResult> DownloadPersisted(Guid fileId,
        CancellationToken cancellationToken)
    {
        var metaData = await _fileStore.GetStoredFileInfo(fileId, cancellationToken);
        var fileStream = await _fileStore.LoadStoredFile(fileId, cancellationToken);

        if (metaData is null || fileStream is null)
            return NotFound();
        var mimeType = GetMimeType(metaData.UploadName);
        return File(fileStream, mimeType, metaData.UploadName);
    }
  

    [HttpGet("store/{fileId}/info")]
    public async Task<ActionResult<StoredFileDto?>> GetInfoPersisted(Guid fileId,
        CancellationToken cancellationToken)
    {
        var metaData = await _fileStore.GetStoredFileInfo(fileId, cancellationToken);

        //metaData.Folder = null;
        return metaData;
    }

    [HttpDelete("store/{fileId}")]
    [PermissionAuthorize]
    [CheckPermission(Roles.Customer, null)]
    public async Task<ActionResult<bool>> DeletePersisted(Guid fileId,
        CancellationToken cancellationToken)
    {
        var result = await _fileStore.RemoveStoredFile(fileId, cancellationToken);
        return result;
    }

    private string GetMimeType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };
    }
}


