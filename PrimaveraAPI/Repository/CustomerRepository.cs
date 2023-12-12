using PrimaveraAPI.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public class CustomerRepository : BaseRepository<CustomerDTO, int>
    {
        internal override async Task<CustomerDTO> GetAsync(CustomerDTO baseDTO, int key)
        {
            return await SendFirstAsync($"SELECT * FROM customer WITH(NOLOCK) WHERE id = '{key}'", baseDTO);
        }
        internal override async Task<List<CustomerDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM customer WITH(NOLOCK)");
        }
        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM customer WHERE id = {key}") == 1;
        }
        internal async Task<bool> PostIntoDB(CustomerDTO cust/*, string userID*/)
        {
            try
            {
                var sql = $"INSERT INTO customer ({nameof(cust.id)}, {nameof(cust.name)}, {nameof(cust.country)}, {nameof(cust.taxID)}, {nameof(cust.dateCreate)}, {nameof(cust.dateUpdate)}) " +
                          $"VALUES ('{cust.id}', '{cust.name}', '{cust.name}', '{cust.country}', '{cust.taxID}', " +
                          $"'{cust.dateCreate.ToString("yyyy-MM-dd HH:mm:ss.fff")}' , '{cust.dateUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff")}');";

                return await RecordsAffected(sql) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal async Task<bool> EditAsync(CustomerDTO cust)
        {
            try
            {
                var sql = $"UPDATE customer SET {nameof(cust.id)} = '{cust.id}', {nameof(cust.name)} = '{cust.name}', " +
                    $"{nameof(cust.country)} = '{cust.country}', {nameof(cust.taxID)} = '{cust.taxID}', " +
                    $"{nameof(cust.dateUpdate)} = '{cust.dateUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff")}'   " +
                    $" WHERE {nameof(cust.id)} = ' {cust.id}';";
                return await this.RecordsAffected(sql) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
