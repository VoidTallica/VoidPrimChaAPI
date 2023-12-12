using PrimaveraAPI.Class_;
using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.DTO
{
    [Entity(Table = "salesUnit")]
    public class SalesUnitDTO : BaseDTO<SalesUnitDTO, int>
    {
        public int id { get; set; }
        public string name { get; set; }
        public string location { get; set; }

        public override int GetPrimaryKey()
        {
            return id;
        }

        internal override BaseRepository<SalesUnitDTO, int> GetRepository()
        {
            return new SalesUnitRepository();
        }

    }
}
