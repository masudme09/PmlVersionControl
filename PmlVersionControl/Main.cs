using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using PmlVersionControl;

/// <summary>
/// This plugin is intended to developed for PML file version controlling
/// Concept of this development is..having commit and versions button
/// Commit button will save current works to a specified directory with committed message into it and time stamp
/// versions button provide a window to show all versions of this file previously committed
/// User can select any version to open
/// </summary>
namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "PmlVersionControl";
        static string iniFilePath = null;
        static bool someSetting = false;
        static frmMyDlg frmMyDlg = null;
        static int idMyDlg = -1;
        static Bitmap tbBmp = PmlVersionControl.Properties.Resources.versions_16x16;
        static Bitmap tbBmp_tbTab = PmlVersionControl.Properties.Resources.commit_16x16;
        static Bitmap revert_tbTab = PmlVersionControl.Properties.Resources.revertImage;
        static Icon tbIcon = null;

        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "Commit Code", pmlCommit, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Available Versions", availableVersions);
            PluginBase.SetCommand(2, "Revert to This", revertToOldVersion);
            idMyDlg = 1;
            //PluginBase.SetCommand(2, "Test", availableVersions); idMyDlg = 1;
        }

        private static void revertToOldVersion()
        {
            string codeDirectory = CommitWindow.codeDirectory;

            NotepadPPGateway notepadPPGateway = new NotepadPPGateway();

            //Save the file to original location
            string path = notepadPPGateway.GetCurrentFilePath();
            string savePath = path.Split('\\')[path.Split('\\').Length-1];//file name
            savePath = savePath.Replace("temp_", "");
            if(!(CommitWindow.currentFileDirectory=="") && Path.GetFileName(CommitWindow.currentFileDirectory)==savePath)
            {
                savePath = CommitWindow.currentFileDirectory;
            }
            else
            {
                //read the file and for original path to save
                string[] lines = File.ReadAllLines(path);
                List<string> linesList = new List<string>();

                foreach(string line in lines)
                {
                    if(line.Contains("--Commit Original Directory: "))
                    {
                        savePath = line.Replace("--Commit Original Directory: ", "");
                        break;
                    }
                }

            }

            if(!File.Exists(savePath))
            {
                MessageBox.Show("This file could not be reverted");
            }
            else
            {
                notepadPPGateway.SaveCurrentFile(savePath);
                MessageBox.Show("Reverted successfully!");
            }


        }

        private static void availableVersions()
        {
            try
            {
                string installationPath =CommitWindow.installationPath;
                Directory.CreateDirectory(installationPath + "\\ConfigXml");
                string xmlPath = installationPath + "\\ConfigXml\\configXml.xml";
                CommitWindow.createAndUpdateXmlForPathSettings(xmlPath);
           
            VersionShowWindow versionWpf = new VersionShowWindow();
            versionWpf.Show();
            }
            catch (Exception errr)
            {
                MessageBox.Show(errr.ToString());
            }
        }

        private static void pmlCommit()
        {
            NotepadPPGateway notepadPPGateway = new NotepadPPGateway();

            //Save the file to original location
            string path = notepadPPGateway.GetCurrentFilePath();

            //check the file is committed file or not.
            if (path.Contains("temp"))
            {
                MessageBox.Show("This file can not be committed. Revert to this version before committing.");
                return;
            }


            //Open Commit form to enter commit message
            CommitWindow commitFrm = new CommitWindow();
            commitFrm.Show();

            //Save the current file

            //Take file path

            //Copy the file to new directory and rename

            //Open the renamed file and add committed message save & close
        }

        internal static void SetToolBarIcon()
        {
            toolbarIcons commitIcons = new toolbarIcons();
            commitIcons.hToolbarBmp = tbBmp_tbTab.GetHbitmap();
            IntPtr pcommitIcons = Marshal.AllocHGlobal(Marshal.SizeOf(commitIcons));
            Marshal.StructureToPtr(commitIcons, pcommitIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[0]._cmdID, pcommitIcons);
            Marshal.FreeHGlobal(pcommitIcons);

            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idMyDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);

            //revert_tbTab
            toolbarIcons revertIcons = new toolbarIcons();
            revertIcons.hToolbarBmp = revert_tbTab.GetHbitmap();
            IntPtr revertTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(revertIcons));
            Marshal.StructureToPtr(revertIcons, revertTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[2]._cmdID, revertTbIcons);
            Marshal.FreeHGlobal(revertTbIcons);
        }

        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }





        //internal static void myMenuFunction()
        //{
        //    MessageBox.Show("Hello N++!");
        //}

        //internal static void myDockableDialog()
        //{
        //    if (frmMyDlg == null)
        //    {
        //        frmMyDlg = new frmMyDlg();

        //        using (Bitmap newBmp = new Bitmap(16, 16))
        //        {
        //            Graphics g = Graphics.FromImage(newBmp);
        //            ColorMap[] colorMap = new ColorMap[1];
        //            colorMap[0] = new ColorMap();
        //            colorMap[0].OldColor = Color.Fuchsia;
        //            colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
        //            ImageAttributes attr = new ImageAttributes();
        //            attr.SetRemapTable(colorMap);
        //            g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
        //            tbIcon = Icon.FromHandle(newBmp.GetHicon());
        //        }

        //        NppTbData _nppTbData = new NppTbData();
        //        _nppTbData.hClient = frmMyDlg.Handle;
        //        _nppTbData.pszName = "My dockable dialog";
        //        _nppTbData.dlgID = idMyDlg;
        //        _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
        //        _nppTbData.hIconTab = (uint)tbIcon.Handle;
        //        _nppTbData.pszModuleName = PluginName;
        //        IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
        //        Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

        //        Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
        //    }
        //    else
        //    {
        //        Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmMyDlg.Handle);
        //    }
        //}
    }
}