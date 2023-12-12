using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimaveraAPI.DTO
{
    public class FileListVMDTO
    {
        public FileVM[] FileList { get; set; }
    }
    public class FileVM
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FileContent { get; set; }
    }
}

