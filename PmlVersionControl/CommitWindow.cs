using System;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace PmlVersionControl
{
    public partial class CommitWindow : Form
    {
        public static string pushDirectory ="";
        public static string rootDirectory ="";
        public static string codeDirectory = "";
        public static string finalDirectoryForEncryption = "";
        //public static string installationPath = @"C:\Users\Public\Documents\PmlVersionControlPlugin";
        public static string currentFileDirectory="";
        public static string installationPath = @"X:\PDMSUSER\sduranama\My Document";
        public CommitWindow()
        {
            InitializeComponent();
            this.CenterToParent();

            try
            {
                //getting installation directory to save xml
                //string installationPath = AppDomain.CurrentDomain.BaseDirectory;
                //string installationPath = @"C:\Program Files\Notepad++\plugins\PmlVersionControl";

                
                Directory.CreateDirectory(installationPath + "\\ConfigXml");
                string xmlPath = installationPath + "\\ConfigXml\\configXml.xml";
                createAndUpdateXmlForPathSettings(xmlPath);
            }
            catch(Exception errr)
            {
                MessageBox.Show(errr.ToString());
            }
            
            //if pushdirectory is not null then loop through each file
            //Delete if any filename contains temp
            if(!(pushDirectory==""))
            {
                string[] files = Directory.GetFiles(pushDirectory);
                foreach(string file in files)
                {
                    if(file.Contains("temp"))
                    {
                        File.Delete(file);
                    }
                }

            }

        }

        /// <summary>
        /// This method will create xml to installation directory
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
                foreach(XmlNode nd in pushDirectory)
                {
                    CommitWindow.pushDirectory = nd.InnerText;
                }

                if(CommitWindow.pushDirectory=="")
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

            }

        }
               
        public static void writeTagXML(XmlElement elem, XmlDocument doc, string savePath, string TagName, string TagValue )
        {
            //Create xml document for writing
            if (!File.Exists(savePath))
            {
                
                XmlElement newNode =  doc.CreateElement(string.Empty, TagName, string.Empty);
                XmlText text1 = doc.CreateTextNode(TagValue);
                newNode.AppendChild(text1);
                //newNode.Value = TagValue;
                elem.AppendChild(newNode);
                //newNode.Value = TagValue;
                try
                {
                    doc.Save(savePath);
                
                }
                catch(Exception e)
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
            
        private void btnCommit_Click(object sender, EventArgs e)
        {          
            //Take the commit message
            string[] commitMessage = richTxtCommit.Lines;
            int i = 0;
            foreach(string s in commitMessage)
            {
                commitMessage[i] ="--Commit Message:$ "+ commitMessage[i];

                i++;
            }

            if (!(commitMessage.Length == 0))
            {
                NotepadPPGateway notepadPPGateway = new NotepadPPGateway();

                //Save the file to original location
                string path = notepadPPGateway.GetCurrentFilePath();

                //check the file is committed file or not.
                if(path.Contains("temp"))
                {
                    MessageBox.Show("This file can not be committed. Revert to this version before committing.");
                    return;
                }

                currentFileDirectory = path;//for later use
                notepadPPGateway.SaveCurrentFile();

                //Save the file to another directory
                string currentFileName = Path.GetFileName(path);
                string userName = Environment.UserName;
                DateTime now = System.DateTime.Now;
                string pushedFileName = now.ToString() + "_" + userName + "_" + currentFileName;
                pushedFileName = pushedFileName.Replace(":", "_");
                pushedFileName = pushedFileName.Replace(@"\", "_");
                //Create directory for the original file
                if (Directory.Exists(pushDirectory) && !(pushDirectory=="") && Directory.Exists(rootDirectory) && !(rootDirectory=="")
                    && Directory.Exists(codeDirectory) && !(codeDirectory == "") && Directory.Exists(finalDirectoryForEncryption) && !(finalDirectoryForEncryption == ""))
                {
                    Directory.CreateDirectory(pushDirectory + "\\" + currentFileName);
                    string pushedFileDirectory = pushDirectory + "\\" + currentFileName + "\\" + pushedFileName;

                    notepadPPGateway.SaveCurrentFile(pushedFileDirectory);

                    //Open the file from remote directory and write commit message on top of it as comment and save

                    AddCommitMessageToFileTop(commitMessage, pushedFileDirectory);

                    MessageBox.Show("Successfully Committed");

                    //Closing the save as file and opening the original file
                    notepadPPGateway.CloseCurrentFile();
                    notepadPPGateway.openFile(path);

               

                if(chkFinal.Checked==true)
                {
                    //Check file path contains root directory of PML files
                    if(path.Contains(rootDirectory))
                    {

                        string strucDirectory = (path.Replace(codeDirectory,"")).Replace(currentFileName,"");
                        string newDirectory = finalDirectoryForEncryption + strucDirectory;
                        Directory.CreateDirectory(newDirectory);
                        if(File.Exists(newDirectory + currentFileName))
                        {
                            File.Delete(newDirectory + currentFileName);
                            File.Copy(path, newDirectory + currentFileName);
                        }
                        else
                        {
                            File.Copy(path, newDirectory + currentFileName);
                        }
                     
                    }
                }

                }
                else
                {
                    MessageBox.Show("Invalid Directory");
                }


                //Close the commit form and dispose
                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Write a Commit Message before commiting code describing changes");
            }
        }

        static void AddCommitMessageToFileTop(string[] commitMessage, string filePathToModify)
        {
            //commitMessage = "--"+DateTime.Now+Environment.UserName +"!!! commit message:"+ commitMessage;

            // Read a text file line by line.  
            string[] lines = File.ReadAllLines(filePathToModify);
            List<string> linesList = new List<string>();
            linesList.Add("--Committed by:$ " + Environment.UserName);
            linesList.Add("--Commit Time & Date:$ " + DateTime.Now);
            linesList.Add("--Commit Original Directory: " + currentFileDirectory);

            foreach(string ln in commitMessage)
            {
                linesList.Add(ln);
            }
            
            foreach (string ln in lines)
            {
                linesList.Add(ln);
            }

            //Now write the file
            if(File.Exists(filePathToModify))
            {
                File.Delete(filePathToModify);
            }

            File.WriteAllLines(filePathToModify,linesList.ToArray());
            
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
