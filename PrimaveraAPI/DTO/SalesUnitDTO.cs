using PrimaveraAPI.Class_;
using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrimaveraAPI.DTO
{
    [Entity(Table = "salesUnit")]
    public class SalesUnitDTO : BaseDTO<SalesUnitDTO, int>
    {
        [JsonIgnore]
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
        internal async Task<SalesUnitDTO> GetSalesUnitAsync(int key)
        {
            return await ((SalesUnitRepository)GetRepository()).GetSalesUnitAsync(key);
        }
        internal async Task<List<SalesUnitDTO>> GetSalesUnitsByNameAsync(string nameCustomer)
        {
            return await ((SalesUnitRepository)GetRepository()).GetSalesUnitsByNameAsync(nameCustomer);
        }
        

    }
}
