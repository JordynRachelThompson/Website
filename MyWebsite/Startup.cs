using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioWebsite.Data;
using PortfolioWebsite.Data.Interfaces;
using PortfolioWebsite.Data.Repositories;
using PortfolioWebsite.Models;
using PortfolioWebsite.Services;
using Newtonsoft.Json.Serialization;

namespace PortfolioWebsite
{
    public class Startup
    {
        public IHostingEnvironment environment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddJsonFile("azurekeyvault.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            // add this file name to your .gitignore file
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            //builder.AddEnvironmentVariables();

            var config = builder.Build();

            builder.AddAzureKeyVault(
                $"https://{config["azureKeyVault:vault"]}.vault.azure.net/",
                config["azureKeyVault:clientId"],
                config["azureKeyVault:clientSecret"]
            );

            //Configuration = builder.Build();

            environment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (environment.IsDevelopment())
            {             
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PortfolioConnection")));

            }
            else if (environment.IsProduction())
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            }
            services.AddSingleton(Configuration);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<StorageOptions>(Configuration.GetSection("GoogleReCaptcha"));
            services.Configure<StorageOptions>(Configuration.GetSection("EmailConfiguration"));
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IBudgetItemsRepository, BudgetItemsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddMvc()
            .AddJsonOptions(options =>
            options.SerializerSettings.ContractResolver = new DefaultContractResolver());            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            app.UseBrowserLink();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
