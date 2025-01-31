
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


//shell built-in arr
string[] shellKeyWordsArr = ["echo", "type", "exit", "pwd","cd"];

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

string GetExecutableByName(string progName)
{
    string filepath = "";
    foreach (var path in GetPathDirectives())
    {
        var tempfilepath = Path.Join(path, progName);
        if (File.Exists(tempfilepath))
        {
           // Console.WriteLine("path found" + filepath);
           filepath = tempfilepath;
           break;
        }
    }
    return filepath;
}

Match[] GetPatternMatchesByRegex(string strPhrase, string regPattern)
{
    //single -Match[] keywords = GetPatternMatchesByRegex(strKeyword, "'([^']+)'");
    return Regex.Matches(strPhrase, regPattern).ToArray();
}
bool RunTheExecutable(string progName, string progArgs)
{
    //Executing the executable
    try
    {
        using var process = new Process();
        process.StartInfo.FileName = progName;//GetExecutableByName(progName);
        process.StartInfo.Arguments = progArgs;
        process.Start();
        return true;
    }
    catch (Exception ex)
    {
    }
    return false;
}

string ReadTheFileContent(string filePath)
{
    string fileContent = "";
    try
    {
        if (File.Exists(filePath))
        {
            fileContent = File.ReadAllText(filePath);
            return fileContent;
        }
        Console.WriteLine("File doesn't exsist in the path \n" + filePath);
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

    if (command == null) continue;  // Handle unexpected null input

    if (command == "exit 0")
    {
        //exit command implementation
        Environment.Exit(0);
    }
    
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("echo "))
    {
        string strKeyword = command.Substring(4).Trim();
        if (strKeyword.StartsWith("\'") && strKeyword.EndsWith("\'"))
        {
            char[] output = strKeyword.ToCharArray()
                                      .Where(character => character != '\'')
                                      .ToArray();
            Console.WriteLine(string.Join("",output));
        }
        else
        {
           // $ echo test     shell => test shell
            string[] keywordsArr = strKeyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine(string.Join(" ", keywordsArr));
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
        string strKeyword = command.Substring(3);
        Match[] keywords = GetPatternMatchesByRegex(strKeyword, "'([^']+)'");

        ArgumentNullException.ThrowIfNull(strKeyword);

        if (keywords != null && keywords.Count() > 0)
        {
            string output = "";
            foreach (Match match in keywords)
            {
                char[] _output  = match.Value.ToCharArray()
                                               .Where(character => character != '\'')
                                               .ToArray();
                string _path = string.Join("", _output);
                var path = ReadTheFileContent(_path);
                output+= path; 
            }
            Console.WriteLine(output);
        }
        // Ensure prompt is printed after execution

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


