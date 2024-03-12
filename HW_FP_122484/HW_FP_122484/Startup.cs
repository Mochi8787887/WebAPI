using HW_FP.Data.TrainDB122484.Data;
using HW_FP_122484.Models;
using HW_FP_122484.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW_FP_122484
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // DI注入model，內容設定環境變數的參數
            services.Configure<Settings>(Configuration);

            // DI注入ClaimAccessor
            services.AddSingleton<ClaimAccessor>();

            services.AddScoped<AuthoriaztionFilter>();

            // DI注入IHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEntityFrameworkSqlServer().AddDbContext<TrainDB122484Context>(options =>
            {
                options
                .UseSqlServer("Data Source=10.11.37.148;Initial Catalog=TrainDB122484;User ID=122484;Password=122484")
                .EnableSensitiveDataLogging(true);
            });

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();

            //    app.UseMiddleware<Middleware>();
            //}

            app.UseDeveloperExceptionPage();

            app.UseMiddleware<Middleware>();
            app.UseMvc();
        }
    }
}