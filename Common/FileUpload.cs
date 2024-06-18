using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Common
{
    public static class FileUpload
    {
        public static string CreateAndSaveFile(string fileName, int fileSize = 0)
        {
            string remoteHostName = Get.Parameter("server");
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            using (FileStream fileStream = File.Create(filePath))
            {
                if (fileSize > 0)
                {
                    // Create large file
                    byte[] dataArray = new byte[fileSize];
                    new Random().NextBytes(dataArray);
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        fileStream.WriteByte(dataArray[i]);
                    }
                }
                else
                {
                    // Create file containing string "Test text."
                    byte[] fileText = new UTF8Encoding(true).GetBytes("Test text.");
                    fileStream.Write(fileText, 0, fileText.Length);
                }
            }

            return SystemCfg.TransferFileToServer(filePath, fileName, remoteHostName);
        }
    }
}
