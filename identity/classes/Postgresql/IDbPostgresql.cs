using Npgsql;
using NpgsqlTypes;

namespace identity.classes.Postgresql
{
    public interface IDbPostgresql
    {
        public Task Prepare(string sql = "");
        public void SetCommandSql(string sql, bool clearParameter = false);
        public void AddParameter(string ParaName, NpgsqlDbType npgDbType, object value);
        public Task<NpgsqlDataReader> ExecuteReaderAsync();
    }
}
