using System.Net.Http;
using ApplicationCore.Interfaces;
using Auth0.AspNetCore.Authentication;
using HealthChecks.UI.Client;
using Infrastructure.WebServices;
using Javeriana.Api.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TareasWeb.Extensions;

namespace TareasWeb
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
            
            services.AddSingleton<HttpClient>();
            services.AddSingleton<IRestClient<Tarea>, JSONRestClient<Tarea>>();

            services.AddHealthChecks();

            services.ConfigureSameSiteNoneCookies();
            services.AddAuth0WebAppAuthentication(options => {
                options.Domain = Configuration["Auth0:Domain"];
                options.ClientId = Configuration["AUTH0_CLIENT_ID"]; //Variables de entorno
                options.ClientSecret = Configuration["AUTH0_CLIENT_SECRET"]; //Variables de entorno
            }).WithAccessToken(options => {
                options.Audience = Configuration["Auth0:Audience"];
                options.UseRefreshTokens = true;
                options.Events = new Auth0WebAppWithAccessTokenEvents
                {
                    OnMissingRefreshToken = async (context) =>
                    {
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder().WithRedirectUri("/").Build();
                        await context.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
                    }
                };
            });

            services.AddControllersWithViews();
        }

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Descomentar esta l�nea si usa Linux o Mac OS
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
