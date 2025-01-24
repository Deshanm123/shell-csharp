while (true)
{
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
        Console.WriteLine(command.Replace("echo", ""));
    }
    else
    {
        Console.WriteLine($"{command}: command not found");
    }
}

