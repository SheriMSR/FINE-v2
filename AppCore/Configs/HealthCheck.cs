using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppCore.Models;
using AppCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace AppCore.Configs;

public static class SqlServerHealthCheck
{
    private const string HealthQuery = "SELECT 1;";
    private const string Name = "sqlserver";

    public static IHealthChecksBuilder AddCheckDb(this IHealthChecksBuilder builder,
        CheckDatabaseOption checkDatabaseOption)
    {
        if (string.IsNullOrEmpty(checkDatabaseOption.HealthQuery))
            checkDatabaseOption.HealthQuery = HealthQuery;

        if (string.IsNullOrEmpty(checkDatabaseOption.Name))
            checkDatabaseOption.Name = Name;

        return builder.Add(new HealthCheckRegistration(
            checkDatabaseOption.Name,
            sp => new DatabaseCheck(
                checkDatabaseOption.ConnectionStringFactory(sp),
                checkDatabaseOption.HealthQuery,
                checkDatabaseOption.Name,
                checkDatabaseOption.Important,
                checkDatabaseOption.DependencyType.ToString()),
            checkDatabaseOption.FailureStatus,
            checkDatabaseOption.Tags,
            checkDatabaseOption.Timeout));
    }
}

public static class HealthCheckBuilder
{
    public static IApplicationBuilder UseHealthCheck(this IApplicationBuilder app, PathString path,
        string? dateTimeFormat)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        var options = new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var healthCheckResponse = new HealthCheckResponse
                {
                    Status = report.Status == HealthStatus.Unhealthy ? 500 : 200,
                    Msg = report.Status.ToString(),
                    Name = Assembly.GetEntryAssembly()?.GetName().Name,
                    Dependencies = report.Entries.Select(x => new Dependency
                    {
                        Important = x.Value.Data.FirstOrDefault(z => z.Key == "Important").Value,
                        Status = x.Value.Status == HealthStatus.Unhealthy ? 500 : 200,
                        Msg = x.Value.Status.ToString(),
                        Description = x.Value.Description,
                        Name = x.Key,
                        Speed = x.Value.Duration,
                        ServiceType =
                            x.Value.Data.FirstOrDefault(z => z.Key == "Type").Value.ToString() ?? string.Empty,
                        DateTime = DateTime.Now,
                        DateTimeFormat = dateTimeFormat ?? "dd-MM-yyyy hh:mm:ss tt zz"
                    })
                };

                var response = JsonSerializer.Serialize(healthCheckResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                });
                context.Response.StatusCode = report.Status == HealthStatus.Unhealthy ? 500 : 200;
                await context.Response.WriteAsync(response);
            }
        };

        UseHealthChecksCore(app, path, new object[] { Options.Create(options) });
        return app;
    }

    private static void UseHealthChecksCore(IApplicationBuilder app, PathString path, object[] args)
    {
        bool Predicate(HttpContext c)
        {
            return !path.HasValue || (c.Request.Path.StartsWithSegments(path, out var remaining) &&
                                      string.IsNullOrEmpty(remaining));
        }

        app.MapWhen(Predicate, b => b.UseMiddleware<HealthCheckMiddleware>(args));
    }
}