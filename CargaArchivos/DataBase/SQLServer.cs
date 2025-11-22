using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace CargaArchivos.DataBase
{
    public class SQLServer
    {
        private readonly string _connectionString;
        public SQLServer(string connectionString)
        {
           _connectionString = connectionString;
        }
        public SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new(_connectionString);
            return sqlConnection;
        }
        public async Task<SqlTransaction> GetTransaction(SqlConnection sqlConnection)
        {
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            if (sqlConnection.State != ConnectionState.Open)
            {
                await sqlConnection.OpenAsync();
            }
            var sqlTransaction = await sqlConnection.BeginTransactionAsync();
            return (SqlTransaction)sqlTransaction;
        }
        public async Task<T> ScalarAsync<T>(string query, SqlParameter[] parameters = null)
        {
            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();
            object result = await sqlCommand.ExecuteScalarAsync();

            return result is T value ? value : default;

        }
        public async Task<T> ScalarAsync<T>(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string query, SqlParameter[] parameters = null)
        {
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            ArgumentNullException.ThrowIfNull(sqlTransaction, nameof(sqlTransaction));

            await using SqlCommand sqlCommand = new(query, sqlConnection, sqlTransaction)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            object result = await sqlCommand.ExecuteScalarAsync();

            return result is T value ? value : default;

        }
        public async Task NonQueryAsync(string query, SqlParameter[] parameters = null)
        {

            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();

            int registrosAfectados = await sqlCommand.ExecuteNonQueryAsync();

            if (registrosAfectados == 0)
            {
                throw new Exception("No se afectaron registros.");
            }
        }
        public async Task NonQueryAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string query, SqlParameter[] parameters = null)
        {
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            ArgumentNullException.ThrowIfNull(sqlTransaction, nameof(sqlTransaction));

            await using SqlCommand sqlCommand = new(query, sqlConnection, sqlTransaction)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }


            int registrosAfectados = await sqlCommand.ExecuteNonQueryAsync();

            if (registrosAfectados == 0)
            {
                throw new Exception("No se afectaron registros.");
            }
        }
        public async Task<T> ReaderAsync<T>(string query, SqlParameter[] parameters = null)
            where T : class, new()
        {
            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();
            await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (await reader.ReadAsync())
            {
                T item = new();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);

                    var property = props.FirstOrDefault(p =>
                        string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                    if (property != null && !reader.IsDBNull(i))
                    {
                        object value = reader.GetValue(i);
                        object converted = Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        property.SetValue(item, converted);
                    }
                }

                return item;
            }

            return null;
        }
        public async Task<T> ReaderAsync<T>(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string query, SqlParameter[] parameters = null)
            where T : class, new()
        {
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            ArgumentNullException.ThrowIfNull(sqlTransaction, nameof(sqlTransaction));

            await using SqlCommand sqlCommand = new(query, sqlConnection, sqlTransaction)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (await reader.ReadAsync())
            {
                T item = new();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);

                    var property = props.FirstOrDefault(p =>
                        string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                    if (property != null && !reader.IsDBNull(i))
                    {
                        object value = reader.GetValue(i);
                        object converted = Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        property.SetValue(item, converted);
                    }
                }

                return item;
            }

            return null;
        }
        public async Task<List<T>> ReaderListAsync<T>(string query, SqlParameter[] parameters = null)
            where T : class, new()
        {

            await using SqlConnection sqlConnection = GetConnection();
            await using SqlCommand sqlCommand = new(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await sqlConnection.OpenAsync();
            await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var lista = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (await reader.ReadAsync())
            {
                T item = new();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);

                    var property = props.FirstOrDefault(p =>
                        string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                    if (property != null && !reader.IsDBNull(i))
                    {
                        object value = reader.GetValue(i);
                        object converted = Convert.
                            ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        property.SetValue(item, converted);
                    }
                }

                lista.Add(item);
            }

            return lista;
        }
        public async Task<List<T>> ReaderListAsync<T>(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string query, SqlParameter[] parameters = null)
            where T : class, new()
        {
            ArgumentNullException.ThrowIfNull(sqlConnection, nameof(sqlConnection));
            ArgumentNullException.ThrowIfNull(sqlTransaction, nameof(sqlTransaction));

            await using SqlCommand sqlCommand = new(query, sqlConnection, sqlTransaction)
            {
                CommandType = CommandType.Text
            };

            if (parameters is not null)
            {
                sqlCommand.Parameters.AddRange(parameters);
            }

            await using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var lista = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (await reader.ReadAsync())
            {
                T item = new();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);

                    var property = props.FirstOrDefault(p =>
                        string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                    if (property != null && !reader.IsDBNull(i))
                    {
                        object value = reader.GetValue(i);
                        object converted = Convert.
                            ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        property.SetValue(item, converted);
                    }
                }

                lista.Add(item);
            }

            return lista;
        }
    }
}
