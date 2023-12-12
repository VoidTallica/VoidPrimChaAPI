using PrimaveraAPI.Class_;
using PrimaveraAPI.DTO;
using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.Models
{
    [Entity(Table = "salesItem")]
    public class SalesItemDTO : BaseDTO<SalesItemDTO, int>
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string category { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateUpdate { get; set; }
        public int imageId { get; set; }
        public string imgPath { get; set; }
        public bool isHighlighted { get; set; }
        public bool isAvailable { get; set; }

        public override int GetPrimaryKey()
        {
            return id;
        }

        internal override BaseRepository<SalesItemDTO, int> GetRepository()
        {
            return new SalesItemRepository();
        }
        internal async Task<SalesItemDTO> GetSalesItemAsync(int key)
        {
            return await ((SalesItemRepository)GetRepository()).GetSalesItemAsync(key);
        }
        internal async Task<List<SalesItemDTO>> GetSalesItemByNameAsync(string nameSalesItem)
        {
            return await ((SalesItemRepository)GetRepository()).GetSalesItemByNameAsync(nameSalesItem);
        }
        internal async Task<List<SalesItemDTO>> GetListWithCategory(string category)
        {
            return await ((SalesItemRepository)GetRepository()).GetListWithCategory(category);
        }
        internal async Task<List<SalesItemDTO>> GetListAvailable()
        {
            return await ((SalesItemRepository)GetRepository()).GetListAvailable();
        }
        internal async Task<List<SalesItemDTO>> GetListAvailableWithCategory(string category)
        {
            return await ((SalesItemRepository)GetRepository()).GetListAvailableWithCategory(category);
        }
        
    }
}
