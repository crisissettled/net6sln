using Microsoft.Extensions.Options;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.Common;

namespace identity.classes.Postgresql
{
    public class DbPostgresql : IDbPostgresql, IDisposable
    {
        private readonly PostgresqlSettings _pgSettings;
        private NpgsqlConnection? _connection = null;
        private NpgsqlCommand? _cmd = null;
        private NpgsqlDataReader? _dr = null;
  
        public DbPostgresql(IOptions<PostgresqlSettings> optionsPgSettings)
        {
            _pgSettings = optionsPgSettings.Value;
        }

        public async Task Prepare(string sql = "")
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_pgSettings.ConnectionString);
                await _connection.OpenAsync();
            }

            if(_cmd == null) { 
                _cmd = _connection.CreateCommand();
                if(!string.IsNullOrWhiteSpace(sql)) _cmd.CommandText = sql;
            }         
        }

        public void SetCommandSql(string sql, bool clearParameter = false)
        {
            if (_cmd != null)
            {
                if (clearParameter) _cmd.Parameters.Clear();
                _cmd.CommandText = sql;
            }
        }

        public void AddParameter(string ParaName,NpgsqlDbType npgDbType,object value )
        {
            if(_cmd !=null) _cmd.Parameters.Add(ParaName, npgDbType).Value = value; 
        }

        public async Task<NpgsqlDataReader> ExecuteReaderAsync()
        {          
            _dr = await _cmd!.ExecuteReaderAsync();
            return _dr;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _dr?.Close();
                _cmd?.Dispose();
                _connection?.Close();
            }
        }

    }
}
