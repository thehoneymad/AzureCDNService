using AzureCDNService.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AzureCDNService.Controllers
{
    public class StorageController : ApiController
    {
        private readonly IBlobService _service;

        public StorageController(IBlobService service)
        {
            _service = service;
        }


        /// <summary>
        /// Upload one or more files to Azure Blob that is associated with a CDN.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                var result = await _service.UploadBlobs(Request.Content);
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetFile(string blobName)
        {
            try
            {
                var result = await _service.DownloadBlob(blobName);
                if (result == null)
                {
                    return NotFound();
                }

                // Reset the stream position; otherwise, download will not work
                result.BlobStream.Position = 0;

                // Create response message with blob stream as its content
                var message = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(result.BlobStream)
                };

                // Set content headers
                message.Content.Headers.ContentLength = result.BlobLength;
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(result.BlobContentType);
                message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlDecode(result.BlobFileName),
                    Size = result.BlobLength
                };

                return ResponseMessage(message);
            }
            catch (Exception ex)
            {
                return ResponseMessage(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                });
            }
        }
    }
}

