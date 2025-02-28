using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_shell.src
{
    public class FileIOHandler
    {

       public  async Task<string> ReadTheFileContent(string filePath)
        {
            string fileContent = "";
            try
            {
                if (File.Exists(filePath))
                {
                    string temp = await File.ReadAllTextAsync(filePath);
                    fileContent += temp;
                }
                else
                {
                    Console.WriteLine("File doesn't exist in the path \n" + filePath);
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
            return fileContent;
        }

        public async Task WriteContentToFile(string filePath, string content)
        {
            await File.WriteAllTextAsync(filePath, content);
        }
    }
}
