using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AppCore.Models;

public class CheckDatabaseOption
{
    public string ConnectionString { get; set; }
    public bool Important { get; set; } = false;
    public DependencyType DependencyType { get; set; } = DependencyType.Internal;
    public string HealthQuery { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public HealthStatus? FailureStatus { get; set; } = default;
    public IEnumerable<string> Tags { get; set; } = default;

    public TimeSpan? Timeout { get; set; } = default;
    public Func<IServiceProvider, string> ConnectionStringFactory => _ => ConnectionString;
}

public class CheckUriOption
{
    public Uri Uri { get; set; }
    public bool Important { get; set; } = false;
    public DependencyType DependencyType { get; set; } = DependencyType.Internal;
    public string Name { get; set; } = string.Empty;
    public HealthStatus? FailureStatus { get; set; } = default;
    public IEnumerable<string> Tags { get; set; } = default;
    public TimeSpan? Timeout { get; set; } = default;
    public bool IsHealthCheck { get; set; } = false;
    public string HealthUri { get; set; } = "health";
}

public class HealthCheckResponse
{
    public string Name { get; set; }
    public int Status { get; set; }
    public string Msg { get; set; }
    public string Description => GetDescription();
    public IEnumerable<Dependency> Dependencies { get; set; }

    private string GetDescription()
    {
        var descriptions = Dependencies.Where(x => x.Status != 200).Select(z => $"{z.Name} - {z.Description}")
            .Distinct().ToList();
        return string.Join(", ", descriptions);
    }
}

public class Dependency
{
    public string Name { get; set; }
    public int Status { get; set; }
    public string Msg { get; set; }
    public string Description { get; set; }
    public object Important { get; set; }
    public string ServiceType { get; set; }
    [JsonIgnore] public DateTime DateTime { get; set; }
    public string DateTimeFormat { get; set; }
    public string TimeCheck => DateTime.ToString(DateTimeFormat);
    public long TimeStamp => new DateTimeOffset(DateTime).ToUnixTimeSeconds();
    [JsonIgnore] public TimeSpan Speed { get; set; }
    public string CountTimeCheck => $"{Speed.TotalSeconds} seconds";
}

public enum DependencyType
{
    Internal,
    External
}