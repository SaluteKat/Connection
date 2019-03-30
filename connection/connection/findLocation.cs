using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace connection
{
    class findLocation
    {
        public async static Task<string> GetLoc(string phone)
        {
            var http = new HttpClient();
            var res = await http.GetAsync("http://api.k780.com/?app=phone.get&phone="+phone+"&appkey=33173&sign=04957c37d1a768804a833a4b59f20c0b&format=xml");
            var result = await res.Content.ReadAsStringAsync();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);

            var data = doc.DocumentElement;
            var str0 = data.GetElementsByTagName("status");
            var te0 = str0[0].ChildNodes[0].Value;
            if (te0 == "NOT_ATT")
            {
                return null;
            }
            var str = data.GetElementsByTagName("style_simcall");
            var te = str[0].ChildNodes[0].Value;

            return te;
        }

    }

}
