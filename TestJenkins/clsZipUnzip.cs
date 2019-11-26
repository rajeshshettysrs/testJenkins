using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace Download.ZipUnzip
{
    class clsZipUnzip
    {
        public void CreateZipFile(string[] SourceFile, string DestFileName)
        {
            FileStream outputFileStream = new FileStream(DestFileName, FileMode.Create);
            ZipOutputStream zipStream = new ZipOutputStream(outputFileStream);
            ZipFile zipFile = null;
            for (int intIndex = 0; intIndex < SourceFile.Length; intIndex++)
            {
                FileInfo fiFile = new FileInfo(SourceFile[intIndex]);
                ZipEntry zipEntry = new ZipEntry(fiFile.Name);
                Stream inputStream = new FileStream(fiFile.FullName, FileMode.Open);

                zipEntry = new ZipEntry(fiFile.Name);
                zipEntry.IsCrypted = false;
                zipEntry.CompressionMethod = CompressionMethod.Deflated;
                
                zipStream.PutNextEntry(zipEntry);
                CopyStream(inputStream, zipStream);
                inputStream.Close();

                zipStream.CloseEntry();
            }
            if (zipFile != null)
            {
                zipFile.Close();
            }
            zipStream.Finish();
            zipStream.Close();
        }

        /// <summary>
        /// Adding more file to Zip
        /// </summary>
        /// <param name="source">New file</param>
        /// <param name="destination">Existing Zip File</param>
        private void CopyStream(Stream source, Stream destination)
        {
            byte[] buffer = new byte[4096];
            int countBytesRead;
            while ((countBytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                destination.Write(buffer, 0, countBytesRead);
            }
        }

        /// <summary>
        /// Function to Unzipp all the File From Zip File
        /// </summary>
        /// <param name="ZipFile"></param>
        /// <param name="DestinationDir"></param>
        /// <returns></returns>
        public string UnzipFiles(string zipFile, string destinationDir, out int totalFiles)
        {
            string tempZipFilePath = destinationDir + "\\" + Path.GetFileName(zipFile);
            string fileName = string.Empty;
            ZipEntry theEntry = null;

            //Create directory if it doesn't exist and then copy file there and then extract
            if (!Directory.Exists(destinationDir))
                Directory.CreateDirectory(destinationDir);
            File.Copy(zipFile, tempZipFilePath, true);
            totalFiles = 0;

            //We will just count the number of files which are there
            try
            {
                using (ZipInputStream streamInput =
                    new ZipInputStream(File.OpenRead(tempZipFilePath)))
                {
                    while ((theEntry = streamInput.GetNextEntry()) != null)
                    {
                        totalFiles++;
                    }
                }
                if (totalFiles > 1)
                {
                    totalFiles = 100001;
                    return null;
                }

                using (ZipInputStream streamInput =
                    new ZipInputStream(File.OpenRead(tempZipFilePath)))
                {

                    while ((theEntry = streamInput.GetNextEntry()) != null)
                    {
                        fileName = Path.GetFileName(theEntry.Name);
                        if (fileName != String.Empty)
                        {
                            using (FileStream streamWriter =
                                File.Create(destinationDir + "\\" + theEntry.Name))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = streamInput.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                return destinationDir + "\\" + fileName;
            }
            catch
            {
                totalFiles = 0;
                return null;
            }
            finally
            {
                if (File.Exists(tempZipFilePath))
                {
                    File.Delete(tempZipFilePath);
                }
            }
        }
    }
}
