using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;

namespace PmlVersionControl
{
    public partial class Utility
    {
        /// <summary>
        /// This method will create xml to specified directory
        /// Read the xml if already there to get paths
        /// If xml is newly created then promps to get path values
        /// if all path values are not found then also prompts to get that
        /// </summary>
        public static void createAndUpdateXmlForPathSettings(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();


            if (File.Exists(xmlPath))
            {
                doc.Load(xmlPath);
                XmlNode rootNode = doc.FirstChild.NextSibling;

                XmlNodeList pushDirectory = doc.GetElementsByTagName("pushDirectory");
                foreach (XmlNode nd in pushDirectory)
                {
                    CommitWindow.pushDirectory = nd.InnerText;
                }

                if (CommitWindow.pushDirectory == "")
                {
                    PathSettings pushPath = new PathSettings();
                    pushPath.Text = "Set Commit Directory";
                    pushPath.lblSettings.Text = "Set Commit Directory";
                    pushPath.ShowDialog();
                    if (!(CommitWindow.pushDirectory == ""))
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "pushDirectory", CommitWindow.pushDirectory);
                    }
                }

                XmlNodeList rootDirectory = doc.GetElementsByTagName("rootDirectory");
                foreach (XmlNode nd in rootDirectory)
                {
                    CommitWindow.rootDirectory = nd.InnerText;
                }

                if (CommitWindow.rootDirectory == "")
                {
                    PathSettings rootPath = new PathSettings();
                    rootPath.Text = "Set Root Directory";
                    rootPath.lblSettings.Text = "Set Root Directory";
                    rootPath.ShowDialog();
                    if (!(CommitWindow.rootDirectory == ""))
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "rootDirectory", CommitWindow.rootDirectory);
                    }


                }



                XmlNodeList codeDirectory = doc.GetElementsByTagName("codeDirectory");
                foreach (XmlNode nd in codeDirectory)
                {
                    CommitWindow.codeDirectory = nd.InnerText;
                }


                if (CommitWindow.codeDirectory == "")
                {
                    PathSettings codePath = new PathSettings();
                    codePath.Text = "Set Code Directory";
                    codePath.lblSettings.Text = "Set Code Directory";
                    codePath.ShowDialog();

                    if (!(CommitWindow.codeDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "codeDirectory", CommitWindow.codeDirectory);
                    }


                }

                XmlNodeList finalDirectoryForEncryption = doc.GetElementsByTagName("finalDirectoryForEncryption");
                foreach (XmlNode nd in finalDirectoryForEncryption)
                {
                    CommitWindow.finalDirectoryForEncryption = nd.InnerText;
                }

                if (CommitWindow.finalDirectoryForEncryption == "")
                {
                    PathSettings Encryption = new PathSettings();
                    Encryption.Text = "Set Encryption Directory";
                    Encryption.lblSettings.Text = "Set Encryption Directory";
                    Encryption.ShowDialog();

                    if (!(CommitWindow.finalDirectoryForEncryption == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "finalDirectoryForEncryption", CommitWindow.finalDirectoryForEncryption);
                    }

                }

                XmlNodeList emergencyDirectory = doc.GetElementsByTagName("emergencyDirectory");
                foreach (XmlNode nd in emergencyDirectory)
                {
                    CommitWindow.emergencyDirectory = nd.InnerText;
                }

                if (CommitWindow.emergencyDirectory == "")
                {
                    PathSettings Emergency = new PathSettings();
                    Emergency.Text = "Set Emergency Directory";
                    Emergency.lblSettings.Text = "Set Emergency Directory";
                    Emergency.ShowDialog();

                    if (!(CommitWindow.emergencyDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "emergencyDirectory", CommitWindow.emergencyDirectory);
                    }

                }


            }
            else
            {
                //(1) the xml declaration is recommended, but not mandatory
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                XmlElement rootNode = doc.CreateElement(string.Empty, "root", string.Empty);
                doc.AppendChild(rootNode);
                //doc.Save(xmlPath);

                if (CommitWindow.pushDirectory == "")
                {
                    PathSettings pushPath = new PathSettings();
                    pushPath.Text = "Set Commit Directory";
                    pushPath.lblSettings.Text = "Set Commit Directory";
                    pushPath.ShowDialog();
                    if (!(CommitWindow.pushDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(rootNode, doc, xmlPath, "pushDirectory", CommitWindow.pushDirectory);
                    }


                }

                if (CommitWindow.rootDirectory == "")
                {
                    PathSettings rootPath = new PathSettings();
                    rootPath.Text = "Set Root Directory";
                    rootPath.lblSettings.Text = "Set Root Directory";
                    rootPath.ShowDialog();
                    if (!(CommitWindow.rootDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(rootNode, doc, xmlPath, "rootDirectory", CommitWindow.rootDirectory);

                    }


                }

                if (CommitWindow.codeDirectory == "")
                {
                    PathSettings codePath = new PathSettings();
                    codePath.Text = "Set Code Directory";
                    codePath.lblSettings.Text = "Set Code Directory";
                    codePath.ShowDialog();

                    if (!(CommitWindow.codeDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(rootNode, doc, xmlPath, "codeDirectory", CommitWindow.codeDirectory);

                    }


                }

                if (CommitWindow.finalDirectoryForEncryption == "")
                {
                    PathSettings Encryption = new PathSettings();
                    Encryption.Text = "Set Encryption Directory";
                    Encryption.lblSettings.Text = "Set Encryption Directory";
                    Encryption.ShowDialog();

                    if (!(CommitWindow.finalDirectoryForEncryption == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(rootNode, doc, xmlPath, "finalDirectoryForEncryption", CommitWindow.finalDirectoryForEncryption);

                    }


                }
                //The pupose of this emergency directory is to commit on emergency directory to get the function immidiately
                if (CommitWindow.emergencyDirectory == "")
                {
                    PathSettings Emergency = new PathSettings();
                    Emergency.Text = "Set Emergency Directory";
                    Emergency.lblSettings.Text = "Set Emergency Directory";
                    Emergency.ShowDialog();

                    if (!(CommitWindow.emergencyDirectory == "")) //user may close the enter directory form..and then it will return empty directory
                    {
                        writeTagXML(doc.DocumentElement, doc, xmlPath, "emergencyDirectory", CommitWindow.emergencyDirectory);
                    }

                }
            }

        }

        public static void writeTagXML(XmlElement elem, XmlDocument doc, string savePath, string TagName, string TagValue)
        {
            //Create xml document for writing
            if (!File.Exists(savePath))
            {

                XmlElement newNode = doc.CreateElement(string.Empty, TagName, string.Empty);
                XmlText text1 = doc.CreateTextNode(TagValue);
                newNode.AppendChild(text1);
                //newNode.Value = TagValue;
                elem.AppendChild(newNode);
                //newNode.Value = TagValue;
                try
                {
                    doc.Save(savePath);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            else
            {
                XmlElement newNode = doc.CreateElement(string.Empty, TagName, string.Empty);
                XmlText text1 = doc.CreateTextNode(TagValue);
                newNode.AppendChild(text1);
                //newNode.Value = TagValue;
                elem.AppendChild(newNode);
                //newNode.Value = TagValue;
                try
                {
                    doc.Save(savePath);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
