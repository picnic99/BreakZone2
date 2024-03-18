using System;
using System.IO;

namespace StateSyncServer.LogicScripts.Manager
{
    public class ResourceManager : Manager<ResourceManager>
    {
        public string GetConfigRes(string fileName)
        {
            string filePath = @"../../Config/" + fileName + ".json";
            return GetRes(filePath);
        }
        public string GetAnimClipDataRes()
        {
            string filePath = @"../../CustomConfig/animData.json";
            return GetRes(filePath);
        }

        private string GetRes(string filePath)
        {
            try
            {
                // 读取文件的全部内容作为一个字符串  
                string fileContent = File.ReadAllText(filePath);

                // 输出文件内容  
                //Console.WriteLine(fileContent);

                return fileContent;
            }
            catch (IOException e)
            {
                // 输出错误消息  
                Console.WriteLine("An error occurred while reading the file: " + e.Message);
            }
            return "";
        }



    }
}
