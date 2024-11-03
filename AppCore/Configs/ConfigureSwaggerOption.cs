using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AppCore.Configs;

public class ConfigureSwaggerOption : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _versionProvider;

    public ConfigureSwaggerOption(IApiVersionDescriptionProvider versionProvider)
    {
        _versionProvider = versionProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered

        var versions = _versionProvider.ApiVersionDescriptions.GroupBy(z => z.ApiVersion)
            .Select(x => x.FirstOrDefault()).ToList();
        foreach (var description in versions)
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));

        var securityScheme = new OpenApiSecurityScheme
        {
            Description = "JWT Authorization using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        options.AddSecurityDefinition("Bearer", securityScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "Bearer" } }
        });
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"API - {description.GroupName}",
            Version = description.GroupName
        };

        if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

        return info;
    }
}