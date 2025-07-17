using BoxSearch.Commands;
using BoxSearch.Commands.Definitions;

namespace BoxSearch;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        var registry = new CommandRegistry("boxsearch", "SanMar inventory box search tool");
        registry.Register(CreatePortalCommand());

        CommandExecutor executor = new(registry);
        return await executor.ExecuteAsync(args);
    }

    static CommandDefinition CreatePortalCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("verbose", "v", "Enable verbose output")
        };

        return CommandDefinition.Create("portal", "Exacta online portal", options, async cmd =>
        {
            if (cmd.Arguments.Count == 0)
            {
                Console.WriteLine("Error: unimplemented.");
                return 1;
            }

            // TODO implement commands
            await Task.Run(() => { });
            return 0;
        });
    }
}