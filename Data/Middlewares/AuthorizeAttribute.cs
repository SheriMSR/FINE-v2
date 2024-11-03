// using AppCore.Extensions;
// using AppCore.Models;
// using MainData.Entities;
// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace Data.Middlewares
// {
//     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//     public class AuthorizeAttribute : Attribute, IAuthorizationFilter
//     {
//         private readonly bool _allowAllRole;
//         private readonly AccountRole[] _roles;
//
//         public AuthorizeAttribute(AccountRole[] roles, bool allowAllRole = false)
//         {
//             _roles = roles.ToArray();
//             _allowAllRole = allowAllRole;
//         }
//
//         public void OnAuthorization(AuthorizationFilterContext context)
//         {
//             var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
//             if (allowAnonymous)
//                 return;
//
//             var user = context.HttpContext.User;
//             if (user.GetAccountId() == null || user.GetAccountId() == Guid.Empty)
//                 throw new ApiException(MessageKey.Unauthorized, StatusCode.UNAUTHORIZED);
//
//             var userRole = user.GetEnum<AccountRole>(AppClaimTypes.Role);
//             if (userRole == null)
//             {
//                 throw new ApiException(MessageKey.Unauthorized, StatusCode.UNAUTHORIZED);
//             }
//
//             // if (_roles.Any() && _roles.All(x => x != AccountRole.Admin))
//             // {
//             //     // if (!user.GetActive() && !_allowInactive)
//             //     //     throw new ApiException(MessageKey.Forbidden, StatusCode.FORBIDDEN);
//             //
//             //     var active = user.GetActive();
//             //     if (active && _allowInactive)
//             //         throw new ApiException(MessageKey.AccountNotActivated, StatusCode.NOT_ACTIVE);
//             // }
//
//             if (_allowAllRole) return;
//             if (_roles.Any() && !_roles.Contains(userRole.Value))
//                 throw new ApiException(MessageKey.Forbidden, StatusCode.FORBIDDEN);
//         }
//     }
//
//     [AttributeUsage(AttributeTargets.Method)]
//     public class AllowAnonymousAttribute : Attribute
//     {
//     }
// }