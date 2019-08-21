using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PmlVersionControl
{
    public partial class PathSettings : Form
    {
        public PathSettings()
        {
            InitializeComponent();
        }

        public void Label1_Click(object sender, EventArgs e)
        {

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (!(textBox1.Text == ""))
            {
                bool directCheck = directoryValidator(textBox1.Text);
                if (directCheck ==true)
                {
                    if (lblSettings.Text.Contains("Commit"))
                    {
                        CommitWindow.pushDirectory = textBox1.Text;
                    }
                    else if (lblSettings.Text.Contains("Root"))
                    {
                        CommitWindow.rootDirectory = textBox1.Text;
                    }
                    else if (lblSettings.Text.Contains("Code"))
                    {
                        CommitWindow.codeDirectory = textBox1.Text;
                    }
                    else if (lblSettings.Text.Contains("Encryption"))
                    {
                        CommitWindow.finalDirectoryForEncryption = textBox1.Text;
                    }

                    Close();
                    Dispose();
                }else
                {
                    MessageBox.Show("Invalid Directory");
                }
            }
            else
            {
                MessageBox.Show("Set the directory!");
            }
        }

        private bool directoryValidator(string directory)
        {
            if(Directory.Exists(directory) && !(directory==""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
