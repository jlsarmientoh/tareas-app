using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Services;
using Javeriana.Api.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Javeriana.Core.Interfaces;
using TareasAPI.Repositories;
using Javeriana.Core.Contexts;
using Microsoft.EntityFrameworkCore;
using Javeriana.Core.Interfaces.Messaging;
using Infrastructure.Messaging;
using Javeriana.Core.Tareas.Entities;
using Infrastructure.Interface.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TareasAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<TareasContext>(options => options.UseInMemoryDatabase("Tareas"));
            //services.AddDbContext<TareasContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbTareas")));
            ConfigureServices(services);
        }

        public void ConfigureDockerServices(IServiceCollection services)
        {
            services.AddDbContext<TareasContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbTareas")));
            services.AddLogging(logginBuilder => {
                logginBuilder.AddSeq(Configuration.GetSection("Seq"));
            });
            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<TareasContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DbTareas")));
            services.AddLogging(logginBuilder => {
                logginBuilder.AddSeq(Configuration.GetSection("Seq"));
            });
            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IMessagingFactory, RabbitMQFactoryCreator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAsyncRepository<Tarea>, TareasRespository>();
            
            services.AddScoped<IPublisher, TareasPublisher>();
            services.AddHostedService<TareasConsumer>();

            services.AddScoped<ITareasService,TareasServices>();

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

            services.AddHealthChecks()
                .AddCheck("memoria", new ApiHealthCheck())
                .AddSqlServer(
                    connectionString : Configuration.GetConnectionString("DbTareas"),
                    healthQuery : "SELECT 1;",
                    name : "sql",
                    failureStatus: HealthStatus.Degraded
                );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Auth0:Domain"];
                    options.Audience = Configuration["Auth0:Audience"];
                });
            
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Descomentar esta línea si usa Linux o Mac OS
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
