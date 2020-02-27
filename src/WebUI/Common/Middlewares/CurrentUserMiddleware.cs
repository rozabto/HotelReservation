using System;
using System.Net;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Common;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace WebUI.Common.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ICurrentUserService currentUser, IUserManager userManager, IMemoryService memory)
        {
            try
            {
                if (httpContext?.User?.Identity == null || httpContext?.Connection?.RemoteIpAddress == null)
                    throw new Exception();

                currentUser.IsAuthenticated = httpContext.User.Identity.IsAuthenticated;

                if (currentUser.IsAuthenticated)
                {
                    var user = memory.GetValue<AppUser>(httpContext.User.Identity.Name);
                    if (user == null)
                    {
                        user = await userManager.GetUserByUsername(httpContext.User.Identity.Name)
                            ?? throw new UnauthorizedAccessException("Provided credentials are invalid");

                        memory.SetValue(httpContext.User.Identity.Name, user);
                    }
                    currentUser.User = user;
                }
                currentUser.Ip = httpContext.Connection.RemoteIpAddress.ToString();
            }
            catch (UnauthorizedAccessException ex)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsync(ex.Message);
                return;
            }
            await _next(httpContext);
        }
    }

    public static class CurrentUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentUserMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CurrentUserMiddleware>();
    }
}