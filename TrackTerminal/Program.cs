using BoxSearch.Commands;
using BoxSearch.Commands.Definitions;
using BoxSearch.Infrastructure.Database;

using LogParser;

namespace BoxSearch;

internal class Program
{
    private static BoxRepository? _boxRepository;
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
        registry.Register(CreateConnectToDatabaseCommand());
        registry.Register(CreateSelectTableCommand());

        registry.Register(CommandDefinition.Create("exit", "Exit the application", async _ =>
        {
            Environment.Exit(0);
            return 0;
        }));

        CommandExecutor executor = new(registry);

        if (args.Length > 0)
        {
            return await executor.ExecuteAsync(args);
        }

        Console.WriteLine("BoxSearch Interactive Mode. Type 'help' for commands, 'exit' to quit.");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            var inputArgs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            await executor.ExecuteAsync(inputArgs);
        }
    }

    private static CommandDefinition CreateConnectToDatabaseCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("server", "s", "Server name"),
        };

        return CommandDefinition.Create(name: "db-connect", "Connect to database", options, async cmd =>
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
            _boxRepository = new(serverName);
            await _boxRepository.TestConnectionAsync();

            return 0;
        });
    }

    private static CommandDefinition CreateSelectTableCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("table", "t", "Table name")
        };

        return CommandDefinition.Create(name: "db-select", "Select table from database", options, async cmd =>
        {
            if (_boxRepository is null)
            {
                Console.WriteLine("Error: must use db-connect before using this command");
                return 1;
            }
            
            if (!cmd.Options.TryGetValue("table", out var optionValues) || optionValues.Count == 0)
            {
                Console.WriteLine("Error: must provide table name with --table");
                return 1;
            }
            else if (optionValues.Count >= 2)
            {
                Console.WriteLine("Error: must specify a single table name");
                return 1;
            }

            string tableName = optionValues[0];
            
            switch (tableName)
            {
                case "cntnr_header":
                    var results = await _boxRepository.SelectContainerHeaderAsync();
                    foreach (var container in results)
                    {
                        Console.WriteLine(container);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return 0;
        });
    }

    private static CommandDefinition CreateProcessLogCommand()
    {
        var options = new List<OptionDefinition>
        {
            OptionDefinition.Create("verbose", "v", "Enable verbose output"),
            OptionDefinition.Create("input", "i", "Input log file")
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