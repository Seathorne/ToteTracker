using BoxSearch.Commands;
using BoxSearch.Commands.Definitions;
using LogParser;

namespace BoxSearch;

internal class Program
{
    private readonly static LogReader _logReader;

    static Program()
    {
        _logReader = new() { TimeZone = TimeZoneInfo.Local };
        _logReader.WriteBack += Console.WriteLine;
    }

    public static async Task<int> Main(string[] args)
    {
        var registry = new CommandRegistry("boxsearch", "SanMar inventory box search tool");
        registry.Register(CreateProcessLogCommand());

        CommandExecutor executor = new(registry);
        return await executor.ExecuteAsync(args);
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
                Console.WriteLine("Error: must specify a single file name with -i");
                return 1;
            }

            string fileName = optionValues[0];

            // TODO implement commands
            await Task.Run(() =>
            {
                _logReader.CloseFile();
                _logReader.OpenFile(fileName);
                _logReader.ProcessFile();
            });

            return 0;
        });
    }
}