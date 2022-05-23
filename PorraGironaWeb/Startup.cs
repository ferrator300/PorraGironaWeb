using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PorraGironaWeb
{
    public class Startup
    {
        public static string ConnectionStrings { get; private set; } = "server=localhost;database=porragirona;uid=root";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //Per permetre sessions
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Carrega la configuració de la connexió a la base de dades des del fitxer appsettings.json
            ConnectionStrings = Configuration["ConnectionStrings:PorraGironaWebContextConnection"];

            services.AddControllersWithViews();
            services.AddMvc(); //Afegit per funcionalitat Identitat
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); //Afegit per funcionalitat Indetitat
            app.UseSession(); //Per permetre les sessions
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Puntuacions}/{action=Index}/{id?}");
                    pattern: "{controller=Login}/{action=Index}/{id?}");

                endpoints.MapRazorPages(); //Afegit per funcionalitat Identitat
            });
        }
    }
}
