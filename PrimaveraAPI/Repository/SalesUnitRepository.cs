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
            return await SendFirstAsync($"SELECT * FROM salesUnit WITH(NOLOCK) WHERE id = '{key}'", baseDTO);
        }
        internal override async Task<List<SalesUnitDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM salesUnit WITH(NOLOCK)");
        }
        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM salesUnit WHERE id = {key}") == 1;
        }
        internal async Task<bool> PostIntoDB(SalesUnitDTO sUnit/*, string userID*/)
        {
            try
            {
                var sql = $"INSERT INTO salesUnit ({nameof(sUnit.id)}, {nameof(sUnit.name)}, {nameof(sUnit.location)}) " +
                          $"VALUES ('{sUnit.id}', '{sUnit.name}', '{sUnit.name}', '{sUnit.location}');";

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
                var sql = $"UPDATE salesUnit SET {nameof(sUnit.id)} = '{sUnit.id}', {nameof(sUnit.name)} = '{sUnit.name}', " +
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
