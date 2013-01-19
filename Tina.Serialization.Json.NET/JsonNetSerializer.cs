using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TinaORM.Core;
using Newtonsoft.Json;

namespace TinaORM.Serialization.Json.Net
{
    public class ServiceStackTextSerializer : ISerializer
    {
        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public T Deserialize<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
