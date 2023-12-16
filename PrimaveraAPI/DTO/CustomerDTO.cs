using PrimaveraAPI.Class_;
using PrimaveraAPI.Repository;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrimaveraAPI.DTO
{
    public class CustomerDTO : BaseDTO<CustomerDTO, int>
    {
        [JsonIgnore]
        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string taxID { get; set; }
        public DateTime dateCreate { get; set; }
        public DateTime dateUpdate { get; set; }
        public override int GetPrimaryKey()
        {
            return id;
        }

        internal override BaseRepository<CustomerDTO, int> GetRepository()
        {
            return new CustomerRepository();
        }
        internal async Task<CustomerDTO> GetCustomerAsync(int key)
        {
            return await ((CustomerRepository)GetRepository()).GetCustomerAsync(key);
        }
        internal async Task<List<CustomerDTO>> GetCustomersByNameAsync(string nameCustomer)
        {
            return await ((CustomerRepository)GetRepository()).GetCustomersByNameAsync(nameCustomer);
        }
    }
}
