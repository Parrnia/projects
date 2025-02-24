using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Onyx.Application.Services;

public static class FormFileExtensions
{
    public static async Task<byte[]?> ToByteArrayAsync(this IFormFile? file)
    {
        if (file == null)
        {
            return null;
        }
        await using MemoryStream memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    public static FileResult? ConvertByteArrayToFile(this byte[]? fileBytes)
    {
        if (fileBytes == null)
        {
            return null;
        }

        return new FileContentResult(fileBytes, "image/jpeg");
    }
}