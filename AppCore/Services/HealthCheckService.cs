using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace AppCore.Services;

public class DatabaseCheck : IHealthCheck
{
    private readonly string _connectionString;
    private readonly string _sql;
    private readonly string _name;
    private readonly bool _important;
    private readonly string _dependencyType;


    public DatabaseCheck(string connectionString, string sql, string name, bool important,
        string dependencyType)
    {
        _connectionString = connectionString;
        _sql = sql;
        _name = name;
        _important = important;
        _dependencyType = dependencyType;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var data = new Dictionary<string, object>
        {
            { "Name", _name },
            { "Important", _important },
            { "Type", _dependencyType }
        };
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            await using (var command = connection.CreateCommand())
            {
                command.CommandText = _sql;
                _ = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            }

            return HealthCheckResult.Healthy(null, data);
        }
        catch (Exception exception)
        {
            return _important
                ? HealthCheckResult.Unhealthy(exception.Message, exception, data)
                : HealthCheckResult.Degraded(exception.Message, exception, data);
        }
    }
}