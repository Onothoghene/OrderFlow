//using Application.DTOs.File;
//using Application.Features.FileTemp.Command.Create;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class FileController : BaseApiController
    {
        //for doc. download
        //[AllowAnonymous]
        //[HttpGet("download/{fileuniqueName}")]
        //public async Task<IActionResult> DownloadFile(string fileuniqueName)
        //{
        //    return Ok(await Mediator.Send(new GetAzureLinkQuery { fileUniqueName = fileuniqueName }));
        //}

        //[AllowAnonymous]
        //[HttpGet("{fileuniqueName}/binary")]
        //public async Task<IActionResult> GetFileBinary(string fileuniqueName)
        //{
        //    return Ok(await Mediator.Send(new GetFileBinaryByFileUniqueNameQuery { fileUniqueName = fileuniqueName }));
        //}

        //[AllowAnonymous]
        //[HttpPost("upload-and-get-downloadable-link")]
        //public async Task<IActionResult> UploadFile(FileModel file)
        //{
        //    return Ok(await Mediator.Send(new UploadFileToAzure { 
        //        FileName = file.FileName, 
        //        FileExt = file.FileFormat, 
        //        FileBinary = file.FileBinary 
        //    }));
        //}

        //[AllowAnonymous]
        //[HttpGet("download-declaration")]
        //public async Task<IActionResult> DownloadDeclarationFile()
        //{
        //    return Ok(await Mediator.Send(new DownloadDeclarationFileCommand()));
        //}

        //for temporary upload
        //[HttpPost("Temp")]
        //public async Task<IActionResult> CreateFileTemp(CreateFileTempCommand command)
        //{
        //    return Ok(await Mediator.Send(command));
        //}
        
        //[HttpDelete("Temp/{Id}")]
        //public async Task<IActionResult> DeleteFileTemp(int Id)
        //{
        //    return Ok(await Mediator.Send(new DeleteFileTempCommand { Id = Id}));
        //}

        //[AllowAnonymous]
        //[HttpPost("upload-large-file")]
        //public async Task<IActionResult> UploadLargefileFile(IFormFile doc)
        //{
        //    var fileBinary = "";

        //    using (var ms = new MemoryStream())
        //    {
        //        doc.CopyTo(ms);
        //        var fileBytes = ms.ToArray();
        //        fileBinary = Convert.ToBase64String(fileBytes);
        //    }

        //    var file = new FileModel
        //    {
        //        FileBinary = fileBinary,
        //        FileName = doc.FileName
        //    };
        //    file.FileFormat = Path.GetExtension(file.FileName);

        //    return Ok(await Mediator.Send(new UploadFileToAzure
        //    {
        //        FileName = file.FileName,
        //        FileExt = file.FileFormat,
        //        FileBinary = file.FileBinary
        //    }));
        //}
    }
}
