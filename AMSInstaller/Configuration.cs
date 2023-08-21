using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace AMSInstaller
{
    class Configuration
    {
        public static string iisHost="";
        public static string iisFrameworkVersion="";
        string path = "";

        public void Init()
        {     
            try
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"Configuration.xml";
            }
            catch (Exception) { }

            if (!File.Exists(path))
            {
                SaveConfiguration(path);
            }
            LoadConfiguration(path);           
        }

        public static void LoadConfiguration(string archivoConfiguracion)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(archivoConfiguracion);

                foreach (XmlElement elem in xDoc.SelectNodes(@"/ConfigParameters/ConfigParameter"))
                {
                    string parameterID = elem.Attributes["id"].Value;

                    switch (parameterID)
                    {
                        case "IISHost":
                            iisHost = elem.Attributes["value"].Value;
                            break;
                        case "IISFrameworkVersion":
                            iisFrameworkVersion = elem.Attributes["value"].Value;
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Log.GetInstance().DoLog("Error obtaining the configuration from xml file ","General",ex);
            }
        }

        public static void SaveConfiguration(string archivoConfiguracion)
        {
            XmlDocument xdoc = new XmlDocument();
            XmlElement elementRoot = xdoc.CreateElement("ConfigParameters");
            xdoc.AppendChild(elementRoot);

            //------------------------IISHost
            XmlElement elementIISHost = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributeIISHost = xdoc.CreateAttribute("id");
            attributeIISHost.Value = "DataSource";
            elementIISHost.Attributes.Append(attributeIISHost);
            XmlAttribute valueAttributeIISHost = xdoc.CreateAttribute("value");
            valueAttributeIISHost.Value = elementIISHost.ToString();
            elementIISHost.Attributes.Append(valueAttributeIISHost);
            elementRoot.AppendChild(elementIISHost);

            //------------------------IISsFrameworkVersion
            XmlElement elementIISFramework = xdoc.CreateElement("ConfigParameter");
            XmlAttribute attributeIISFramework = xdoc.CreateAttribute("id");
            attributeIISFramework.Value = "DataSource";
            elementIISFramework.Attributes.Append(attributeIISFramework);
            XmlAttribute valueIISFramework = xdoc.CreateAttribute("value");
            valueIISFramework.Value = elementIISHost.ToString();
            elementIISFramework.Attributes.Append(valueIISFramework);
            elementRoot.AppendChild(elementIISFramework);


            xdoc.Save(archivoConfiguracion);
        }
    }
}
