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
        //public static string installationPath;// = @"C:\Users\Public\Documents\PmlVersionControlPlugin";
        public static string currentFileDirectory="";
        public static string emergencyDirectory = "";
        //public static string installationPath = @"X:\PDMSUSER\sduranama\My Document";
        public CommitWindow()
        {
            InitializeComponent();
            this.CenterToParent();
            //Adding tooltip on option button
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.chkFinal, "Available to production tommorrow");
            toolTip1.SetToolTip(this.chkEmergency, "Available to production now as emergency");
            toolTip1.SetToolTip(this.btnCommit, "Click this button to archive this document's copy with commit message");
            toolTip1.SetToolTip(this.richTxtCommit, "Write your changes or new implementation details");
            toolTip1.SetToolTip(this.label1, "Write your changes or new implementation details");

            

            try
            {
                               
                int sucessfull= Utility.initializeDirectoryFromConfigXml();

                if(sucessfull==5) //All paths set sucessfully
                {
                    Utility.setPathsToCommitWindow();
                }else
                {
                    Close();
                    return;
                }
                //createAndUpdateXmlForPathSettings(xmlPath);
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

       
            
        private void btnCommit_Click(object sender, EventArgs e)
        {
            //Check its pml file or not
            NotepadPPGateway notepadPPGatewayTemp = new NotepadPPGateway();
            string pathTemp = notepadPPGatewayTemp.GetCurrentFilePath();
            string currentFileNameTemp = Path.GetFileName(pathTemp);
            bool isPml = Utility.isPml(currentFileNameTemp); //True if this is a PML file

            


            //Take the commit message
            string[] commitMessage = richTxtCommit.Lines;
            int i = 0;

            //Getting commit message
            foreach(string s in commitMessage)
            {
                commitMessage[i] =Utility.commentSpecifier+"Commit Message:$ "+ commitMessage[i];

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

                 //Only pml files can be commited as final
                if((chkFinal.Checked==true || chkEmergency.Checked==true) && isPml==false)
                {
                        MessageBox.Show("Only PML file can be committed as Final");
                        return;
                }


                if(chkFinal.Checked==true)
                {
                    //Check file path contains code directory of PML files
                    if(path.Contains(codeDirectory))
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
                    else
                        {
                            MessageBox.Show("File should be opened from code directory to make as final");
                        }

                }
                else if(chkEmergency.Checked==true)
                {
                        //Check file path contains code directory of PML files
                        if (path.Contains(codeDirectory))
                        {
                            //To copy on final directory for encryption
                            //Same name on same if else conflicts so 'New' added 
                            string strucDirectoryNew = (path.Replace(codeDirectory, "")).Replace(currentFileName, "");
                            string newDirectoryNew = finalDirectoryForEncryption + strucDirectoryNew;
                            if(!Directory.Exists(newDirectoryNew))
                            {
                                Directory.CreateDirectory(newDirectoryNew);
                            }
                            
                            if (File.Exists(newDirectoryNew + currentFileName))
                            {
                                File.Delete(newDirectoryNew + currentFileName);
                                File.Copy(path, newDirectoryNew + currentFileName);
                            }
                            else
                            {
                                File.Copy(path, newDirectoryNew + currentFileName);
                            }


                            //To copy on emergency directory to get immediately 
                            
                            newDirectoryNew = emergencyDirectory + strucDirectoryNew;

                            if (!Directory.Exists(newDirectoryNew))
                            {
                                Directory.CreateDirectory(newDirectoryNew);
                            }
                            if (File.Exists(newDirectoryNew + currentFileName))
                            {
                                File.Delete(newDirectoryNew + currentFileName);
                                File.Copy(path, newDirectoryNew + currentFileName);
                            }
                            else
                            {
                                File.Copy(path, newDirectoryNew + currentFileName);
                            }

                            Utility.pmlindexAppend(emergencyDirectory + "\\" + "pml.index", newDirectoryNew + currentFileName);

                        }
                        else
                        {
                            MessageBox.Show("File should be opened from code directory to make as final");
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
