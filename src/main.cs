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
        //Console.Write(0);
        Environment.Exit(0);
        //Console.WriteLine("0");
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
        string strKeyword = command.Split(" ")[1].Trim();
        //#Array.Exists(shellKeyWordsArr, keyword => keyword == strKeyword);
        bool isaShellKeyword = Array.Exists(shellKeyWordsArr, keyword => keyword == strKeyword);
        if (isaShellKeyword)
            Console.WriteLine($"{strKeyword} is a shell builtin");
        else
            Console.WriteLine($"{strKeyword}: not found");

    }
    else
    {
        Console.WriteLine($"{command}: command not found");
    }
}

