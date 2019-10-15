using System;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITI.PrimarySchool.WebApp
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GatewayOptions>(options =>
            {
                options.ConnectionString = _configuration["ConnectionStrings:PrimarySchool"];
            });

            services.AddMvc();
            services.AddSingleton<ClassGateway>();
            services.AddSingleton<TeacherGateway>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(b =>
            {
                b.AllowAnyHeader()
                 .AllowCredentials()
                 .AllowAnyMethod()
                 .WithOrigins(_configuration["Spa:Url"]);
            });
            app.UseMvcWithDefaultRoute();
        }
    }
}
