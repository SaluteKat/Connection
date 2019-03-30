using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Xml.Linq;
using connection.Models;

namespace connection.Services
{
    public class TileService
    {

        public static XmlDocument CreateTiles(LinkMan listItem)
        {
            XDocument xDoc = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Happy Birthday!"), new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("src", "Assets/color.jpg"), new XAttribute("placement", "background")),
                            //new XElement("image", new XAttribute("source",listItem.img), new XAttribute("placement","background")),
                            //new XElement("text","test"),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", listItem.name, new XAttribute("hint-style", "caption")),
                                    new XElement("text", listItem.tel, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                        // Medium Tile
                        new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Happy Birthday!"), new XAttribute("template", "TileMedium"),
                            new XElement("image", new XAttribute("src", "Assets/color.jpg"), new XAttribute("placement", "background")),
                            new XElement("group",
                                new XElement("subgroup",
                                    new XElement("text", listItem.name, new XAttribute("hint-style", "caption")),
                                    new XElement("text", listItem.tel, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                                )
                            )
                        ),

                       // Wide Tile
                       new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Happy Birthday!"), new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("src", "Assets/color.jpg"), new XAttribute("placement", "background")),
                           new XElement("group",
                               new XElement("subgroup",
                                   new XElement("text", listItem.name, new XAttribute("hint-style", "caption")),
                                   new XElement("text", listItem.tel, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 3))
                               )
                           )
                       ),

                       //Large Tile
                       new XElement("binding", new XAttribute("branding", "name"), new XAttribute("displayName", "Happy Birthday!"), new XAttribute("template", "TileLarge"),
                           new XElement("image", new XAttribute("src", "Assets/color.jpg"), new XAttribute("placement", "background")),
                           //new XElement("text",listItem.img.UriSource.ToString().Substring(11), new XAttribute("hint-wrap","true")),  
                           new XElement("group",
                               new XElement("subgroup",
                                   new XElement("text", listItem.name, new XAttribute("hint-style", "caption")),
                                   new XElement("text", listItem.tel, new XAttribute("hint-style", "captionsubtle"), new XAttribute("hint-wrap", true), new XAttribute("hint-maxLines", 5))
                                   )
                           )
                       )
                    )
                )
            );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xDoc.ToString());
            return xmlDoc;
        }
    }


}
