using System.IO;

namespace Doplace.Helpers
{
    public static class FileHelper
    {
        public static void RenameFile(string filePath, string oldfileName, string newFileName)
        {
            filePath = filePath.EndsWith("/") ? filePath : filePath + "/";
            if (File.Exists(filePath + oldfileName))
            {
                File.Copy(filePath + oldfileName, filePath + newFileName);
                File.Delete(filePath + oldfileName);
            }
        }

        public static void CreateFolder(string path, string folderName)
        {
            if (Directory.Exists(path + folderName)) return;
            Directory.CreateDirectory(path + folderName);
        }
    }
}
