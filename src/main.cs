
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;



//shell built-in arr
string[] shellKeyWordsArr = ["echo", "type", "exit", "pwd","cd","cat"];

//Get user path environments according to OS
string[] GetPathDirectives()
{
    // Retrieve the PATH environment variable, or use an empty string if null.
    string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
    // Split the PATH string into individual directories based on the platform's PATH separator ignoring any empty entries.
    string[] pathDirs = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
    return pathDirs;
} 

//shell reserved words check 
bool isShellKeyword(string commandkeyword)
{
    return Array.Exists(shellKeyWordsArr, keyword => keyword == commandkeyword);
}


//Program can be found some where in directives.
//Therefore program name is passed to get the file location
string GetExecutableByName(string progName)
{
    string filepath = "";
    foreach (var path in GetPathDirectives())
    {
        var tempfilepath = Path.Join(path, progName);
        if (File.Exists(filepath))
        {
           filepath = tempfilepath;
           break;
        }
    }
    return filepath;
}


//string getJointPathsWithPathDirectives(string path)
//{
//   string filePath = "";
//   char[] pathArr = path.ToCharArray() 
//                          .Where((chr,ind) =>   ind != 0 && chr != '/') //  remove / if present  in path as first element cuz dirPath has / at end  
//                          .Where((chr, ind) => ind != path.Length-1 && chr != '\'')  //   remove single quote at the end of the path
//                          .ToArray();
//   string validPath = string.Join("", pathArr);
//   foreach (var dirPath in GetPathDirectives())
//    {
//        var tempPath = Path.Join(dirPath, validPath);
//        Console.WriteLine(tempPath);    
//        if (Path.Exists(tempPath))
//        {
//            filePath = tempPath;
//            return filePath;
//        }
//    }
//    return filePath;
//}




bool RunTheExecutable(string progName, string progArgs)
{
    bool isProcessWorking;
    //Executing the executable
    try
    {
        using var process = new Process();
        process.StartInfo.FileName = progName;//GetExecutableByName(progName);
        process.StartInfo.Arguments = progArgs;
        process.Start();
        isProcessWorking = true;
    }
    catch (Exception ex)
    {
        isProcessWorking = false;
    }
    return isProcessWorking;
}

string ReadTheFileContent(string filePath)
{
   // Console.WriteLine("****" + filePath);
    string fileContent = "";
    try
    {
        //if (File.Exists(filePath))
        //{
            fileContent = File.ReadAllText(filePath);
            return fileContent;
        //}
        //else
        //{
            Console.WriteLine("File doesn't exsist in the path \n" + filePath);
        //}
    }
    catch (Exception ex) 
    { 
        // Console.WriteLine(ex.Message); }
    } 
    return fileContent;
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
        string filPathstr = command.Substring(4);
        string pattern = "'([^']+)'";
        var validPaths = Regex.Matches(filPathstr, pattern).ToArray();

        //string[] filePaths = filPathstr.Split("\'",StringSplitOptions.RemoveEmptyEntries);
        //string[] filePaths = filPathstr.Split(' ',StringSplitOptions.RemoveEmptyEntries);
        //string[] filePathsReg = Regex.Split(filPathstr, pattern);
        if (validPaths.Length > 0)
        {
            string fileContent = "";
            foreach (var filePathRegex in validPaths)
            {
                string _filePath = filePathRegex.Value;
                char[] pathArr = _filePath.ToCharArray()
                          .Where((chr, ind) => ind != 0 && chr != '/') //  remove / if present  in path as first element cuz dirPath has / at end  
                          .Where((chr, ind) => ind != _filePath.Length - 1 && chr != '\'')  //   remove single quote at the end of the path
                          .ToArray();
                string filePath = string.Join("", pathArr);
                Console.WriteLine($"{filePath}");
                var _fileContent = File.ReadAllText(filePath);
                //fileContent += _fileContent;
                // char[] noSpacePathArr = filePath.ToCharArray().Where(character => character != ' ').ToArray();
                // string noSpacePath = string.Join("", noSpacePathArr);
                // //string filePath = Path.GetFullPath(GetExecutableByName(filePathx));
                // string filePathX = getJointPathsWithPathDirectives(noSpacePath);
                // Console.WriteLine("llast pathj "+ filePathX);
                //// if (!string.IsNullOrWhiteSpace(filePath))
                //// {
                //     //var result = ReadTheFileContent(filePath);
                //    // Console.WriteLine("***" + result + "xxxxxx");
                //     fileContent  = fileContent + ReadTheFileContent(filePathX);
                //// }

            }
            Console.WriteLine(fileContent);
        }
        else
        {
            Console.WriteLine(filPathstr);
        }
    }
    else
    {
        if (!String.IsNullOrEmpty(command))
        {
            string[] commandContentArr = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string progName = commandContentArr[0].Trim();
            if (!String.IsNullOrEmpty(GetExecutableByName(progName)))
            {
                string progArgs = string.Join(" ", commandContentArr.Where((arg, index) => index != 0));
                //Executing the executable
                if(!RunTheExecutable(progName,progArgs))
                    Console.WriteLine($"{command}: command not found");
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

