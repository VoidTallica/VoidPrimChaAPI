﻿using PrimaveraAPI.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public class CustomerRepository : BaseRepository<CustomerDTO, int>
    {
        internal override async Task<CustomerDTO> GetAsync(CustomerDTO baseDTO, int key)
        {
            return await SendFirstAsync($"SELECT * FROM Customer WITH(NOLOCK) WHERE id = '{key}'", baseDTO);
        }
        internal async Task<CustomerDTO> GetCustomerAsync(int key)
        {
            return await SendFirstAsync($"SELECT * FROM Customer WITH(NOLOCK) WHERE id = '{key}'");
        }
        internal override async Task<List<CustomerDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM Customer WITH(NOLOCK)");
        }
        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM Customer WHERE id = {key}") == 1;
        }
        internal async Task<List<CustomerDTO>> GetCustomersByNameAsync(string name)
        {
            return await SendListAsync($"SELECT * FROM Customer WITH(NOLOCK) WHERE name = '{name}'");
        }
        internal async Task<int> findNextAvailableIndex()
        {
            var obj = await SendFirstAsync("SELECT (max(id) + 1) as id FROM Customer");
            return obj.id;
        }
        internal async Task<bool> PostIntoDB(CustomerDTO cust/*, string userID*/)
        {
            try
            {
                var sql = $"INSERT INTO Customer ( {nameof(cust.username)},{nameof(cust.name)}, {nameof(cust.country)}, {nameof(cust.taxID)}, {nameof(cust.dateCreate)}, {nameof(cust.dateUpdate)}) " +
                          $"VALUES ( '{cust.username}', '{cust.name}', '{cust.country}', '{cust.taxID}', " +
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
                var sql = $"UPDATE Customer SET {nameof(cust.name)} = '{cust.name}', " +
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
