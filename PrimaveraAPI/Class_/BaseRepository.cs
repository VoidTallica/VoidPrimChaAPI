using PrimaveraAPI.Class_;
using PrimaveraAPI.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PrimaveraAPI.Repository
{
    public abstract class BaseRepository<M, K>
        where M : BaseDTO<M, K>, new()
    {
        private readonly BindingFlags bindingFlags;
        internal string connectionString;

        public BaseRepository()
        {
            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            connectionString = DataContext.Connection;
        }

        internal abstract Task<M> GetAsync(M baseDTO, K key);
        internal abstract Task<List<M>> GetListAsync();

        internal virtual Task<bool> InsertOrUpdate(M baseDTO) => throw new NotImplementedException();
        internal virtual Task<bool> DeleteAsync(K key) => throw new NotImplementedException();

        internal virtual M ToDataBaseEntity(SqlDataReader rd, M obj, IEnumerable<string> fieldNames, PropertyInfo[] objProper)
        {
            foreach (PropertyInfo flDb in objProper)
            {
                try
                {
                    if (!fieldNames.Any(x => x == flDb.Name)) //verifica se a variavel da query existe no objeto
                        continue;

                    var res = rd[flDb.Name]; //valor

                    var str = Convert.ToString(res);

                    if (str == "{}") continue;

                    if (flDb.PropertyType == typeof(string))
                    {
                        res = str;
                    }
                    else if (flDb.PropertyType == typeof(bool))
                    {
                        if (str == "1")
                            res = true;
                        else if (str == "0" || str == "")
                            res = false;
                        else
                            res = bool.TryParse(str, out bool val) ? val : false;
                    }
                    else if (flDb.PropertyType == typeof(Nullable<int>) || flDb.PropertyType == typeof(int))
                    {
                        int.TryParse(str, out int val);
                        res = val;
                    }
                    else if (flDb.PropertyType == typeof(Nullable<decimal>) || flDb.PropertyType == typeof(decimal))
                    {
                        decimal.TryParse(str, out decimal val);
                        res = val;
                    }
                    else if (flDb.PropertyType == typeof(Nullable<long>) || flDb.PropertyType == typeof(long))
                    {
                        long.TryParse(str, out long val);
                        res = val;
                    }
                    else if (flDb.PropertyType == typeof(Nullable<DateTime>) || flDb.PropertyType == typeof(DateTime))
                    {
                        if (string.IsNullOrWhiteSpace(str))
                            res = default(DateTime);
                    }
                    else if (flDb.PropertyType == typeof(Nullable<TimeSpan>) || flDb.PropertyType == typeof(TimeSpan))
                    {
                        if (string.IsNullOrWhiteSpace(str))
                            res = default(TimeSpan);
                    }
                    else if (flDb.PropertyType == typeof(byte[]))
                    {
                        if (string.IsNullOrWhiteSpace(str))
                            res = null;
                    }

                    try
                    {
                        flDb.SetValue(obj, res, null);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                return obj;
            }
            catch (Exception)
            {
                return obj;
            }
            finally
            {
                obj = null;
            }
        }

        protected async Task<M> SendFirstAsync(string sqlQuery, M baseDTO = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                var rd = await cmd.ExecuteReaderAsync();

                if (!rd.HasRows)
                    return null;

                var fieldNames = Enumerable.Range(0, rd.FieldCount).Select(i => rd.GetName(i)).ToArray(); //variaveis da query
                var objProper = (baseDTO ?? new M()).GetType().GetProperties(bindingFlags); //variaveis do objeto

                while (rd.Read())
                {
                    var res = ToDataBaseEntity(rd, baseDTO ?? new M(), fieldNames, objProper);
                    if (baseDTO == null)
                        baseDTO = res;
                    break;
                }
                con.Close();
            }

            try
            {
                return baseDTO;
            }
            catch (Exception)
            {
                return baseDTO;
            }
            finally
            {
                GC.Collect(0);
                GC.WaitForPendingFinalizers();
            }
        }

        protected async Task<List<M>> SendListAsync(string sqlQuery)
        {
            var lst = new List<M>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.CommandTimeout = 60; //60 segundos
                var rd = await cmd.ExecuteReaderAsync();

                if (!rd.HasRows)
                    return lst;

                var fieldNames = Enumerable.Range(0, rd.FieldCount).Select(i => rd.GetName(i)).ToArray(); //variaveis da query
                var objProper = new M().GetType().GetProperties(bindingFlags); //variaveis do objeto

                while (rd.Read())
                {
                    lst.Add(ToDataBaseEntity(rd, new M(), fieldNames, objProper));
                }

                con.Close();
            }

            try
            {
                return lst;
            }
            catch (Exception)
            {
                return lst;
            }
            finally
            {
                lst = null;
                GC.Collect(0);
                GC.WaitForPendingFinalizers();
            }
        }

        protected async Task<int> RecordsAffected(string sqlQuery)
        {
            var recordsAffected = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                var rd = await cmd.ExecuteReaderAsync();
                recordsAffected = rd.RecordsAffected;
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
