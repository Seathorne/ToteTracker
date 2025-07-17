using BoxSearch.Commands.Definitions;

using System.Collections.Frozen;

namespace BoxSearch.Commands;

internal static class CommandLineParser
{
    public static ParsedCommand? Parse(string[] args)
    {
        if (args.Length == 0)
            return null; // No command provided

        string? commandName = null;
        Dictionary<string, List<string>> options = [];
        List<string> arguments = [];

        bool isNamed = false;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].StartsWith("--"))   // e.g., --verbose; --tote U0847
            {
                string key = args[i][2..];
                List<string> values = GetOptionValues(ref i);
                options[key] = values;
            }
            else if (args[i].StartsWith('-'))   // e.g., -v; -t U0847
            {
                string key = args[i][1..];
                List<string> values = GetOptionValues(ref i);
                options[key] = values;
            }
            else if (isNamed)
            {
                arguments.Add(args[i]);
            }
            else commandName = args[0];
        }

        return new ParsedCommand(
            commandName,
            arguments.AsReadOnly(),
            options.ToFrozenDictionary(x => x.Key, x => (IReadOnlyList<string>)x.Value.AsReadOnly())
        );

        List<string> GetOptionValues(ref int index)
        {
            List<string> values = [];
            while (index + 1 < args.Length && !args[index + 1].StartsWith('-'))
            {
                values.Add(args[++index]);
            }

            return values.Count >= 1 ? values : ["true"];
        }
    }
}