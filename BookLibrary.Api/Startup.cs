using BookLibrary.Domain.Interfaces.ReadRepositories.Book;
using BookLibrary.Domain.Interfaces.WriteRepositories.Book;
using BookLibrary.Infra.Data.Data;
using BookLibrary.Infra.Data.Repositories.ReadRepositories;
using BookLibrary.Infra.Data.Repositories.WriteRepositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookLibrary.Infra.WebFramework.Filters;
using BookLibrary.Infra.WebFramework.Middlewares;
namespace BookLibrary.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ApiResultFilter));
            }).AddNewtonsoftJson(config =>
            {
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<BookLibraryDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("default"));
                });
            services.AddMediatR(Assembly.Load("BookLibrary.Application"));
            services.AddAutoMapper(typeof(BookLibraryDbContext).Assembly, typeof(Startup).Assembly);
            services.AddScoped<IWriteBookRepository, WriteBookRepository>();
            services.AddScoped<IReadBookRepository, ReadBookRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseCustomExceptionHandler();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
