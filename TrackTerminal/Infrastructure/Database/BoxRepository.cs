using BoxSearch.Infrastructure.Database.Extensions;
using BoxSearch.Infrastructure.Database.Records;

using Microsoft.Data.SqlClient;

namespace BoxSearch.Infrastructure.Database;

internal class BoxRepository(string serverName)
{
    private readonly string _connectionString = @$"Server={serverName};Integrated Security=true;Encrypt=true;TrustServerCertificate=true";

    public async Task<SqlConnection> GetConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task TestConnectionAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        Console.WriteLine("Successfully connected to SQL Server \"VARXACTATESTDB\"");
        Console.WriteLine($"Server: {connection.DataSource}");
        Console.WriteLine($"Database: {connection.Database}");
    }

    public async Task<List<ContainerRecord>> SelectContainerHeaderAsync()
    {
        var connection = await GetConnectionAsync();

        var sql = @"USE Exactadb;
                    SELECT TOP 20 cntnr_id, cntnr_type, cntnr_name, created_date
                    FROM cntnr_header
                    ORDER BY created_date DESC;";

        using var command = new SqlCommand(sql, connection);

        var results = new List<ContainerRecord>();
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            results.Add(new ContainerRecord(
                reader.GetGuidOrNull(0),
                reader.GetDecimalOrNull(1),
                reader.GetStringOrNull(2),
                reader.GetDateTimeOrNull(3))
            );
        }

        return results;
    }
}