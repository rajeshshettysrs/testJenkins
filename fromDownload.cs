using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Download.ZipUnzip;

namespace TestJenkins
{
    public partial class fromDownload : Form
    {
        private string _pathToDownload;   
        public fromDownload()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            _pathToDownload = Application.StartupPath + "\\updates";

            if (!Directory.Exists(_pathToDownload))
            {
                Directory.CreateDirectory(_pathToDownload);
            }
        }

        private void unZipFiles()
        {
            clsZipUnzip unzip = null;
            unzip = new clsZipUnzip();
            string unZipPath = Path.GetDirectoryName(Application.StartupPath);
            string zipName = Path.ChangeExtension(Path.GetFileName(Application.StartupPath), ".zip");
            string zipPath = _pathToDownload + "\\" + zipName;
            int noOfFiles = 1;

            if (File.Exists(zipPath))
            {
                unzip.UnzipFiles(zipPath, unZipPath, out noOfFiles);
            }
        }


    }
}
