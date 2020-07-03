using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Infrastructure.DI;
using Application.Transfer.Commands;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace BenkiKuu
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
            services.AddControllers();
            services.AddMediatR(typeof(Startup));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Account Transfer gone DigiTALL",
                    Version = "v1"
                });
            });

            Loader.Register(services, Configuration,
               (Type a) => services.AddMediatR(a)
           );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           // SetLocation(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.InjectStylesheet("change logo");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account v1");
            });
        }

     /*   private void SetLocation(IApplicationBuilder app)
        {
            var local = Configuration["LocalSite"];
            RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo> { new CultureInfo(local) },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo(local) },
                DefaultRequestCulture = new RequestCulture(local)
            };
            app.UseRequestLocalization(localizationOptions);
        }
        */
    }
}
