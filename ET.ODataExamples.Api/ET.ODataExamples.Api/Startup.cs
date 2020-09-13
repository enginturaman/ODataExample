using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ET.ODataExamples.Repositories;
using ET.ODataExamples.Storage.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ET.ODataExamples.Api
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

            services.AddDbContext<ApiContext>(opt =>
            {
                opt.UseInMemoryDatabase("testDb");
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            var context = app.ApplicationServices.GetService<ApiContext>();
            AddTestData(context);

        }

        private void AddTestData(ApiContext context)
        {
            for (int i = 0; i < 10 ; i++)
            {
                var dateTimeOffset = new DateTimeOffset(DateTime.Now);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

                var r = new Random();

                var product = new ProductDmo
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Deleted = false,
                    Name = unixDateTime.ToString(),
                    Price = r.Next(100 , 10000),
                    Quantity = r.Next(1, 500)
                };

                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
