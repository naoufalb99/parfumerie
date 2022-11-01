using System;
using System.Data;
using Burn.DataAccess.Database;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Microsoft.Extensions.Options;

namespace Burn.DataAccess;

public class PostgreSqlConnectionFactory : IBuildPostgreSqlConnection
{
    private readonly string _connectionString;

    public PostgreSqlConnectionFactory(PostgreSqlConfiguration config)
    {
        _connectionString = new NpgsqlConnectionStringBuilder()
        {
            Host = config.Host,
            Port = config.Port,
            Database = config.Database,
            Username = config.Username,
            Password = config.Password
        }.ConnectionString;
    }

    public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
