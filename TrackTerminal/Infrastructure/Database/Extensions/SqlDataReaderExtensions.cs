using Microsoft.Data.SqlClient;

namespace BoxSearch.Infrastructure.Database.Extensions;

internal static class SqlDataReaderExtensions
{
    public static Guid? GetGuidOrNull(this SqlDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetGuid(ordinal);

    public static decimal? GetDecimalOrNull(this SqlDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);

    public static string? GetStringOrNull(this SqlDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);

    public static DateTime? GetDateTimeOrNull(this SqlDataReader reader, int ordinal)
        => reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
}