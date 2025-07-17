using BoxSearch.Commands.Definitions;

using System.Collections.Frozen;

namespace BoxSearch.Commands;

internal static class CommandLineParser
{
    public static ParsedCommand? Parse(string[] args)
    {
        if (args.Length == 0)
            return null; // No command provided

        string commandName = args[0];
        Dictionary<string, List<string>> options = [];
        List<string> arguments = [];

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i].StartsWith("--")) // -- is a flag argument
            {
                string key = args[i][2..];
                options[key] = ["true"];
            }
            else if (args[i].StartsWith('-')) // - is an argument that may be followed by at least 1 value
            {
                string key = args[i][1..];
                List<string> values = GetOptionValues(ref i);
                options[key] = values;
            }
            else arguments.Add(args[i]);
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