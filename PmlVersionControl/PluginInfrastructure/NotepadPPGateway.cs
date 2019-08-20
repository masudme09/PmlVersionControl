// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET.PluginInfrastructure
{
	public interface INotepadPPGateway
	{
		void FileNew();

		string GetCurrentFilePath();
		unsafe string GetFilePath(int bufferId);
		void SetCurrentLanguage(LangType language);
	}

	/// <summary>
	/// This class holds helpers for sending messages defined in the Msgs_h.cs file. It is at the moment
	/// incomplete. Please help fill in the blanks.
	/// </summary>
	public class NotepadPPGateway : INotepadPPGateway
	{
		private const int Unused = 0;

		public void FileNew()
		{
			Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_MENUCOMMAND, Unused, NppMenuCmd.IDM_FILE_NEW);
		}

		/// <summary>
		/// Gets the path of the current document.
		/// </summary>
		public string GetCurrentFilePath()
		{
			var path = new StringBuilder(2000);
			Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);
			return path.ToString();
		}

        /// <summary>
        /// This method save the currentfile file
        /// </summary>
        public void SaveCurrentFile()
        {
            string sessionPath =this.GetCurrentFilePath();
            Win32.SendMessage(PluginBase.nppData._nppHandle,(uint) NppMsg.NPPM_SAVEFILE, 0, sessionPath);
            //if (!string.IsNullOrEmpty(sessionPath))
            //    MessageBox.Show(sessionPath, "Saved Session File :", MessageBoxButtons.OK);
        }

        /// <summary>
        /// This method save the currentfile file to given Path
        /// </summary>
        public void SaveCurrentFile(string filePath)
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SAVECURRENTFILEAS, 0, filePath);
           
        }

        /// <summary>
        /// This method close the currentfile file to given Path
        /// </summary>
        public void CloseCurrentFile(string filePath)
        {
            //Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_MENUCOMMAND, Unused, NppMenuCmd.IDM_FILE_NEW);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_CLOSE);

        }

        /// <summary>
        /// Gets the path of the current document.
        /// </summary>
        public unsafe string GetFilePath(int bufferId)
		{
			var path = new StringBuilder(2000);
			Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufferId, path);
			return path.ToString();
		}

		public void SetCurrentLanguage(LangType language)
		{
			Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETCURRENTLANGTYPE, Unused, (int) language);
		}
	}

	/// <summary>
	/// This class holds helpers for sending messages defined in the Resource_h.cs file. It is at the moment
	/// incomplete. Please help fill in the blanks.
	/// </summary>
	class NppResource
	{
		private const int Unused = 0;

		public void ClearIndicator()
		{
			Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) Resource.NPPM_INTERNAL_CLEARINDICATOR, Unused, Unused);
		}
	}
}
