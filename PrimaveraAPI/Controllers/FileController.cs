using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PrimaveraAPI.Class_;
using PrimaveraAPI.DTO;
using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PrimaveraAPI.Controllers
{
    [EnableCors(Cors.CorsPolicy)]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        #region Upload files to backEnd

        [HttpPost("fileUpload"),DisableRequestSizeLimit]
        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #endregion
        #region Upload files to database
        [HttpPost("dbFileUpload")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadDB(FileListVMDTO file)
        {
            try
            {
                //if (file.fileId == 0)
                //{
                //    file.fileId = await ((FileRepository)file.GetRepository()).findNextAvailableIndex();
                //}
                List<FileDTO> DocList = await GetDocList(file.FileList);
                if (!(await ((FileRepository)DocList[0].GetRepository()).PostListIntoDB(DocList)))
                    return NotFound(new { message = "Invalid file" });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dbImagesSalesItems")]
        [AllowAnonymous]
        public async Task<List<FileDTO>> GetImagesFromSalesItemsDB()
        {
            FileDTO file = new FileDTO();
            return await ((FileRepository)file.GetRepository()).GetListWithSalesItemsAsync();
        }

        private async Task<List<FileDTO>> GetDocList(FileVM[] lstDocVM)
        {
            //converting document array received to database document table list
            List<FileDTO> DBDocList = new List<FileDTO>();
            foreach (var Doc in lstDocVM)
            {
                // dividing file content from file type
                Doc.FileContent = Doc.FileContent.Substring(Doc.FileContent.IndexOf(",") + 1);
                var file = new FileDTO()
                {
                    fileName = Doc.FileName,
                    fileContent = Convert.FromBase64String(Doc.FileContent),
                    contentType = Doc.ContentType
                };
                file.fileId = await((FileRepository)file.GetRepository()).findNextAvailableIndex();
                DBDocList.Add(file);
            }
            return DBDocList;
        }

        [HttpGet("dbDownloadFile/{FileId}")]
        public async Task<IActionResult> DownloadDoumentDB(long FileId)
        {
            try
            {
                var file = new FileDTO();
                var listFile = await((FileRepository)file.GetRepository()).GetListAsync();
                FileDTO doc = listFile.FirstOrDefault(x => x.fileId == FileId);
                return File(doc.fileContent, doc.contentType, doc.fileName);
            }
            catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }

        }
        #endregion
    }
}
