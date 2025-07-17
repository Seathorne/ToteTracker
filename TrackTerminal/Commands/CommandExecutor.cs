using BoxSearch.Commands.Definitions;

namespace BoxSearch.Commands;

internal class CommandExecutor(CommandRegistry registry)
{
    private readonly CommandRegistry _registry = registry;

    public async Task<int> ExecuteAsync(string[] args)
    {
        ParsedCommand? parsed = CommandLineParser.Parse(args);
        if (parsed is null)
        {
            _registry.ShowHelp();
            return 1;
        }
        else if (parsed.CommandName == "help"
              || parsed.CommandName is null && parsed.Options.ContainsKey("help"))  // special exception for --help
        {
            _registry.ShowHelp();
            return 0;
        }
        else if (parsed.CommandName is null)
        {
            Console.WriteLine($"A command must be provided.");
            Console.WriteLine();
            _registry.ShowHelp();
            return 1;
        }

        CommandDefinition? command = _registry.GetCommand(parsed.CommandName);
        if (command is null)
        {
            Console.WriteLine($"Unknown command: {parsed.CommandName}");
            Console.WriteLine();
            _registry.ShowHelp();
            return 1;
        }

        if (parsed.Options.ContainsKey("help"))
        {
            // Show help for specified command
            _registry.ShowCommandHelp(command);
            return 0;
        }

        try
        {
            // Execute command asynchronously
            return await command.Handler(parsed);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}