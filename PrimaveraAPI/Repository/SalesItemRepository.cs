using PrimaveraAPI.Data;
using PrimaveraAPI.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public class SalesItemRepository: BaseRepository<SalesItemDTO, int>
    {
        internal override async Task<SalesItemDTO> GetAsync(SalesItemDTO baseDTO, int key)
        {
            return await SendFirstAsync($"SELECT * FROM salesItem WITH(NOLOCK) WHERE id = '{ key }'", baseDTO);
        }
        internal async Task<SalesItemDTO> GetSalesItemAsync(int key)
        {
            return await SendFirstAsync($"SELECT * FROM salesItem WITH(NOLOCK) WHERE id = '{ key }'");
        }
        internal async Task<List<SalesItemDTO>> GetSalesItemByNameAsync(string nameSalesItem)
        {
            return await SendListAsync("SELECT * FROM salesItem WITH(NOLOCK) WHERE name LIKE '" + nameSalesItem + "'");
        }
        internal override async Task<List<SalesItemDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM salesItem WITH(NOLOCK)");
        }
        internal async Task<List<SalesItemDTO>> GetListWithCategory(string category)
        {
            return await SendListAsync("SELECT * FROM salesItem WITH(NOLOCK) WHERE category = '" + category + "'");
        }
        internal async Task<List<SalesItemDTO>> GetListAvailable()
        {
            return await SendListAsync("SELECT * FROM salesItem WITH(NOLOCK) WHERE isAvailable = 1");
        }
        internal async Task<List<SalesItemDTO>> GetListAvailableWithCategory(string category)
        {
            return await SendListAsync("SELECT * FROM salesItem WITH(NOLOCK) WHERE isAvailable = 1 " +
                "AND category = '" + category + "'");
        }
        
        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM salesItem WHERE id = { key }") == 1;
        }

        internal async Task<bool> PostIntoDB(SalesItemDTO sItem/*, string userID*/)
        {
            try
            {
                var sql = $"INSERT INTO salesItem ({nameof(sItem.id)}, {nameof(sItem.name)}, {nameof(sItem.description)}, {nameof(sItem.price)}, " +
                    $"{nameof(sItem.category)}, {nameof(sItem.dateCreate)}, {nameof(sItem.dateUpdate)}, {nameof(sItem.imageId)}, {nameof(sItem.imgPath)}, " +
                    $"{nameof(sItem.isHighlighted)}, {nameof(sItem.isAvailable)}) " +
                          $"VALUES ('{sItem.id}', '{sItem.name}', '{sItem.description}', '{sItem.price}', '{sItem.category}', " +
                          $"'{sItem.dateCreate.ToString("yyyy-MM-dd HH:mm:ss.fff")}' , '{sItem.dateUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', " +
                          $"'{sItem.imageId}', '{sItem.imgPath}', '{sItem.isHighlighted}', '{sItem.isAvailable}');";

                return await RecordsAffected(sql) == 1;
                //If i want to pass a parameter like a big text, i should create a param and the use this method
                //RecordsAffectedWithTextParam(sql, sItem.paramText) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal async Task<bool> EditAsync(SalesItemDTO sItem)
        {
            try
            {
                var sql = $"UPDATE salesItem SET {nameof(sItem.id)} = '{sItem.id}', {nameof(sItem.name)} = '{sItem.name}', " +
                    $"{nameof(sItem.description)} = '{sItem.description}', {nameof(sItem.price)} = '{sItem.price}', " +
                    $"{nameof(sItem.category)} = '{sItem.category}', " +
                    $"{nameof(sItem.dateUpdate)} = '{sItem.dateUpdate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', " +
                    $"{nameof(sItem.imgPath)} = '{sItem.imgPath}', {nameof(sItem.isHighlighted)} = '{sItem.isHighlighted}', " +
                    $"{nameof(sItem.isAvailable)} = '{sItem.isAvailable}    " +
                    $" WHERE {nameof(sItem.id)} = ' {sItem.id}';";
                return await this.RecordsAffected(sql) == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal async Task<bool> EditAvailableAsync(SalesItemDTO sItem)
        {
            try
            {
                var sql = $"UPDATE salesItem SET {nameof(sItem.id)} = '{sItem.id}', {nameof(sItem.isAvailable)} = '{sItem.isAvailable}'" +
                    $" WHERE {nameof(sItem.id)} = ' {sItem.id}';";
                return await this.RecordsAffected(sql) == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal async Task<bool> EditThumbnailAsync(SalesItemDTO sItem)
        {
            try
            {
                var sql = $"UPDATE salesItem SET {nameof(sItem.id)} = '{sItem.id}', {nameof(sItem.imgPath)} = (@text)" +
                    $" WHERE {nameof(sItem.id)} = ' {sItem.id}';";
                return await this.RecordsAffectedWithTextParam(sql, sItem.imgPath) == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal async Task<int> findNextAvailableIndex()
        {
            var obj = await SendFirstAsync("SELECT (max(id) + 1) as id from salesItem");
            return obj.id;
        }
        private async Task<int> RecordsAffectedWithTextParam(string sqlQuery, string salesItemText)
        {
            string connectionString = DataContext.Connection;
            var recordsAffected = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@Text", salesItemText);
                var rd = cmd.ExecuteNonQuery();
                //var rd = await cmd.ExecuteReaderAsync();
                recordsAffected = rd;
                con.Close();
            }
            try
            {
                return recordsAffected;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                GC.Collect(0);
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
