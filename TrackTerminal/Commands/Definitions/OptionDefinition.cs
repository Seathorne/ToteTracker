namespace BoxSearch.Commands.Definitions;

internal record OptionDefinition(
   string LongFlag,
   string? ShortFlag,
   string Description,
   int RequiredValues,
   string? DefaultValue = null)
{
    public static OptionDefinition Create(string longFlag, string description, int requiredValues = 0)
        => new(longFlag, null, description, requiredValues, null);

    public static OptionDefinition Create(string longFlag, string shortFlag, string description, int requiredValues = 0)
        => new(longFlag, shortFlag, description, requiredValues, null);
}