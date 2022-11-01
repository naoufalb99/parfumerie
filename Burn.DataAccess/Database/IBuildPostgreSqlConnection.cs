using System;
using Npgsql;

namespace Burn.DataAccess.Database;

public interface IBuildPostgreSqlConnection
{
    NpgsqlConnection CreateConnection();
}

