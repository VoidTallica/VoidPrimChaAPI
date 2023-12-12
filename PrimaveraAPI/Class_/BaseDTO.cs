using PrimaveraAPI.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrimaveraAPI.Class_
{
    public abstract class BaseDTO<C, K>
        where C : BaseDTO<C, K>, new()
    {
        public override bool Equals(object obj)
        {
            return (obj is C) ? ((C)obj).GetPrimaryKey().Equals(GetPrimaryKey()) : false;
        }

        internal abstract BaseRepository<C, K> GetRepository();
        public abstract K GetPrimaryKey();

        internal async Task<C> GetAsync() => await GetRepository().GetAsync((C)this, GetPrimaryKey());
        internal async Task<IEnumerable<C>> GetListAsync() => await GetRepository().GetListAsync();
        //internal async Task InsertOrUpdate() => await GetRepository().InsertOrUpdate((C)this);
        //internal async Task<C> Delete() => await GetRepository().DeleteAsync((C)this);
        public override string ToString() => Convert.ToString(GetPrimaryKey());
        public override int GetHashCode() => base.GetHashCode();

        public C Copy()
        {
            var obj = new C();
            foreach (var ps in this.GetType().GetProperties())
            {
                foreach (var pt in obj.GetType().GetProperties())
                {
                    if (pt.Name != ps.Name) continue;
                    try
                    {
                        pt.GetSetMethod().Invoke(obj, new object[] { ps.GetGetMethod().Invoke(this, null) });
                    }
                    catch (Exception)
                    { }
                    break;
                }
            };
            return obj;
        }
    }
}