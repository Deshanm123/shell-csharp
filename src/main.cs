while (true)
{
    // Uncomment this line to pass the first stage
    Console.Write("$ ");

    // Wait for user input
    var command = Console.ReadLine();

    //exit command implementation
    if (command == "exit")
    {
        Console.WriteLine($"$ {command} 0");
        break;
    }
    Console.WriteLine($"{command}: command not found");
}

