using PrimaveraAPI.Class_;
using PrimaveraAPI.Repository;
using System;

namespace PrimaveraAPI.DTO
{
    public class CustomerDTO : BaseDTO<CustomerDTO, int>
    {
        public int id { get; set; }
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

    }
}
}
