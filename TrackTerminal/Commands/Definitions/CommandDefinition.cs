namespace BoxSearch.Commands.Definitions;

internal record CommandDefinition(
    string Name,
    string Description,
    IReadOnlyList<OptionDefinition> Options,
    Func<ParsedCommand, Task<int>> Handler)
{
    public static CommandDefinition Create(string name, string description, Func<ParsedCommand, Task<int>> handler)
        => new(name, description, [], handler);

    public static CommandDefinition Create(string name, string description, List<OptionDefinition> options, Func<ParsedCommand, Task<int>> handler)
        => new(name, description, options.AsReadOnly(), handler);
}