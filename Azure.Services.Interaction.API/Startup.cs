using Azure.Services.Interaction.API.Model;
using Azure.Services.Interaction.B2C;
using Azure.Services.Interaction.RestClient;
using Azure.Services.Interaction.ServiceBus;
using Azure.Services.Interaction.Storage;
using Azure.Services.Interaction.Storage.Contracts;
using Azure.Services.Interaction.Storage.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.Services.Interaction.API
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
            services.AddLogging(opt =>
            {
                opt.AddConsole();
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });

            // Add Azure B2C Service.
            services.AddAzureB2CServices(Configuration);
            
            // Add HttpClient And Polity Factory, Support for B2C Service.
            services.AddHttpClientAndPolicyFactory();
            
            // Add Azure Service Bus Service.
            services.AddAzureServiceBusServices(Configuration);

            // Add Azure Storage Blob and Table Service.
            services.AddAzureStorageServices<ItemTransaction>(Configuration);
            
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Azure.Services.Interaction.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azure.Services.Interaction.API v1"));
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
