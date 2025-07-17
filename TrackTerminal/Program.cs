using BoxSearch.Commands;
using BoxSearch.Commands.Definitions;
using BoxSearch.Infrastructure.Database;

using LogParser;

namespace BoxSearch;

internal class Program
{
    //private readonly static LogReader _logReader;

    static Program()
    {
        //_logReader = new() { TimeZone = TimeZoneInfo.Local };
        //_logReader.WriteBack += Console.WriteLine;
    }

    public static async Task<int> Main(string[] args)
    {
        var registry = new CommandRegistry("boxsearch", "SanMar inventory box search tool");
        registry.Register(CreateProcessLogCommand());
        registry.Register(CreateTestDatabaseCommand());

        CommandExecutor executor = new(registry);
        return await executor.ExecuteAsync(args);
    }

    private static CommandDefinition CreateTestDatabaseCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("server", "s", "Provide server name"),
            OptionDefinition.Create("verbose", "v", "Enable verbose output")
        };

        return CommandDefinition.Create(name: "test-db", "Connect to database", options, async cmd =>
        {
            if (!cmd.Options.TryGetValue("server", out var optionValues) || optionValues.Count == 0)
            {
                Console.WriteLine("Error: must provide server name with --server");
                return 1;
            }
            else if (optionValues.Count >= 2)
            {
                Console.WriteLine("Error: must specify a single server name");
                return 1;
            }

            string serverName = optionValues[0];
            BoxRepository repo = new(serverName);
            await repo.TestConnectionAsync();

            return 0;
        });
    }

    private static CommandDefinition CreateProcessLogCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("verbose", "v", "Enable verbose output"),
            OptionDefinition.Create("input", "i", "Specify input log file")
        };

        return CommandDefinition.Create("process-log", "Process log file", options, async cmd =>
        {
            if (!cmd.Options.TryGetValue("i", out var optionValues) || optionValues.Count == 0)
            {
                Console.WriteLine("Error: must specify file name with -i");
                return 1;
            }
            else if (optionValues.Count >= 2)
            {
                Console.WriteLine("Error: must specify a single file name");
                return 1;
            }

            string fileName = optionValues[0];

            // TODO implement commands
            await Task.Run(() =>
            {
                //_logReader.CloseFile();
                //_logReader.OpenFile(fileName);
                //_logReader.ProcessFile();
            });

            return 0;
        });
    }
}