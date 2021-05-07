using BookLibrary.Domain.Interfaces.WriteRepositories.Book;
using BookLibrary.Infra.Data.Data;
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
            services.AddControllers();
            services.AddDbContext<BookLibraryDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("default"));
            });
            services.AddMediatR(Assembly.Load("BookLibrary.Application"));
            services.AddAutoMapper(typeof(BookLibraryDbContext).Assembly);
            services.AddScoped<IWriteBookRepository, WriteBookRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
