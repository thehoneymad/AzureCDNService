using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCDNService.Model
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSizeInBytes { get; set; }
        public long FileSizeInKb { get { return (long)Math.Ceiling((double)FileSizeInBytes / 1024); } }
        public long FileSizeInMb { get { return (long)Math.Ceiling((double)FileSizeInKb / 1024); } }
    }
}
