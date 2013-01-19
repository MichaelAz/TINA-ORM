using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace TinaORM.Core
{
    public class JavascriptSerializer : ISerializer
    {
        private readonly System.Web.Script.Serialization.JavaScriptSerializer serializer = new JavaScriptSerializer();


        public string Serialize(object o)
        {
            return serializer.Serialize(o);
        }

        public T Deserialize<T>(string s)
        {
            return serializer.Deserialize<T>(s);
        }
    }
}
