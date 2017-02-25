using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class FileIO
    {

        ///
        /// \brief Creates specified file and folder if they don't exist
        /// \details <b>Details</b> 
        /// \param fileName - <b>string </b> -the name of the file
        /// \param folderName - <b>string </b> -the name of the folder, folder should be under the same directory the executable locates
        /// \param fileFilter - <b>string </b> -file's extention, e.g. ".csv" for dBase; ".log" for log
        /// \return true <b>bool </b> - if file and folder are created successfully
        /// \return false <b>bool </b> - if file and folder are not created 
        ///
        public bool CreateFile(string fileName, string folderName, string fileFilter)
        {

            //create folder if "DBase" doesn't exist
            bool isFileExist = false;

            string folderPath = Directory.GetCurrentDirectory() + "\\" + folderName + "\\";
            try
            {
                // create the requred folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

                //open and scan folder if it exist
                FileInfo[] fileInfo = directoryInfo.GetFiles(fileFilter, SearchOption.AllDirectories);

                foreach (FileInfo fileInfoItem in fileInfo)
                {
                    if (fileInfoItem.Name == fileName)
                    {
                        isFileExist = true;
                    }
                }

                //create the file if it doesn't exist in the DBase folder
                if (!isFileExist)
                {
                    // Create a file to write to. 
                    StreamWriter sw = File.CreateText(folderPath + fileName);
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        ///
        /// \brief Creates specified file and folder if they don't exist
        /// \details <b>Details</b>  Must create file by using CreateFile() before using GetStreamWriter()
        /// \param fileName - <b>string </b> - specify the name of the file of which the stream writer is returned
        /// \param folderName - <b>string </b> -the name of the folder where file is put, folder should be under the same directory the executable locates
        /// \param fileFilter - <b>string </b> -the file's extention, e.g. ".csv" for dBase; ".log" for log
        /// \return sr <b>StreamReader </b> - when the stream writer of the specified is successfuly achieved
        /// \return null if the stream writer is not achieved
        /// 
        public StreamWriter GetStreamWriter(string fileName, string folderName, string fileFilter)
        {
            StreamWriter sw = null;
            string folderPath = Directory.GetCurrentDirectory() + "\\" + folderName + "\\";
            try
            {

                if (folderName == "log")
                {
                    sw = new StreamWriter(folderPath + fileName, true);
                }
                else
                {
                    sw = new StreamWriter(folderPath + fileName, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return sw;
        }
    }
}
