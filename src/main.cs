using System.Diagnostics;
using System.Text.RegularExpressions;


//shell built-in arr
string[] shellKeyWordsArr = ["echo", "type", "exit", "pwd", "cd"];

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

async Task<bool> RunTheExecutable(string progName, string progArgs)
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


char[] RemoveCharFromString(string keyword, char character)
{
    return keyword.ToCharArray()
                   .Where(chr => chr != character)
                   .ToArray();
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
            char[] output = RemoveCharFromString(strKeyword, '\'');
            Console.WriteLine(string.Join("", output));
        }
        else if (strKeyword.StartsWith("\"") && strKeyword.EndsWith("\""))
        {

            char[] strCharArr = strKeyword.ToCharArray();
            string quotationState = "Closed";
            bool spaceAdded = false;

            for (int i = 0; i < strCharArr.Length; i++)
            {
                char? c = strCharArr[i];
                if (c == '\"')
                {
                    if (quotationState == "Closed")
                    {
                        quotationState = "Open";
                        spaceAdded = false;
                    }
                    else
                    {
                        quotationState = "Closed";
                    }
                }
              
                switch (quotationState)
                {
                    case "Open":
                        switch (c)
                        {
                            case '\"':
                                {
                                    break;
                                }
                            case '\\':
                                {
                                    char nextChar = strCharArr[i + 1];
                                    //escape character
                                    
                                    if(nextChar == '\"')
                                        Console.Write('\"');
                                    else if (nextChar != '\\')
                                        Console.Write('\\');
                                    break;
                                }
                            case '\'':
                                {
                                    Console.Write('\'');
                                    break;
                                }
                            
                            default:
                                {
                                    Console.Write(c);
                                    break;
                                }
                        }
                        break;

                    case "Closed":
                        switch (c)
                        {
                            case ' ':
                                {
                                    if (!spaceAdded)
                                    {
                                        Console.Write(' ');
                                        spaceAdded = true;
                                    }
                                    break;
                                }
                            case '\"':
                                {
                                    //you cant have double quotes with in a double quotes
                                    //if it's thats simply a seperation of a strings
                                    break;
                                }
                            case '\\':
                                {
                                    char nextChar = strCharArr[i + 1];
                                    if (nextChar != '\\')
                                        Console.Write('\\');
                                    break;
                                }
                            default:
                                {
                                    Console.Write(c);
                                    break;
                                }
                        }
                        break;
                }

            }
            Console.Write('\n');
    
        }
        else
        {
            // $ echo test     shell => test shell
            string[] keywordsArr = strKeyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string output = string.Join(" ", keywordsArr);
            //remove backslash character from a output when displaying /escape characcter
            if (output.Contains('\\'))
            {
                var _ = RemoveCharFromString(output, '\\');
                Console.WriteLine(string.Join("", _));
            }
            else
            {
                Console.WriteLine(output);
            }
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
        string strKeyword = command.Substring(3).Trim();
        Match[] keywords = new Match[] { };
        bool isDoubleQuotes = false;

        ArgumentNullException.ThrowIfNull(strKeyword);

        if (strKeyword.StartsWith("\'") && strKeyword.EndsWith("\'"))
        {
            keywords = GetPatternMatchesByRegex(strKeyword, "'([^']+)'");
        }
        else if (strKeyword.StartsWith("\"") && strKeyword.EndsWith("\""))
        {
            keywords = GetPatternMatchesByRegex(strKeyword, "\"([^\"]+)\"");
            isDoubleQuotes = true;
        }

        if (keywords != null && keywords.Count() > 0)
        {
            foreach (Match match in keywords)
            {
                char quoteChr = isDoubleQuotes ? '\"' : '\'';
                char[] _output = match.Value.ToCharArray()
                                             .Where(chr => chr != quoteChr)
                                             .ToArray();
                string _path = string.Join("", _output);
                var content = ReadTheFileContent(_path);
                Console.Write(content);
            }
        }
        else
        {
            Console.WriteLine(strKeyword);
        }

    }
    else
    {
        if (Regex.IsMatch(command, ".+exe.*with.*"))
        {
            int leftSlashIndex = Array.IndexOf(command.ToCharArray(), '/');
            string progPath = command.Substring(leftSlashIndex);
            //var xx = command.ToCharArray().First((chr, ind) => { if (chr == '/') return ind; });
           // string progPath = command.Replace("'exe  with  space'", "").Trim();
            var content = ReadTheFileContent(progPath);
            Console.Write(content);
        }
        else if (!String.IsNullOrEmpty(command))
        {
            
            string[] commandContentArr = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string progName = commandContentArr[0].Trim();

            if (!String.IsNullOrEmpty(GetExecutableByName(progName)))
            {
                string progArgs = string.Join(" ", commandContentArr.Where((arg, index) => index != 0));
                //Executing the executable
                if (!await RunTheExecutable(progName, progArgs))
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


