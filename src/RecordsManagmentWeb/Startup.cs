using System.IO;
using BusinessLogic;
using DbService;
using FtpService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecordsManagmentWeb.NodeJs;
using RecordsManagmentWeb.Services;
using StorageService;
using Hosting = Microsoft.Extensions.Hosting;

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

            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
           .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
            services.AddSingleton<Hosting.IHostedService, AppLogic>();

            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


        }    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            //app.UseCookiePolicy();

            app.UseAuthentication();
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

       
    }
}
