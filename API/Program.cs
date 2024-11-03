using System.Reflection;
using AppCore.Configs;
using AppCore.Extensions;
using Data.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
//

// builder.Services.AddDbContext<FWContext>(options =>
// {
//     var connectString = EnvironmentExtension.GetAppConnectionString();
//     options.UseNpgsql(connectString,
//         b =>
//         {
//             b.MigrationsAssembly("API.Main");
//             b.CommandTimeout(1200);
//         }
//     );
//     options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//     options.EnableDetailedErrors();
// }, ServiceLifetime.Transient);

//

builder.Services.AddHttpClient();
builder.Services.AddControllers();
// builder.Services.AddScoped<FWUnitOfWork>();
builder.Services.AddConfig([
    "AppCore",
    "MainData",
    Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty
], [
    nameof(IApiVersionDescriptionProvider),
    "UnitOfWork",
    "IUnitOfWork"
]);

//
builder.Host.UseSerilog();
//

var app = builder.Build();
app.UseConfig();

//
app.MapControllers();
app.UseMiddleware<AuthMiddleware>();
app.Run();