using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShorteningApplication.Middlewares;

namespace UrlShorteningApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var urlShorteningServiceEndpoint = Configuration["WebService:UrlShorteningServiceEndPoint"];
            services.AddHttpClient("urlshorteningservice", c => c.BaseAddress = new Uri(urlShorteningServiceEndpoint));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            app.MapWhen(
                context => new Regex("^[a-zA-Z0-9]{1,6}$").IsMatch(context.Request.Path.Value.Substring(1)),
                builder =>
                {
                    builder.UseMiddleware<ShortUrlHandler>();
                });
        }
    }
}
