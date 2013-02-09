using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TinaORM.Core;

using YamlDotNet.RepresentationModel.Serialization;

namespace TinaORM.Serialization.YAML
{
    public class YamlSerializer : ISerializer
    {
        private Serializer serializer;

        public YamlSerializer()
        {
            serializer = new Serializer();
        }

        public string Serialize(object o)
        {
            var writer = new StreamWriter(new MemoryStream());

            serializer.Serialize(writer, o);

            var buffer = ((MemoryStream) writer.BaseStream).ToArray();
            return Encoding.Unicode.GetString(buffer);
        }

        public T Deserialize<T>(string s)
        {
            var deser = new YamlDotNet.RepresentationModel.Serialization.YamlSerializer(typeof (T));
            return (T) deser.Deserialize(new StreamReader(new MemoryStream(Encoding.Unicode.GetBytes(s))));
        }
    }
}
