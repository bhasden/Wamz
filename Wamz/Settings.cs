using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Wamz
{
    public partial class Settings : Form
    {
        private AboutBox m_AboutBox = new AboutBox();
        public Settings()
        {
            InitializeComponent();
        }

        #region Button Events
        private void Browse_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(WatchDirectory.Text))
            {
                folderBrowserDialog1.SelectedPath = WatchDirectory.Text;
            }

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                WatchDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            AppSettings.Default.WatchDirectory = WatchDirectory.Text;
            fileSystemWatcher1.Path = WatchDirectory.Text;
            this.Close();
        }
        #endregion

        #region Form Events
        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ShowInTaskbar = true;

            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                this.Hide();

                if (AppSettings.Default.ShowCloseTip)
                {
                    AppSettings.Default.ShowCloseTip = false;
                    notifyIcon1.ShowBalloonTip(2500, "Information", "Wamz is still running. To close, right-click on the Wamz icon and select 'Close' from the options.", ToolTipIcon.Info);
                }
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            fileSystemWatcher1.Path = WatchDirectory.Text;
        }

        private void Settings_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                WatchDirectory.Text = AppSettings.Default.WatchDirectory;
                this.ShowInTaskbar = true;
            }
        }
        #endregion

        #region ToolStrip Events
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        #endregion

        private void fileSystemWatcher1_Event(object sender, FileSystemEventArgs e)
        {
            ProcessFiles();
        }

        private void fileSystemWatcher1_Event(object sender, RenamedEventArgs e)
        {
            fileSystemWatcher1_Event(sender, e);
        }

        private void ProcessFiles()
        {
            try
            {
                //Loop through all available files in case we missed a .amz file earlier
                foreach (string File in Directory.GetFiles(fileSystemWatcher1.Path, fileSystemWatcher1.Filter, fileSystemWatcher1.IncludeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                {
                    Process.Start(File);
                }
            }
            catch
            {
                //Eat exception since there's really nothing we can do
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_AboutBox.Visible != true)
            {
                m_AboutBox.Show();
            }
        }
    }
}
