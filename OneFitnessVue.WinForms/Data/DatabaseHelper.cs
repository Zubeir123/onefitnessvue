using System.Data;
using Microsoft.Data.SqlClient;

namespace FitnessTimeGym.WinForms.Data;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable ExecuteQuery(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? transaction = null)
    {
        var table = new DataTable();

        if (transaction != null)
        {
            using var command = BuildCommand(sql, parameters, transaction.Connection!, transaction);
            using var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            return table;
        }

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var standaloneCommand = BuildCommand(sql, parameters, connection, null);
        using var standaloneAdapter = new SqlDataAdapter(standaloneCommand);
        standaloneAdapter.Fill(table);
        return table;
    }

    public int ExecuteNonQuery(string sql, IEnumerable<SqlParameter>? parameters = null, SqlTransaction? transaction = null)
    {
        if (transaction != null)
        {
            using var command = BuildCommand(sql, parameters, transaction.Connection!, transaction);
            return command.ExecuteNonQuery();
        }

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var standaloneCommand = BuildCommand(sql, parameters, connection, null);
        return standaloneCommand.ExecuteNonQuery();
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    private static SqlCommand BuildCommand(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        SqlConnection connection,
        SqlTransaction? transaction)
    {
        var command = new SqlCommand(sql, connection, transaction);
        if (parameters == null)
        {
            return command;
        }

        foreach (var parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }

        return command;
    }
}
