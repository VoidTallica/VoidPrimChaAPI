using PrimaveraAPI.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public class SalesUnitRepository : BaseRepository<SalesUnitDTO, int>
    {
        internal override async Task<SalesUnitDTO> GetAsync(SalesUnitDTO baseDTO, int key)
        {
            return await SendFirstAsync($"SELECT * FROM SalesUnit WITH(NOLOCK) WHERE id = '{key}'", baseDTO);
        }
        internal async Task<SalesUnitDTO> GetSalesUnitAsync(int key)
        {
            return await SendFirstAsync($"SELECT * FROM SalesUnit WITH(NOLOCK) WHERE id = '{key}'");
        }
        internal override async Task<List<SalesUnitDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM SalesUnit WITH(NOLOCK)");
        }
        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM SalesUnit WHERE id = {key}") == 1;
        }
        internal async Task<List<SalesUnitDTO>> GetSalesUnitsByNameAsync(string name)
        {
            return await SendListAsync($"SELECT * FROM SalesUnit WITH(NOLOCK) WHERE name = '{name}'");
        }
        internal async Task<int> findNextAvailableIndex()
        {
            var obj = await SendFirstAsync("SELECT (max(id) + 1) as id FROM SalesUnit");
            return obj.id;
        }
        internal async Task<bool> PostIntoDB(SalesUnitDTO sUnit/*, string userID*/)
        {
            try
            {
                var sql = $"INSERT INTO salesUnit ({nameof(sUnit.name)}, {nameof(sUnit.location)}) " +
                          $"VALUES ('{sUnit.name}', '{sUnit.location}');";

                return await RecordsAffected(sql) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal async Task<bool> EditAsync(SalesUnitDTO sUnit)
        {
            try
            {
                var sql = $"UPDATE salesUnit SET {nameof(sUnit.name)} = '{sUnit.name}', " +
                    $"{nameof(sUnit.location)} = '{sUnit.location}' " +
                    $" WHERE {nameof(sUnit.id)} = ' {sUnit.id}';";
                return await this.RecordsAffected(sql) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
