using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TinaORM.Core;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace TinaORM.Serialization.BSON
{
    public class BsonSerializer : ISerializer
    {
        private JsonSerializer serializer;

        public BsonSerializer()
        {
            serializer = new JsonSerializer();
        }

        public string Serialize(object o)
        {
            var IOStream = new MemoryStream();
            var writer = new BsonWriter(IOStream);
            serializer.Serialize(writer, o);

            var s = Convert.ToBase64String(IOStream.ToArray());

            return Convert.ToBase64String(IOStream.ToArray());
        }

        public T Deserialize<T>(string s)
        {
            var IOStream = new MemoryStream(Convert.FromBase64String(s));
            var reader = new BsonReader(IOStream);
            return serializer.Deserialize<T>(reader);
        }
    }
}
