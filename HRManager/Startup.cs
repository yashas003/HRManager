using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRManager.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HRManager
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EmployeeDbConnection")));

            services.AddMvc(op => op.EnableEndpointRouting = false)
                .AddXmlDataContractSerializerFormatters();

            services.AddScoped<IRepo, Repo>();
            services.AddSession(op => op.IdleTimeout = TimeSpan.FromMinutes(30));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(route => {
                route.MapRoute("default", "{controller=employee}/{action=login}");
            });
        }
    }
}
