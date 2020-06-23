﻿using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GOEddie.Dacpac.References
{
    public class HeaderParser
    {
        private readonly string _dacPacPath;

        public HeaderParser(string dacPacPath)
        {
            _dacPacPath = dacPacPath;
        }

        public List<CustomData> GetCustomData()
        {
            var dac = new DacHacXml(_dacPacPath);
            var xml = dac.GetXml("Model.xml");

            var reader = XmlReader.Create(new StringReader(xml));
            reader.MoveToContent();

            var data = new List<CustomData>();
            CustomData currentCustomData = null;

            while (reader.Read())
            {
                if (reader.Name == "CustomData" && reader.NodeType == XmlNodeType.Element)
                {
                    var cat = reader.GetAttribute("Category");
                    var type = reader.GetAttribute("Type");

                    currentCustomData = new CustomData(cat, type);
                    data.Add(currentCustomData);
                }

                if (reader.Name == "Metadata" && reader.NodeType == XmlNodeType.Element)
                {
                    var name = reader.GetAttribute("Name");
                    var value = reader.GetAttribute("Value");

                    currentCustomData.AddMetadata(name, value);
                }

                if (reader.Name == "Header" && reader.NodeType == XmlNodeType.EndElement)
                {
                    break; //gone too far
                }
            }
            dac.Close();

            return data;
        }
    }
}