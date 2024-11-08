using AppCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Configs;

public static class AddSwaggerServiceCollectionExtensions
{
    public static void AddConfigSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.OperationFilter<SnakeCasingParameterOperationFilter>();
            config.DocumentFilter<UrlRenameDocumentFilter>();
            config.DescribeAllParametersInCamelCase();
            config.EnableAnnotations();
        });
        services.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
        services.ConfigureOptions<ConfigureSwaggerOption>();
    }

    public static IApplicationBuilder UseConfigSwagger(this IApplicationBuilder app)
    {
        var descriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            descriptionProvider.ApiVersionDescriptions.GroupBy(x => x.ApiVersion)
                .Select(x => x.FirstOrDefault()).ToList()
                .ForEach(description =>
                {
                    c.SwaggerEndpoint(
                        $"{EnvironmentExtension.GetPath()}/swagger/{description.GroupName}/swagger.json",
                        $"{description.GroupName.ToUpperInvariant()}");
                });
        });
        return app;
    }
}