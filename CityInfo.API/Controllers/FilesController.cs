using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private FileExtensionContentTypeProvider _contentTypeProvider;

        // we inject a Fileextension content type provider
        // that will find the content type from the extension
        // and give us the file with proper content type
        // based on the type of file
        // we need to injet this as a singleton in program.cs
        public FilesController(FileExtensionContentTypeProvider contentTypeProvider)
        {
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(int fileId)
        {
            try
            {
                // how download resource from an api end point
                // hard coded file path
                // in real life path will come from the database
                var pathToFile = "static/File/linux_book.pdf";
                // check if the file exists
                if (!System.IO.File.Exists(pathToFile))
                {
                    return NotFound("File does not exist");
                }
                // if the File Exists then
                // find the file name from path
                // and  the content type
                var filename = Path.GetFileName(pathToFile);


                /*
                    The TryGetContentType() method is called on an instance of 
                    the _fileExtensionContentTypeProvider object, 
                    passing the pathToFile variable as the argument.
                    If the method is successful in determining the content type 
                    of the file based on its extension, it will return true and the 
                    content type will be assigned to the contentType variable using 
                    the out keyword.

                    If the method is unsuccessful in determining the content type of
                    the file, the contentType variable will be assigned the value of
                    "application/octet-stream".
                    This content type is a default value that is often used when the
                    actual content type of a file is unknown or cannot be determined.

                    Overall, this code snippet is a simple way to determine the content
                    type of a file based on its extension, and it provides a fallback
                    option if the content type cannot be determined.
                */








                if (!_contentTypeProvider.TryGetContentType(pathToFile, out var contentType)){
                    // if the content type found then it autometically assigned in contentType
                    // otherwise this will assign 
                    // we dont  need the return value and assign it to a variable
                    //out keyword take care of this 
                    // "application/octet-stream" is default 
                    // that accept everything
                    contentType = "application/octet-stream";
                }

                // set the content type now read the file
                var bytes = System.IO.File.ReadAllBytes(pathToFile);
                return File(bytes,contentType,filename);


            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }


    }
}