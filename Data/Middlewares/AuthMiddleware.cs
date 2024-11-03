using AppCore.Extensions;
using AppCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Data.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        FineContext context)
    {
        var endPoint = httpContext.GetEndpoint();
        if (endPoint != null)
        {
            var allowAnonymous = endPoint.Metadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                await _next(httpContext);
                return;
            }
        }

        var accessToken = httpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ")[^1];
        if (string.IsNullOrEmpty(accessToken))
        {
            await _next(httpContext);
            return;
        }

        var tokenClaim = JwtExtensions.ValidateAccessToken(accessToken).ToList();
        var accountIdString = tokenClaim.Find(x => x.Type == AppClaimTypes.AccountId)?.Value;
        Guid.TryParse(accountIdString, out var accountId);
        if (accountId != Guid.Empty)
        {
            // var now = DatetimeExtension.UtcNow();
            // var account = await context.Accounts.FirstOrDefaultAsync(a =>
            //     !a.IsDeleted &&
            //     a.Id == accountId
            // );
            // var token = await context.Tokens
            //     .FirstOrDefaultAsync(t =>
            //         !t.IsDeleted &&
            //         t.AccessToken == accessToken &&
            //         t.AccountId == accountId &&
            //         t.AccessExpiredAt > now &&
            //         t.Status == TokenStatus.Active
            //     );
            //
            // if (account == null || token == null)
            // {
            //     await _next(httpContext);
            //     return;
            // }
            //
            // httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            // {
            //     new Claim(AppClaimTypes.AccountId, account.Id.ToString()),
            //     new Claim(AppClaimTypes.Role, account.Role.ToString()),
            //     // new Claim(AppClaimTypes.Status, account.Status.ToString()),
            // }));
            await _next(httpContext);
            return;
        }

        await _next(httpContext);
    }
}