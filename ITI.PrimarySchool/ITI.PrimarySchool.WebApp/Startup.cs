using System;
using ITI.PrimarySchool.WebApp.Middlewares;
using ITI.PrimarySchool.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ITI.PrimarySchool.WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<WinOrLoseService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<WinOrLoseMiddleware>();

            app.Use(async (context, next) =>
            {
                var request = context.Request;
                var queryCollection = request.Query;
                bool callNext = true;
                foreach (var arg in queryCollection)
                {
                    callNext = false;
                    var values = arg.Value;
                    foreach (var v in values)
                    {
                        string s = string.Format("Key: {0} - Value: {1}", arg.Key, v);
                        await context.Response.WriteAsync(s);
                        await context.Response.WriteAsync(Environment.NewLine);
                    }
                }

                if (callNext) await next();

                await context.Response.WriteAsync("in'tech 2019");
            });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
