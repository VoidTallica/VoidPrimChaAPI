using System;

namespace PrimaveraAPI.Class_
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public string Table;
        public string Name;

        public EntityAttribute()
        {
            Table = string.Empty;
        }

        public EntityAttribute(string Table, string Name)
        {
            this.Table = Table;
            this.Name = Name;
        }
    }
}
