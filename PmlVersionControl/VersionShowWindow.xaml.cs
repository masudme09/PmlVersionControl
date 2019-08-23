using Kbg.NppPluginNET.PluginInfrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PmlVersionControl
{
    /// <summary>
    /// Interaction logic for VersionShowWindow.xaml
    /// </summary>
    public partial class VersionShowWindow : Window
    {
        public VersionShowWindow()
        {
            InitializeComponent();

            try
            {
                NotepadPPGateway notepadPPGateway = new NotepadPPGateway();

                //Save the file to original location
                string path = notepadPPGateway.GetCurrentFilePath();

                //Save the file to another directory
                string currentFileName = System.IO.Path.GetFileName(path);

                List<PmlVersion> allVersionInfo = versionInfo(currentFileName); //All available version info is stored here

                //Now need to add them to UI
                int count = 1;
                foreach (PmlVersion version in allVersionInfo)
                {
                    if (count < 12)
                    {
                        //Finding and assigning filename
                        System.Windows.Controls.Label lblFileName = this.FindName("lblfileName_" + count) as System.Windows.Controls.Label;
                        lblFileName.Content = version.fileName;
                        lblFileName.Visibility = Visibility.Visible;

                        //Finding and assigning modifiedDate
                        System.Windows.Controls.Label lblModifiedDate = this.FindName("lblfileDate_" + count) as System.Windows.Controls.Label;
                        lblModifiedDate.Content = version.modifiedDate;
                        lblModifiedDate.Visibility = Visibility.Visible;

                        //Finding and assigning modifiedBy
                        System.Windows.Controls.Label lblModifiedBy = this.FindName("lblModifiedBy_" + count) as System.Windows.Controls.Label;
                        lblModifiedBy.Content = version.modifiedBy;
                        lblModifiedBy.Visibility = Visibility.Visible;

                        //Finding and assigning commitMessage
                        System.Windows.Controls.RichTextBox lblCommitMessage = this.FindName("lblCommitMessage_" + count) as System.Windows.Controls.RichTextBox;
                        lblCommitMessage.AppendText(version.commitMessage);
                        lblCommitMessage.Visibility = Visibility.Visible;

                    }
                    count++;
                }

                //Finding and assigning count
                System.Windows.Controls.Label lblCountNumber = this.FindName("countNumber") as System.Windows.Controls.Label;
                lblCountNumber.Content =(count - 1).ToString("00");
                lblCountNumber.Visibility = Visibility.Visible;

                //Search all labels and set horizontal allignement center for version info labels only
                try
                {
                    UIElementCollection uiFileInfoLabels = fileInfoLabels.Children;
                    foreach (System.Windows.Controls.Label lbl in uiFileInfoLabels)
                    {
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;

                    }
                }
                catch
                {

                }
            }catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

        }

        //Getting Available version info
        public List<PmlVersion> versionInfo(string pmlFileName)
        {
            string commitDirectory = CommitWindow.pushDirectory; //File searching directory
            List<PmlVersion> versions = new List<PmlVersion>(); //Holds all the versionInfo of current file

            if(Directory.Exists(commitDirectory+"\\"+pmlFileName))
            {
                string[] files = Directory.GetFiles(commitDirectory + "\\" + pmlFileName);

                foreach(string file in files)
                {
                    //Need to extract filename, modifiedDate, modifiedby, and commit message from each file
                    PmlVersion version = new PmlVersion();
                    version.fileName = pmlFileName;

                    string replacedString = file.Split('\\')[file.Split('\\').Length-1];

                    replacedString = replacedString.Replace("_" + pmlFileName, ""); //Taking the filename out
                    string[] flSplit= replacedString.Split('_');
                    version.modifiedBy = flSplit[flSplit.Length - 1]; //Getting the last element of the split
                    replacedString=replacedString.Replace("_" + version.modifiedBy, "");

                    replacedString = replacedString.Replace("_", ":");
                    version.modifiedDate = replacedString;

                    foreach(string msg in commitMessage(file))
                    {
                        version.commitMessage = version.commitMessage + msg;
                    }

                    versions.Add(version);

                }


            }
            else
            {
                //MessageBox.Show("No available versions");
                
            }

            return versions;
        }

        public List<string> commitMessage(string filePath)
        {
            List<string> commitMsg= new List<string>();
            string[] allLines = File.ReadAllLines(filePath);

            foreach(string line in allLines)
            {
                if(line.Contains("--Commit Message:$"))
                {
                    string cMsg = line.Replace("--Commit Message:$", "");
                    commitMsg.Add(cMsg);
                }
            }

            return commitMsg;
        }

        private void LblfileName_1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {                
                System.Windows.Controls.Label srcLabel = e.Source as System.Windows.Controls.Label; //to know which label call this handler
                string fileName = srcLabel.Content.ToString();
                string fileSearchPath = CommitWindow.pushDirectory;

                string[] folders = Directory.GetDirectories(fileSearchPath);

                foreach(string folder in folders)
                {
                    string folderName= folder.Split('\\')[folder.Split('\\').Length - 1];

                    if (folderName == fileName)
                    {
                        string[] files = Directory.GetFiles(folder);

                        foreach(string file in files)
                        {
                            string replacedString = file.Split('\\')[file.Split('\\').Length - 1];
                            string flname = replacedString.Split('_')[replacedString.Split('_').Length-1];

                            if(flname==fileName)
                            {
                                NotepadPPGateway notepadPPGateway = new NotepadPPGateway();
                                notepadPPGateway.openFile(file);
                                this.Close();
                                break;
                            }

                        }


                    }
                }



            }
            catch 
            {
                //Write error log here
            }
        }
    }
}
