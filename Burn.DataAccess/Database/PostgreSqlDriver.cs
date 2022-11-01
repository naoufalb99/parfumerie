using System.Data;
using System.Transactions;
using Dapper;

namespace Burn.DataAccess.Database;

public class PostgreSqlDriver : IPostgreSqlDriver
{
    private readonly IBuildPostgreSqlConnection _buildPostgreSqlConnection;

    public PostgreSqlDriver(IBuildPostgreSqlConnection buildPostgreSqlConnection)
    {
        _buildPostgreSqlConnection = buildPostgreSqlConnection;
    }

    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
        await using var connection = _buildPostgreSqlConnection.CreateConnection();
        await connection.OpenAsync();
        return await connection.ExecuteAsync(sql, param);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
    {
        await using var connection = _buildPostgreSqlConnection.CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id")
    {
        await using var connection = _buildPostgreSqlConnection.CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<TFirst, TSecond, TReturn>(sql, map, param);
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id")
    {
        await using var connection = _buildPostgreSqlConnection.CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sql, map, param);
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object param = null)
    {
        await using var connection = _buildPostgreSqlConnection.CreateConnection();
        await connection.OpenAsync();
        return await connection.QuerySingleAsync<T>(sql, param);
    }
}

