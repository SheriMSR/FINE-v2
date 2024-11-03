using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using AppCore.Configs;
using AppCore.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace AppCore.Middlewares;

public class HandleResponseMiddleware
{
    private readonly RequestDelegate _next;

    public HandleResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ignorePath = (Environment.GetEnvironmentVariable("IGNORE_LOG_PATH") ?? string.Empty)
            .Replace(" ", "")
            .Split(",")
            .ToList();
        if (ignorePath.Contains(context.Request.Path))
            await _next(context);

        var startTime = Stopwatch.GetTimestamp();
        try
        {
            await _next(context);
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            Log.Information(
                "Http Response Information | Method: {Method} | Path: {Path} | Status Code: {StatusCode} | QueryString: {QueryString}",
                context.Request.Method.ToUpper(),
                context.Request.Path,
                $"{context.Response.StatusCode} in {elapsedMilliseconds} ms",
                context.Request.QueryString
            );
        }
        catch (ApiException apiException)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            var resultException = new ApiException(
                apiException.Message,
                apiException.StatusCode, apiException.Data);
            await HandleExceptionAsync(
                context,
                resultException,
                resultException,
                elapsedMilliseconds
            );
        }
        catch (AntiforgeryValidationException antiForgeryValidationException)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            var resultException = new ApiException(
                MessageKey.CrossSiteRequestForgery,
                StatusCode.SERVER_ERROR);
            await HandleExceptionAsync(
                context,
                resultException,
                antiForgeryValidationException,
                elapsedMilliseconds
            );
        }
        catch (DbUpdateException exception)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            if (exception.InnerException?.Data["SqlState"]?.ToString() == "23505")
            {
                var table = exception.InnerException?.Data["TableName"]?.ToString() ?? string.Empty;
                var column = exception.InnerException?.Data["ConstraintName"]?.ToString() ?? string.Empty;
                var index = column.LastIndexOf("_", StringComparison.InvariantCultureIgnoreCase);
                index = index > 0 ? index + 1 : index;
                column = column[index..];
                await HandleExceptionAsync(
                    context,
                    new ApiException
                    (
                        $"Column {column} ({table}) has duplicated",
                        StatusCode.ALREADY_EXISTS
                    ),
                    exception,
                    elapsedMilliseconds
                );
            }
            else if (exception is DbUpdateConcurrencyException)
            {
                await HandleExceptionAsync(
                    context,
                    new ApiException
                    (
                        StatusCode.UNPROCESSABLE_ENTITY
                    ),
                    exception,
                    elapsedMilliseconds
                );
            }
            else
            {
                await HandleExceptionAsync(context, new ApiException(), exception, elapsedMilliseconds);
            }
        }
        catch (Exception exception)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTime, Stopwatch.GetTimestamp());
            await HandleExceptionAsync(context, new ApiException(), exception, elapsedMilliseconds);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, ApiException apiException, Exception exception,
        double elapsedMilliseconds)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)apiException.StatusCode;
        await context.Response.WriteAsJsonAsync(
            new
            {
                apiException.Message,
                statusCode = apiException.StatusCode,
                result = apiException.Data
            }, new JsonSerializerOptions
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }
        );


        IEnumerable<LogEventProperty> properties = new LogEventProperty[]
        {
            new("RequestMethod", new ScalarValue(context.Request.Method.ToUpper())),
            new("RequestPath", new ScalarValue(context.Request.Path)),
            new("StatusCode", new ScalarValue(context.Response.StatusCode)),
            new("Elapsed", new ScalarValue(elapsedMilliseconds)),
            new("Message",
                new ScalarValue(apiException.Message))
        };
        var messageTemplate = new MessageTemplateParser().Parse(
            "Http Response - Method: {RequestMethod} | Path: {RequestPath} | Responded {StatusCode} in {Elapsed:0.0000} ms | Message: {Message}");
        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Error,
            exception,
            messageTemplate,
            properties
        );
        Log.Write(logEvent);
    }

    private static double GetElapsedMilliseconds(long start, long stop)
    {
        return (stop - start) * 1000L / (double)Stopwatch.Frequency;
    }
}