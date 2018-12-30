using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using DbService;
using FtpService;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StorageService;

namespace RecordsManagmentWeb
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
            var connectionString = Configuration.GetConnectionString("RequestManagmentDb");
            var storageConnectionString = Configuration.GetConnectionString("BlobStorage");
            var blobContainer   = Configuration["BlobContainer"];
            string ftpUrl       = Configuration["FtpUrl"];
            string ftpLogin     = Configuration["FtpLogin"];
            string ftpPassword  = Configuration["FtpPasword"];
            string processedDir = Configuration["ProcessedDir"];

            services.AddTransient((s)=> new DataService(connectionString));
            services.AddTransient(s =>
            {
                var logger = s.GetService<ILogger<BlobStorageService>>();
                return new BlobStorageService(storageConnectionString, blobContainer, logger);
            });
            services.AddTransient(s =>
            {
                var logger = s.GetService<ILogger<FtpClient>>();
             return new FtpClient(ftpUrl, ftpLogin, ftpPassword, processedDir, logger);
            });
            services.AddTransient<AppLogic>();

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("RequestManagmentDb")));
        }

    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RunJob(app, env);

            
        }

        private void RunJob(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appJob = app.ApplicationServices.GetService<AppLogic>();
            RecurringJob.AddOrUpdate(() => appJob.Run(), Cron.Minutely);
                        
        }

        private static void UseBasicMvc(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfugureBasicMvc(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
    }
}
