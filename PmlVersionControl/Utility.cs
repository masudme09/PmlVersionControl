using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PmlVersionControl
{
    public static partial class Utility
    {
        public static string commitPath { get; set; }
        public static string codePath { get; set; }
        public static string  regularFinalPath { get; set; }
        public static string rootPath { get; set; }
        public static string emergencyFinalPath { get; set; }
        public static string  configXMLpath { get; set; } //Set configXML path hardcoded
        public static string langConfigXMLPath { get; set; }

        public static string commentSpecifier = "--";


        /// <summary>
        /// This method will initialize directories from configXML
        /// Need to call it when menu is initialize
        /// </summary>
        public static int initializeDirectoryFromConfigXml()
        {
            if (Directory.Exists(@"X:\PDMSUSER\sduranama\My Document"))
            {
                configXMLpath = @"X:\AVEVA\Manage\PMLVersionManagement" + "\\configXml.xml";
                langConfigXMLPath = @"X:\AVEVA\Manage\PMLVersionManagement" + "\\langSetupXml.xml";
            }
            else
            {
                configXMLpath = @"C:\Users\Public\Documents\PmlVersionControlPlugin" + "\\ConfigXml\\configXml.xml";
                langConfigXMLPath = @"C:\Users\Public\Documents\PmlVersionControlPlugin" + "\\ConfigXml\\langSetupXml.xml";
            }


            int successfull = 0;
            XmlDocument doc = new XmlDocument();
            if (File.Exists(configXMLpath))
            {
                doc.Load(configXMLpath);
                XmlNode rootNode = doc.FirstChild.NextSibling;

                //Getting commit directory from configXml
                XmlNodeList pushDirectory = doc.GetElementsByTagName("pushDirectory");
                foreach (XmlNode nd in pushDirectory)
                {
                    if(Directory.Exists(nd.InnerText))
                    {
                        commitPath = nd.InnerText;
                    }else
                    {
                        commitPath = "";
                    }
                    
                }
                if(commitPath=="")
                {
                    MessageBox.Show("Commit directory not found or specified directory not exists");
                }else
                {
                    successfull++;
                }

                //Getting root directory from configXML
                XmlNodeList rootDirectory = doc.GetElementsByTagName("rootDirectory");
                foreach (XmlNode nd in rootDirectory)
                {
                    if(Directory.Exists(nd.InnerText))
                    {
                        rootPath = nd.InnerText;
                    }
                    else
                    {
                        rootPath = "";
                    }
                }

                if(rootPath=="")
                {
                    MessageBox.Show("Root directory not found or specified directory not exists");
                }
                else
                {
                    successfull++;
                }

                //Getting code directory from configXML
                XmlNodeList codeDirectory = doc.GetElementsByTagName("codeDirectory");
                foreach (XmlNode nd in codeDirectory)
                {
                    if (Directory.Exists(nd.InnerText))
                    {
                        codePath = nd.InnerText;
                    }
                    else
                    {
                        codePath = "";
                    }
                }

                if (codePath == "")
                {
                    MessageBox.Show("Code directory not found or specified directory not exists");
                }
                else
                {
                    successfull++;
                }


                //Getting regular final directory from configXML
                XmlNodeList finalDirectoryForEncryption = doc.GetElementsByTagName("finalDirectoryForEncryption");
                foreach (XmlNode nd in finalDirectoryForEncryption)
                {
                    if (Directory.Exists(nd.InnerText))
                    {
                        regularFinalPath = nd.InnerText;
                    }
                    else
                    {
                        regularFinalPath = "";
                    }
                }

                if (regularFinalPath == "")
                {
                    MessageBox.Show("Final directory for encryption not found or specified directory not exists");
                }
                else
                {
                    successfull++;
                }


                //Getting emergency directory form configXML
                XmlNodeList emergencyDirectory = doc.GetElementsByTagName("emergencyDirectory");
                foreach (XmlNode nd in emergencyDirectory)
                {
                    if (Directory.Exists(nd.InnerText))
                    {
                        emergencyFinalPath = nd.InnerText;
                    }
                    else
                    {
                        emergencyFinalPath = "";
                    }
                }

                if (emergencyFinalPath == "")
                {
                    MessageBox.Show("Final directory for encryption not found or specified directory not exists");
                }else
                {
                    successfull++;
                }

                return successfull;

            }
            else
            {
                MessageBox.Show("No configXML found on directory: " + configXMLpath);
                return successfull;
            }

        }

        /// <summary>
        /// This method sets required path to commit window
        /// </summary>
        public static void setPathsToCommitWindow()
        {
            CommitWindow.pushDirectory=commitPath;
            CommitWindow.codeDirectory= codePath;
            CommitWindow.finalDirectoryForEncryption=regularFinalPath;
            CommitWindow.rootDirectory=rootPath;
            CommitWindow.emergencyDirectory=emergencyFinalPath;
                       
            
        }

        /// <summary>
        /// This method takes file name as input and check that is PMl or not
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>

        public static bool isPml(string fileExtension)
        {
            bool isPml = false;

            if(fileExtension.Contains(".pml") || fileExtension.Contains(".pmlfrm") || fileExtension.Contains(".pmlfnc") || fileExtension.Contains(".MAC"))
            {
                isPml = true;
            }


            return isPml;
        }

        /// <summary>
        /// This method appends to pml index file
        /// </summary>
        /// <param name="filePath"></param>

        public static void pmlindexAppend(string indexPath, string filePathToAppend)
        {
            if(File.Exists(filePathToAppend))
            {
                if(File.Exists(indexPath))
                {
                    string[] allContents = File.ReadAllLines(indexPath);
                    List<string> contents = new List<string>();
                    //contents = allContents.ToList();

                    
                    int length = allContents.Length;
                    allContents[allContents.Length - 1] = "";
                    allContents[length - 1] = filePathToAppend.Replace(Path.GetDirectoryName(indexPath), "");
                    allContents[length - 1]= allContents[length - 1].Replace("\\","/");
                    //allContents[length] = "/";

                    foreach(string line in allContents)
                    {
                        contents.Add(line);
                    }
                    contents.Add("/");

                    File.Delete(indexPath);
                    File.WriteAllLines(indexPath, contents);

                }
                else
                {
                    var file = File.Create(indexPath);
                    file.Dispose();
                    List<string> contents = new List<string>();
                    //contents = allContents.ToList();
                   
                    string addString = filePathToAppend.Replace(Path.GetDirectoryName(indexPath), "");
                    addString = addString.Replace("\\", "/");
                    contents.Add(addString);

                    contents.Add("/");
                    File.WriteAllLines(indexPath, contents);
                }
            }

        }
        


    }
}
