using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Services;
using Javeriana.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace TareasAPI
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

            services.AddSingleton<ITareasService,TareasServices>();

            services.AddSwaggerDocument( config => 
            {
                config.PostProcess = document => 
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Tareas API";
                    document.Info.Description = "PICA Módulo .Net";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "PICA",
                        Email = "pica@javeriana.edu.co"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    };
                };
            });

            services.AddHealthChecks().AddCheck("memoria", new ApiHealthCheck());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                endpoints.MapHealthChecks("/health", new HealthCheckOptions {
                    ResponseWriter = async (context, report) => 
                    {
                        context.Response.ContentType = "application/json";

                        var response = new HealthCheckResponse 
                        {
                            Status = report.Status.ToString(),
                            Checks = report.Entries.Select (x => new CheckInfo
                            {
                                Status = x.Value.Status.ToString(),
                                Component = x.Key,
                                Description = x.Value.Description
                            }),
                            Duration = report.TotalDuration
                        };
                        var json = JsonConvert.SerializeObject(response);
                        byte[] bytes = Encoding.ASCII.GetBytes(json);
                        await context.Response.Body.WriteAsync(bytes);
                    }
                });
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
