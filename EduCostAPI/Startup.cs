using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;

namespace EduCostAPI
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

            ConfigureSwagger(services);
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "College Education Cost API",
                    Description = "Simple API for determining cost of a given college including tuition as well as room and board fees (optional). Created with .Net Core 3.1",
                    Contact = new OpenApiContact
                    {
                        Name = "Tyler Ng",
                        Email = "tylerdng18@gmail.com",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EduCost API V1");
            });


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
            });


        }

        public class BufferNonStreamedContentHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                   CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (response.Content != null)
                {
                    var services = request.GetConfiguration().Services;
                    var bufferPolicy = (IHostBufferPolicySelector)services.GetService(typeof(IHostBufferPolicySelector));

                    // If the host is going to buffer it anyway
                    if (bufferPolicy.UseBufferedOutputStream(response))
                    {
                        // Buffer it now so we can catch the exception
                        await response.Content.LoadIntoBufferAsync();
                    }
                }
                return response;
            }
        }
    }
}
