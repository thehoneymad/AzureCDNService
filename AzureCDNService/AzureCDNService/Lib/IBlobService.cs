using AzureCDNService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureCDNService.Lib
{
    public interface IBlobService
    {
        Task<List<FileUploadModel>> UploadBlobs(HttpContent content);
        Task<FileDownloadModel> DownloadBlob(string blobName);

    }
}
