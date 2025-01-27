

using System.Diagnostics;
using System.Globalization;

while (true)
{
    //shell built-in arr
    string[] shellKeyWordsArr = ["echo", "type", "exit"];
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    var command = Console.ReadLine();

    //exit command implementation
    if (command == "exit 0")
    {
        Environment.Exit(0);
        //break;
    }
    else if(!String.IsNullOrEmpty(command) && command.StartsWith("echo "))
    {
        //printing as output
        Console.WriteLine(command.Replace("echo ", ""));
    }
    else if (!String.IsNullOrEmpty(command) && command.StartsWith("type "))
    {
        //indentifying reserved shell keyword by Type
        string strKeyword = command.Substring(5).Trim();
        bool isaShellKeyword = Array.Exists(shellKeyWordsArr, keyword => keyword == strKeyword);
        if (isaShellKeyword)
            Console.WriteLine($"{strKeyword} is a shell builtin");
        else
        {
            // Retrieve the PATH environment variable, or use an empty string if null.
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";

            // Split the PATH string into individual directories based on the platform's PATH separator ignoring any empty entries.
            string[] pathDirs = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

            var fullPath = "";
            foreach (var path in pathDirs){
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
    else
    {
        if (!String.IsNullOrEmpty(command))
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
            string[] pathDirsArr = pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            string[] commandContentArr = command.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            string progName = commandContentArr[0].Trim();
            string progArgs = string.Join(" ", commandContentArr.Where((arg, index) => index != 0 ));
            
            var progPath = "";
           
            foreach (var path in pathDirsArr)
            {
                var tempPath = Path.Combine(path, command);
                if (Path.Exists(tempPath))
                {
                    Console.WriteLine($"Debug: tempPath Variable value is {tempPath}"); ;
                    //Get Executable file
                    progPath = Path.GetFullPath(tempPath);
                    Console.WriteLine($"Debug: Variable value is {progPath}"); ;
                    //Executing the executable
                    using var process = new Process();
                    process.StartInfo.FileName = progPath;
                    process.StartInfo.Arguments = progArgs;
                    process.Start();
                    
                    break;
                }
            }
            

        }
        else
        {
            Console.WriteLine($"{command}: command not found");
        }
    }
}

