using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Interfaces;
using EResultResponse = SystemReport.WebAPI.Helpers.EResultResponse;

namespace SystemReport.WebAPI.APIs.Identity
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FilesController(IFileService fileService, IWebHostEnvironment hostingEnvironment)
        {
            _fileService = fileService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("~/api/v1/files/upload")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var uploadDirecotroy = "files/";
                var uploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                var files = HttpContext.Request.Form.Files.Count > 0 ? HttpContext.Request.Form.Files : null;
                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy, dateTime);
                IFormFile file = null;
                if (files != null && files.Count > 0)
                {
                    file = files[0];
                }
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    FileInfo fileInfo = new FileInfo(fileName);
                    var extFile = fileInfo.Extension;
                    if (fileName.Length > 100)
                    {
                        throw new Exception("Tên tệp tin quá dài.");
                    }

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var newFileName = Guid.NewGuid().ToString() + extFile;
                    var relativePath = Path.Combine("", dateTime, newFileName);
                    var filePath = Path.Combine(uploadDirecotroy, relativePath);
                    var pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);
                    using (var strem = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(strem);
                    }
                    var result = await _fileService.SaveFileAsync(filePath, fileName, newFileName, extFile, file.Length);
                    return Ok(
                        new ResultResponse<Models.File>()
                            .WithData(result)
                            .WithCode(EResultResponse.SUCCESS.ToString())
                            .WithMessage("Tải tệp tin thành công.")
                    );
                }
                return Ok(
                    new ResultMessageResponse().WithCode(EResultResponse.FAIL.ToString())
                        .WithMessage("Tải tệp tin thất bại.")
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }
        [HttpPost]
        [Route("~/api/v1/files/uploadMulti")]
        public async Task<IActionResult> UploadFileMulti()
        {
            try
            {
                var uploadDirecotroy = "files/";
                var uploadPath = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                var files = HttpContext.Request.Form.Files.Count > 0 ? HttpContext.Request.Form.Files : null;
                var dateTime = DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss");
                var path = Path.Combine(_hostingEnvironment.ContentRootPath, uploadDirecotroy, dateTime);
                IFormFile file = null;
                if (files != null && files.Count > 0)
                {
                    file = files[0];
                }

                List<Models.File> modelFiles = new List<Models.File>();
                foreach (var item in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        FileInfo fileInfo = new FileInfo(fileName);
                        var extFile = fileInfo.Extension;
                        if (fileName.Length > 100)
                        {
                            throw new Exception("Tên tệp tin quá dài.");
                        }
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var newFileName = Guid.NewGuid().ToString() + extFile;
                        var relativePath = Path.Combine("", dateTime, newFileName);
                        var filePath = Path.Combine(uploadDirecotroy, relativePath);
                        var pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, filePath);
                        using (var strem = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(strem);
                        }

                        var result =
                            await _fileService.SaveFileAsync(filePath, fileName, newFileName, extFile, file.Length);
                        modelFiles.Add(result);
                    }
                }
                return Ok(
                    new ResultResponse<List<Models.File>>()
                        .WithData(modelFiles)
                        .WithCode(EResultResponse.SUCCESS.ToString())
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }

        [HttpGet]
        [Route("~/api/v1/files/view/{id}")]
        public async Task<IActionResult> ViewFile(string id)
        {
            HttpResponseMessage result = null;
            MemoryStream memory = new MemoryStream();
            var pathFile = "";
            try
            {
                var data = _fileService.GetById(id);
                if (data != null)
                {
                    var localFilePath = Path.Combine(data.Path);
                    pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, localFilePath);
                    var filename = data.FileName;
                    if (!System.IO.File.Exists(pathFile))
                    {
                        result = new HttpResponseMessage(HttpStatusCode.Gone);
                    }
                    else
                    {
                        var info = System.IO.File.GetAttributes(pathFile);
                        result = new HttpResponseMessage(HttpStatusCode.OK);
                        result.Content = new StreamContent(new FileStream(pathFile, FileMode.Open, FileAccess.Read));
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        result.Content.Headers.Add("x-filename", filename);
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                        result.Content.Headers.ContentDisposition.FileName = filename;
                        using (FileStream stream = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;
                        return File(memory, "application/octet-stream", filename);
                    }
                }
                return File(memory, "application/octet-stream", Path.GetFileName(pathFile));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        [HttpGet]
        [Route("~/api/v1/files/download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            HttpResponseMessage result = null;
            MemoryStream memory = new MemoryStream();
            var pathFile = "";
            try
            {
                var data = _fileService.GetById(id);
                if (data != null)
                {
                    var localFilePath = Path.Combine(data.Path);
                    pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, localFilePath);
                    var filename = data.FileName;
                    if (!System.IO.File.Exists(pathFile))
                    {
                        result = new HttpResponseMessage(HttpStatusCode.Gone);
                    }
                    else
                    {
                        var info = System.IO.File.GetAttributes(pathFile);
                        result = new HttpResponseMessage(HttpStatusCode.OK);
                        result.Content = new StreamContent(new FileStream(pathFile, FileMode.Open, FileAccess.Read));
                        result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        result.Content.Headers.Add("x-filename", filename);
                        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                        result.Content.Headers.ContentDisposition.FileName = filename;
                        using (FileStream stream = new FileStream(pathFile, FileMode.Open, FileAccess.Read))
                        {
                            await stream.CopyToAsync(memory);
                        }

                        memory.Position = 0;
                    }
                }

                return File(memory, "application/octet-stream", Path.GetFileName(pathFile));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }
    }
}