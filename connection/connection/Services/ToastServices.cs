using connection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;

namespace connection.Services
{
    class ToastServices
    {
        public static XmlDocument CreateToast(LinkMan listItem)
        {
            XDocument xDoc = new XDocument(
                new XElement("toast", new XAttribute("launch", "app-defined-string"),
                    new XElement("visual",
                        new XElement("binding", new XAttribute("template", "ToastGeneric"),
                            new XElement("text", "今天是" + listItem.name + "的生日哦"),
                            new XElement("text", listItem.birthday.Month.ToString() + "月" + listItem.birthday.Day.ToString() + "日"),
                            new XElement("image", new XAttribute("placement", "appLogoOverride"), new XAttribute("src", "Assets/Square44x44Logo.scale-200.png")),
                            new XElement("image", new XAttribute("placement", "inline"), new XAttribute("src", "Assets/birthday.jpg"))
                        )
                    ),
                    new XElement("audio", new XAttribute("src", "ms-appx:///Assets/" + listItem.music.Substring(listItem.music.LastIndexOf('/') + 1)))
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }
    }
}
