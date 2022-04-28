using System;
using System.Text;
using System.Text.Json.Serialization;
using AspNetCore.Authentication.Basic;
using GitMonitor.Configurations;
using GitMonitor.Hubs;
using GitMonitor.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GitMonitor
{
    /// <summary>
    /// Web builder startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Startup method called to configure the services of the application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "Frontend/build";
            });

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentFrontend", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000")
                        .AllowCredentials();
                });
            });

            services.AddAuthentication(BasicDefaults.AuthenticationScheme)
                .AddBasic<BasicUserValidationService>(options => options.Realm = "GitMonitor");

            services.AddSwaggerGen();

            services.AddDependencies(Configuration);
        }

        /// <summary>
        /// Startup method called to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application request pipeline builder.</param>
        /// <param name="env">The application environment information.</param>
        /// <param name="applicationOptions">The application configuration to set the authentication middleware.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ApplicationOptions> applicationOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseCors("DevelopmentFrontend");
            }

            app.UseRouting();

            if (applicationOptions.Value.Username == null)
            {
                app.Use((context, next) =>
                {
                    context.Request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("anonymous:anonymous"));
                    return next();
                });
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<RepositoryChangesHub>("/hubs/changes");
            });

            app.MapWhen(x => !(x.Request.Path.Value?.StartsWith("/api") ?? false), builder =>
            {
                builder.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "Frontend";

                    if (env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            });
        }
    }
}
