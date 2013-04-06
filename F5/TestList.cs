using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using F5.Tests;

namespace F5
{
    public class TestList : List<IAliveTest>, IXmlSerializable
    {
  
        #region IXmlSerializable
        public XmlSchema GetSchema(){ return null; }
   

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("TestList");
            while (reader.IsStartElement("IAliveTest"))
            {
                Type type = Type.GetType(reader.GetAttribute("AssemblyQualifiedName"));
                XmlSerializer serial = new XmlSerializer(type);

                reader.ReadStartElement("IAliveTest");
                this.Add((IAliveTest)serial.Deserialize(reader));
                reader.ReadEndElement(); 
            }
            reader.ReadEndElement(); 
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (IAliveTest test in this)
            {
                writer.WriteStartElement("IAliveTest");
                writer.WriteAttributeString("AssemblyQualifiedName", test.GetType().AssemblyQualifiedName);
                XmlSerializer xmlSerializer = new XmlSerializer(test.GetType());
                xmlSerializer.Serialize(writer, test);
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}