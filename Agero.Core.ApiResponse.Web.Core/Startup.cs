using Agero.Core.ApiResponse.Extensions;
using Agero.Core.ApiResponse.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Agero.Core.ApiResponse.Web.Core
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var responseHandler =
                new ResponseHandler(
                    logInfo: (message, data) => Debug.WriteLine($"INFO: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
                    logError: (message, data) => Debug.WriteLine($"ERROR: {message}{Environment.NewLine}{JsonConvert.SerializeObject(data)}"),
                    extractAdditionalData: ex => ex.ExtractAdditionalData(),
                    includeExceptionDetails: true);

            services.AddMvc(config =>
            {
                config.Filters.Add(new ExceptionHandlingFilterAttribute(responseHandler));
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
                app.UseHsts();
            }

            app.Use(requestDelegate => context =>
            {
                context.Request.EnableBuffering();

                return requestDelegate(context);
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
