using System;
using System.Threading.Tasks;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Http;

namespace ITI.PrimarySchool.WebApp.Middlewares
{
    public class WinOrLoseMiddleware
    {
        readonly RequestDelegate _next;

        public WinOrLoseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, WinOrLoseService winOrLoseService)
        {
            if (winOrLoseService.WinOrLose())
            {
                await context.Response.WriteAsync("You lose!");
            }
            else
            {
                await context.Response.WriteAsync("You win!");
                await context.Response.WriteAsync(Environment.NewLine);
                await _next(context);
            }
        }
    }
}