using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace AppactsPlugin.External.Xml
{
    /// <summary>
    /// 
    /// </summary>
    internal static class Serialization
    {
        /// <summary>
        /// Deserializes the specified XML.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        public static TClass Deserialize<TClass>(string xml) where TClass : class, new()
        {
            TClass tClass = new TClass();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
            using (TextReader textReader = new StringReader(xml))
            {
                tClass = (TClass)xmlSerializer.Deserialize(textReader);
            }
            return tClass;
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static TClass Deserialize<TClass>(Stream stream) where TClass : class, new()
        {
            TClass tClass = new TClass();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                tClass = (TClass)xmlSerializer.Deserialize(stream);
            }
            finally
            {
                stream.Dispose();
            }
            return tClass;
        }


        /// <summary>
        /// Deserializes the specified file path.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static TClass Deserialize<TClass>(string filePath, Encoding encoding) where TClass : class, new()
        {
            TClass tClass = new TClass();

            using (TextReader textReader = new StreamReader(filePath, encoding))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                tClass = (TClass)xmlSerializer.Deserialize(textReader);
            }

            return tClass;
        }


        /// <summary>
        /// Serializes the specified t class.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="tClass">The t class.</param>
        /// <returns></returns>
        public static string Serialize<TClass>(TClass tClass) where TClass : class, new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            xmlSerializer.Serialize(xmlWriter, tClass);
            return stringBuilder.ToString();
        }


        /// <summary>
        /// Serializes the specified t class.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <param name="tClass">The t class.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void Serialize<TClass>(TClass tClass, string filePath, Encoding encoding)
            where TClass : class, new()
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath, false, encoding))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(TClass));
                StringBuilder stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
                xmlSerializer.Serialize(xmlWriter, tClass);
                streamWriter.Write(stringBuilder.ToString());
            }
        }
    }
}
