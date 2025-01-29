
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;



//shell built-in arr
string[] shellKeyWordsArr = ["echo", "type", "exit", "pwd","cd","cat"];
string[] GetPathDirectives()
{
    // Retrieve the PATH environment variable, or use an empty string if null.
    string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
    // Split the PATH string into individual directories based on the platform's PATH separator ignoring any empty entries.
    string[] pathDirs = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    return pathDirs;
} 

bool isShellKeyword(string commandkeyword)
{
    return Array.Exists(shellKeyWordsArr, keyword => keyword == commandkeyword);
}


string GetExecutableByName(string progName)
{
    string filepath = "";
    foreach (var path in GetPathDirectives())
    {
        filepath = Path.Combine(path, progName);
        if (File.Exists(filepath))
           break;
    }
    return filepath;
}

while (true)
{
    Console.Write("$ ");

    // Wait for user input
    var command = Console.ReadLine();

    if (command == "exit 0")
    {
        //exit command implementation
        Environment.Exit(0);
    }
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("echo "))
    {
        string strKeyword = command.Substring(4).Trim();
        if( strKeyword.StartsWith("\'") && strKeyword.EndsWith("\'"))
        {
           // echo 'test shell'=> test shell

            char[] charArr = strKeyword.ToCharArray();
            char[] nwArr = charArr.Where(character => character != '\'')
                                  .ToArray();
            Console.WriteLine(string.Join("", nwArr));
        }
        else
        {
            //$ echo test     shell =>  test shell
            string[] keywordsArr = strKeyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string correctSpacedWords = string.Join(" ", keywordsArr);
            Console.WriteLine(correctSpacedWords);
        }
        
    }
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("type "))
    {
        //indentifying reserved shell keyword by Type
        string strKeyword = command.Substring(5).Trim();
        if (isShellKeyword(strKeyword))
            Console.WriteLine($"{strKeyword} is a shell builtin");
        else
        {
            var fullPath = "";
            foreach (var path in GetPathDirectives())
            {
                fullPath = Path.Combine(path, strKeyword);
                if (Path.Exists(fullPath))
                    break;
                else
                    fullPath = "";
            }
            if (!string.IsNullOrEmpty(fullPath))
                Console.WriteLine($"{strKeyword} is {fullPath}");
            else
                Console.WriteLine($"{strKeyword}: not found");
        }
    }
    else if (command == "pwd")
    {
        Console.WriteLine(Directory.GetCurrentDirectory());
    }
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("cd "))
    {
        var location = command.Substring(2).Trim();

        if (location.Contains('~'))
        {
            string homeEnvVariable = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            location = location.Replace("~", homeEnvVariable);
        }

        //absolute path handling
        if (!String.IsNullOrEmpty(location) && Path.Exists(location))
        {
            try
            {
                Directory.SetCurrentDirectory(location);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"cd: {location}: No such file or directory");
            }
        }
        else
        {
            Console.WriteLine($"cd: {location}: No such file or directory");
        }
    }
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("cat "))
    {
        string filPathstr = command.Substring(4).Trim();
        string[] filePaths = filPathstr.Split("\'",StringSplitOptions.RemoveEmptyEntries);
        string fileContent = "";
        foreach (string filePath in filePaths)
        {
           // char[] noSpacePathArr = filePath.ToCharArray().Where(character => character != ' ').ToArray();
            //string noSpacePath = string.Join("",noSpacePathArr);
           //string fullPath = Path.GetFullPath(GetExecutableByName(noSpacePath));
           if(filePath != " ")
           {
                try
                {
                    Console.WriteLine(File.ReadAllText(filePath));
                    fileContent = fileContent + File.ReadAllText(filePath);
                }
                catch (Exception ex) 
                { 
                    Console.WriteLine(ex.Message);
                }

           }

        }
        Console.WriteLine(fileContent);
    }
    else
    {
        if (!String.IsNullOrEmpty(command))
        {
            string[] commandContentArr = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string progName = commandContentArr[0].Trim();
            string progArgs = string.Join(" ", commandContentArr.Where((arg, index) => index != 0));
            if (!String.IsNullOrEmpty(GetExecutableByName(progName)))
            {
                //Executing the executable
                try
                {
                    using var process = new Process();
                    process.StartInfo.FileName = progName;//GetExecutableByName(progName);
                    process.StartInfo.Arguments = progArgs;
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{command}: command not found");
                }
            }
            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
        else
        {
            Console.WriteLine($"{command}: command not found");
        }

    }
}

