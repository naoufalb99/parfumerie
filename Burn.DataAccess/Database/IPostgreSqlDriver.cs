using System.Data;

namespace Burn.DataAccess.Database;

public interface IPostgreSqlDriver
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);

    Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id");

    Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id");

    Task<T> QuerySingleAsync<T>(string sql, object param = null);

    Task<int> ExecuteAsync(string sql, object param = null);
}

