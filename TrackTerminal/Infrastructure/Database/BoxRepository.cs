using Microsoft.Data.SqlClient;

namespace BoxSearch.Infrastructure.Database;

internal class BoxRepository(string serverName)
{
    private readonly string _connectionString = @$"Server={serverName};Integrated Security=true;Encrypt=true;TrustServerCertificate=true";

    //public async Task<SqlConnection> GetConnectionAsync()
    //{
    //    var connection = new SqlConnection(_connectionString);
    //    await connection.OpenAsync();
    //    return connection;
    //}

    public async Task TestConnectionAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        Console.WriteLine("Successfully connected to SQL Server \"VARXACTATESTDB\"");
        Console.WriteLine($"Server: {connection.DataSource}");
        Console.WriteLine($"Database: {connection.Database}");
    }
}