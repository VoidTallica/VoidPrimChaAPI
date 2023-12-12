using PrimaveraAPI.Class_;
using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrimaveraAPI.DTO
{
    [Entity(Table = "file")]
    public class FileDTO : BaseDTO<FileDTO, int>
    {
        public int fileId { get; set; }
        public int salesItemId { get; set; }
        public string fileName { get; set; }
        public byte[] fileContent { get; set; }
        public string contentType { get; set; }

        public override int GetPrimaryKey()
        {
            return fileId;
        }

        internal override BaseRepository<FileDTO, int> GetRepository()
        {
            return new FileRepository();
        }

        internal async Task<FileDTO> GetFileAsync(int key)
        {
            return await ((FileRepository)GetRepository()).GetFileAsync(key);
        }
    }

}
