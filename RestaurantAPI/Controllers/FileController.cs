using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers;

[Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "fileName" }, Duration = 1200)]
    public ActionResult GetFile([FromQuery] string fileName)
    {
        var rootPath = Directory.GetCurrentDirectory();
        var filePath = $"{rootPath}/PrivateFiles/{fileName}";

        var fileExists = System.IO.File.Exists(filePath);

        if (!fileExists) return NotFound();

        var contentProvider = new FileExtensionContentTypeProvider();
        contentProvider.TryGetContentType(fileName, out var contentType);

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, contentType, fileName);
    }

    // [HttpPost]
    // public ActionResult UploadFile([FromForm] IFormFile file)
    // {
    //     if (file != null && file.Length > 0)
    //     {
    //         var rootPath = Directory.GetCurrentDirectory();
    //         var fileName = file.FileName;
    //         var fullPath = $"{rootPath}/PrivateFiles/{fileName}";
    //         using (var stream = new FileStream(fullPath, FileMode.Create))
    //         {
    //             file.CopyTo(stream);
    //         }
    //
    //         return Ok();
    //     }
    //
    //     return BadRequest();
    // }
}