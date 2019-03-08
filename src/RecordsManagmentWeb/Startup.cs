using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using DbService;
using FtpService;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecordsManagmentWeb.NodeJs;
using RecordsManagmentWeb.Services;
using StorageService;

namespace RecordsManagmentWeb
{
    public class Startup
    {
       public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

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

            services.Configure<NodeOptions>(Configuration.GetSection("NodeOptions"));
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString));

            services.AddTransient((s) =>
            {
                var logger = s.GetService<ILogger<DataService>>();
                return new DataService(connectionString, logger);
            });
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
            services.AddNodeServices((opt)=> {
               
            });
            
            services.AddTransient(s=> {
                var pdfService = new PdfGenearatorService(s.GetService<INodeServices>(), 
                                                          s.GetService<ApplicationDbContext>(),
                                                          s.GetService<BlobStorageService>(),
                                                          s.GetService<IOptions<NodeOptions>>(),
                                                          s.GetService<ILogger<PdfGenearatorService>>());
                return pdfService;

            });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("RequestManagmentDb")));
            ConfugureBasicMvc(services);
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            
        }    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            UseBasicMvc(app, env);

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireAuthorization() },
            });
            //RunJob(app, env);

            
        }

        private void RunJob(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appJob = app.ApplicationServices.GetService<AppLogic>();
            //RecurringJob.AddOrUpdate(() => appJob.Run(), Cron.MinuteInterval(10));
                        
        }

        private static void UseBasicMvc(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //This temporary solution
                app.UseDeveloperExceptionPage();

                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }



            //app.UseHttpsRedirection();
            app.UseFileServer();
            app.UseCookiePolicy();

            //app.UseAuthentication();

            app
            .UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

            });
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });


           // app.UseSpa(
           //     spa =>
           // {
           //     // To learn more about options for serving an Angular SPA from ASP.NET Core,
           //     // see https://go.microsoft.com/fwlink/?linkid=864501

           //       //spa.Options.SourcePath = "ClientApp";

           // //    if (env.IsDevelopment())
           // //    {
           // //        spa.UseAngularCliServer(npmScript: "start");
           // //    }
           //});
        }

        private void ConfugureBasicMvc(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddAuthorization(o =>
            //{
            //    o.AddPolicy("Default", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        //To require the basic user_impersonation scope across the API, you can use:
            //       //policy..RequirePermissions(
            //       //    delegated: new[] { "user_impersonation" },
            //       //    application: new string[0]);
            //    });
            //});

            services
                  .AddAuthentication(AzureADDefaults.AuthenticationScheme)
                  .AddAzureAD(options => Configuration.Bind("AzureAd", options));
                 // .AddAuthentication(o =>
                 // {
                 //     o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                 // })
                 //.AddJwtBearer("AzureAdToken", options =>
                 //{
                 //    options.Authority = Configuration["AzureAd:Authority"];
                 //    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 //    {
                 //        // Both App ID URI and client id are valid audiences in the access token
                 //        ValidAudiences = new List<string>
                 //   {
                 //       Configuration["AzureAd:AppIdUri"],
                 //       Configuration["AzureAd:ClientId"]
                 //   }
                 //    };
                 //});
           // services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }
    }
}
