using System.ComponentModel.Design;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
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

//int getQuoteIndex(char chr, int ind)
//{
//    if (chr == '"') return ind;
//    else return;
//}
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
            //string[] keywordsArr = strKeyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            char[] strCharArr = strKeyword.ToCharArray();
            List<char> outputList = new List<char>() { };
            string quoatation = "Closed";
            bool spaceAdded = false;
            for (int i = 0; i < strCharArr.Length; i++)
            {
                char? c = strCharArr[i];
                if (c == '\"')
                {
                    if (quoatation == "Closed")
                    {
                        quoatation = "Open";
                        spaceAdded = false;
                    }
                    else
                    {
                        quoatation = "Closed";
                    }
                }
                   

                //if(c == '')
                //{
                //    quoatation = "None";
                //}
                switch (quoatation)
                {

                    case "Open":
                                switch ( c) 
                            {
                                case '\"':
                                {
                                    // = quoatation == "None" ?  "Double" :  "None";
                                    break;
                                }
                                case '\\':
                                    {
                                        Console.Write('\\');
                                        break;
                                    }
                                case '\'':
                                    {
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

                    case "Closed":
                        switch(c)
                        {
                            //'"'
                            case ' ':
                            {
                               if (!spaceAdded)
                               {
                                        Console.Write(' ');
                                        spaceAdded = true;
                               }
                            }
                                 break;
                        }
                        break;
                }

                
            }

            
            Console.Write('\n');
            //Console.WriteLine(string.Join("", outputList));


            /*
            //removing end trail " quote after removing echo and start trails
           // string _strKeyword = strKeyword.Substring(1,strKeyword.Length -2);
            string _strKeyword = strKeyword;

            bool foundDbleStartQuote = false;
            bool foundDbleEndQuote = false;
            int dbleQuoteStart = Int32.MinValue;
            int dbleQuoteEnd = Int32.MaxValue;

            List<string> strWordsList = new List<string>() { };
            //if escape characters present
            for (int i = 0; i < _strKeyword.Length; i++ )
            {
                Console.WriteLine(_strKeyword.Length);
               if (_strKeyword[i] == '"' && !foundDbleStartQuote)
               {
                    dbleQuoteStart = i;
                    foundDbleStartQuote = true;
               }
               else if (_strKeyword[i] == '"' && foundDbleStartQuote)
               {
                    dbleQuoteEnd = i;
                    foundDbleEndQuote = true;
               }

                if(foundDbleStartQuote  && foundDbleEndQuote)
                {
                    //copy
                    if (dbleQuoteStart + 1 < _strKeyword.Length)
                    {
                        int firstCharIndex = dbleQuoteStart + 1;
                        int  noOfChars = dbleQuoteEnd - dbleQuoteStart;

                        var word = _strKeyword.Substring(firstCharIndex, noOfChars);
                        strWordsList.Add(word);
                    }
                    foundDbleStartQuote = false;
                    foundDbleEndQuote = false;
                    dbleQuoteStart = Int32.MinValue;
                    dbleQuoteEnd = Int32.MaxValue;
                }
            }
            */

            /*
           // "shel""example"=> making them concat
            string _strKeyword = Regex.Replace(strKeyword, "\"\"", "").Trim();
            //not escape characters presenet
            Match[] keywords = GetPatternMatchesByRegex(_strKeyword, "\"([^\"]+)\"");

            if (keywords != null && keywords.Count() > 0)
            {
                List<string> strWordsList = new List<string>() { };
                foreach (Match match in keywords)
                {
                    char[] outputChrArr = RemoveCharFromString(match.Value, '\"');
                    var output = string.Join("", outputChrArr);
                    //check if escape character exists
                    if (output.Contains(@"\\"))
                    {
                        int index = output.IndexOf('\\');
                        strWordsList.Add(output.Remove(index, 1));
                    }
                    else
                    {
                        strWordsList.Add(output);
                    }
                }
                Console.WriteLine(string.Join(" ", strWordsList));
            }
            else
            {
                Console.WriteLine(_strKeyword);
            }
            */
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
                Console.WriteLine(string.Join("",_));
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

        if (!String.IsNullOrEmpty(command))
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


