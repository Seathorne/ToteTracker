using BoxSearch.Commands.Definitions;

namespace BoxSearch.Commands;

internal class CommandRegistry(string appName, string appDescription)
{
    private readonly Dictionary<string, CommandDefinition> _commands = [];
    private readonly string _appName = appName;
    private readonly string _appDescription = appDescription;

    public void Register(CommandDefinition command)
    {
        _commands[command.Name] = command;
    }

    public void Unregister(CommandDefinition command)
    {
        _ = _commands.Remove(command.Name);
    }

    public CommandDefinition? GetCommand(string name)
    {
        return _commands.GetValueOrDefault(name);
    }

    public IReadOnlyDictionary<string, CommandDefinition> Commands => _commands;

    public void ShowHelp()
    {
        Console.WriteLine($"{_appName} - {_appDescription}");
        Console.WriteLine();
        Console.WriteLine($"Usage: {_appName} <command> [options]");
        Console.WriteLine();
        Console.WriteLine("Commands:");

        foreach (CommandDefinition cmd in _commands.Values)
        {
            Console.WriteLine($"  {cmd.Name,-15} {cmd.Description}");
        }
    }

    public void ShowCommandHelp(CommandDefinition command)
    {
        Console.WriteLine($"{_appName} {command.Name} - {command.Description}");
        Console.WriteLine();
        Console.WriteLine($"Usage: {_appName} {command.Name} [options] [arguments]");

        if (command.Options.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Options:");

            foreach (OptionDefinition option in command.Options)
            {
                string flags = option.ShortFlag != null
                    ? $"-{option.ShortFlag}, --{option.LongFlag}"
                    : $"--{option.LongFlag}";

                Console.WriteLine($"  {flags,-20} {option.Description}");
            }
        }
    }
}