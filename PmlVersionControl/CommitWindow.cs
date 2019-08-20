using System;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using System.IO;
using System.Collections.Generic;

namespace PmlVersionControl
{
    public partial class CommitWindow : Form
    {
        public string pushDirectory = @"E:\Developments\Notepad++ Plugin\Macro_Vault\MACRO_ARCHIVE";
        public string rootDirectory = @"E:\Developments\Notepad++ Plugin\Macro_Vault";
        public string codeDirectory = @"E:\Developments\Notepad++ Plugin\Macro_Vault\MACRO_LEAP_E3D";
        public string finalDirectoryForEncryption = @"E:\Developments\Notepad++ Plugin\Macro_Vault\MACRO_LEAP_E3D_ENCRY";
        public CommitWindow()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            //Take the commit message
            string commitMessage = richTxtCommit.Text;
            if (!(commitMessage == ""))
            {
                NotepadPPGateway notepadPPGateway = new NotepadPPGateway();

                //Save the file to original location
                string path = notepadPPGateway.GetCurrentFilePath();
                notepadPPGateway.SaveCurrentFile();

                //Save the file to another directory
                string currentFileName = Path.GetFileName(path);
                string userName = Environment.UserName;
                DateTime now = System.DateTime.Now;
                string pushedFileName = now.ToString() + "_" + userName + "_" + currentFileName;
                pushedFileName = pushedFileName.Replace(":", "_");
                //Create directory for the original file
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
                        File.Copy(path, newDirectory+currentFileName);

                    }
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

        static void AddCommitMessageToFileTop(string commitMessage, string filePathToModify)
        {
            commitMessage = "--"+DateTime.Now+Environment.UserName +"!!! commit message:"+ commitMessage;

            // Read a text file line by line.  
            string[] lines = File.ReadAllLines(filePathToModify);
            List<string> linesList = new List<string>();
            linesList.Add(commitMessage);
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
