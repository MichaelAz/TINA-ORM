using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinaORM.Core;
using ServiceStack.Text;

namespace TinaORM.Serialization.ServiceStack.Text
{
    public class ServiceStackTextSerializer : ISerializer
    {
        public string Serialize(object o)
        {
            return JsonSerializer.SerializeToString(o);
        }

        public T Deserialize<T>(string s)
        {
            return JsonSerializer.DeserializeFromString<T>(s);
        }
    }
}
