namespace PmlVersionControl
{
    partial class CommitWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommitWindow));
            this.richTxtCommit = new System.Windows.Forms.RichTextBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chkFinal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // richTxtCommit
            // 
            resources.ApplyResources(this.richTxtCommit, "richTxtCommit");
            this.richTxtCommit.Name = "richTxtCommit";
            // 
            // btnCommit
            // 
            this.btnCommit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.btnCommit, "btnCommit");
            this.btnCommit.ForeColor = System.Drawing.Color.Black;
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.UseVisualStyleBackColor = false;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkFinal
            // 
            resources.ApplyResources(this.chkFinal, "chkFinal");
            this.chkFinal.Name = "chkFinal";
            this.chkFinal.UseVisualStyleBackColor = true;
            // 
            // CommitWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.chkFinal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.richTxtCommit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommitWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTxtCommit;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFinal;
    }
}