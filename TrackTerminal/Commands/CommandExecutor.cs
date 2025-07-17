namespace BoxSearch.Commands;

internal class CommandExecutor(CommandRegistry registry)
{
    private readonly CommandRegistry _registry = registry;

    public async Task<int> ExecuteAsync(string[] args)
    {
        Definitions.ParsedCommand? parsed = CommandLineParser.Parse(args);
        if (parsed is null || parsed.CommandName == "help")
        {
            _registry.ShowHelp();
            return 1;
        }

        Definitions.CommandDefinition? command = _registry.GetCommand(parsed.CommandName);
        if (command is null)
        {
            Console.WriteLine($"Unknown command: {parsed.CommandName}");
            Console.WriteLine();
            _registry.ShowHelp();
            return 1;
        }

        if (parsed.Options.ContainsKey("help"))
        {
            _registry.ShowCommandHelp(command);
            return 0;
        }

        try
        {
            return await command.Handler(parsed);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}