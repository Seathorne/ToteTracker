namespace BoxSearch.Commands.Definitions;

internal record ParsedCommand(
    string? CommandName,
    IReadOnlyList<string> Arguments,
    IReadOnlyDictionary<string, IReadOnlyList<string>> Options
);