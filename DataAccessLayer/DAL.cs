using System.Data;
using System.Reflection;
using Common;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace DataAccessLayer
{
    public class DAL
    {
        private IDbConnection? _connection = null;
        private IDbTransaction? _transaction = null;
        private readonly string? _connectionstring;
        private readonly int _commandtimeout;
        private int _transactioncounter;

        public DAL(IOptions<DatabaseOptions> options)
        {
            if (options.Value is null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            _connectionstring = options.Value.LocalConnectionString;
            _commandtimeout = options.Value.CommandTimeout;
            _connection = new SqlConnection(_connectionstring);
            _transactioncounter = 0;
        }
        public void Begin()
        {
            if (_transaction != null)
            {
                _connection = CreateConnection();
                _transaction = _connection.BeginTransaction(IsolationLevel.Snapshot);
            }
            else
            {
                _transactioncounter++;
            }
        }

        public void Commit()
        {
            try
            {
                if (_transactioncounter == 0)
                {
                    _transaction.Commit();
                    Dispose();
                }
                else
                {
                    _transactioncounter--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Rollback()
        {
            try
            {
                _transactioncounter = 0;
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            _transaction = null;
        }
        private IDbConnection? CreateConnection()
        {
            if (_connection?.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }
        public void CloseConnection()
        {
            if (_transaction != null)
            {
                return;
            }
            _connection.Close();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string storedProcedure)
        {
            try
            {
                _connection = CreateConnection();
                return await _connection.QueryAsync<T>(storedProcedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

        }

        public async Task<T?> GetByIdAsync<T>(string storedProcedure, object parameters)
        {
            try
            {
                _connection = CreateConnection();
                return await _connection.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

        }

        public async Task<int> AddAsync<T>(string storedProcedure, T parameters)
        {
            try
            {
                _connection = CreateConnection();
                return await _connection.ExecuteScalarAsync<int>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

        }

        public async Task<bool> UpdateAsync<T>(string storedProcedure, T parameters)
        {
            try
            {
                _connection = CreateConnection();
                var rowsAffected = await _connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task<bool> DeleteAsync<T>(string storedProcedure, T parameters)
        {
            try
            {
                _connection = CreateConnection();
                var rowsAffected = await _connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected > 0;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"SQL Error: {sqlEx.Message}", sqlEx);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
