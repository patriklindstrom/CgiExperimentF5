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

        /// <summary>
        /// Should implement the reading part of the deserialistion.
        /// </summary>
        /// <param name="reader"></param>
        /// <exception cref="NotImplementedException">It s not implemented. Get security issues when xmlserializer is used as cgi.</exception>
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }


        public void WriteXml(XmlWriter writer)
        {
            foreach (IAliveTest test in this)
            {
                writer.WriteStartElement("IAliveTest");
                writer.WriteAttributeString("AssemblyQualifiedName", test.GetType().AssemblyQualifiedName);
                var xmlSerializer = new XmlSerializer(test.GetType());
                xmlSerializer.Serialize(writer, test);
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}