using PrimaveraAPI.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public class FileRepository : BaseRepository<FileDTO, int>
    {
        internal override async Task<FileDTO> GetAsync(FileDTO baseDTO, int key)
        {
            return await SendFirstAsync($"SELECT * FROM [file] WITH(NOLOCK) WHERE fileId = '{ key }'", baseDTO);
        }
        internal async Task<FileDTO> GetFileAsync(int key)
        {
            return await SendFirstAsync($"SELECT * FROM [file] WITH(NOLOCK) WHERE fileId = '{ key }'");
        }

        internal override async Task<List<FileDTO>> GetListAsync()
        {
            return await SendListAsync("SELECT * FROM [file] WITH(NOLOCK)");
        }
        internal async Task<List<FileDTO>> GetListWithSalesItemsAsync()
        {
            return await SendListAsync("SELECT * FROM [file] WITH(NOLOCK) WHERE salesItemId > 0");
        }

        internal override async Task<bool> DeleteAsync(int key)
        {
            return await RecordsAffected($"DELETE FROM [file] WHERE fileId = { key }") == 1;
        }

        internal async Task<bool> PostIntoDB(FileDTO file)
        {
            try
            {
                var sql = $"INSERT INTO [file] ( fileName, fileContent, contentType, salesItemId) " +
                            $"VALUES ('{file.fileName}', '{file.fileContent}', '{file.contentType}', '{file.salesItemId}');";

                return await RecordsAffected(sql) == 1;
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }
        internal async Task<bool> PostListIntoDB(List<FileDTO> listFiles)
        {
            try
            {
                foreach (var file in listFiles)
                {
                    
                    var sql = $"INSERT INTO [file] ( fileName, fileContent, contentType, salesItemId) " +
                          $"VALUES ('{file.fileName}', @File, '{file.contentType}', '{file.salesItemId}');";
                    var sucess = await RecordsAffectedWithParams(sql,file) == 1;
                        if (!sucess)
                            return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return false;
            }
        }
        internal async Task<bool> PostUpdateDocIntoDB(FileDTO file)
        {
            try
            {
                var sql = $"INSERT INTO [file] ( fileName, fileContent, contentType, salesItemId) " +
                        $"VALUES ('{file.fileName}', @File, '{file.contentType}', '{file.salesItemId}');";
                var salesItemId = await FindfileIdFromSalesItemId(file.salesItemId);
                if(salesItemId != 0)
                {
                    sql = $"UPDATE [file] SET fileName = '{file.fileName}', fileContent = @File, contentType = '{file.contentType}' WHERE fileId = {salesItemId} AND salesItemId = {file.salesItemId}";
                }
                
                var sucess = await RecordsAffectedWithParams(sql, file) == 1;
                return true;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return false;
            }
        }
        protected async Task<int> RecordsAffectedWithParams(string sqlQuery, FileDTO file)
        {
            var recordsAffected = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Insert Byte [] Value into Sql Table by SqlParameter
                SqlCommand insertCommand = new SqlCommand(sqlQuery, con);
                SqlParameter sqlParam = insertCommand.Parameters.AddWithValue("@File", file.fileContent);
                sqlParam.DbType = DbType.Binary;
                insertCommand.ExecuteNonQuery();
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
        
        internal async Task<bool> EditAsync(FileDTO file)
        {
            try
            {
                var sql = $"UPDATE [file] SET name = '{file.fileName}', author = '{file.fileContent}', title = '{file.contentType}', " +
                    $" WHERE id = ' {file.fileId}';";

                return await RecordsAffected(sql) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal async Task<int> findNextAvailableIndex()
        {
            var obj = await SendFirstAsync("SELECT (max(fileId) + 1) as fileId from [file]");
            if(obj.fileId == 0)
                return 1;
         
            return obj.fileId;
        }
        internal async Task<int> GetImageFromSalesItem(int id)
        {
            var obj = await SendFirstAsync("SELECT salesItemId FROM [file] WHERE salesItemID = " + id);
            return obj.salesItemId;
        }
        internal async Task<int> FindfileIdFromSalesItemId(int salesItemId)
        {
            var obj = await SendFirstAsync("SELECT fileId FROM [file] WHERE salesItemID = " + salesItemId);
            return obj.fileId;
        }
    }
}
