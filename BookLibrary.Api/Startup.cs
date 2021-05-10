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
using BookLibrary.Application.MediatR.PipeLines;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookLibrary.Infra.WebFramework.Configuration;
using BookLibrary.Domain.Domains.Users;
using Microsoft.AspNetCore.Identity;
using BookLibrary.Infra.WebFramework.Api;
using BookLibrary.Infra.WebFramework.Exceptions;
using AspNetCoreRateLimit;

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
            services.AddOptions();
            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ApiResultFilter));
                config.Filters.Add(typeof(ApiExceptionFilter));
            }).AddNewtonsoftJson(config =>
            {
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<BookLibraryDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("default"));
                });

            services.Configure<JwtConfigs>(Configuration.GetSection(JwtConfigs.JwtAuth));
            services.AddMediatR(Assembly.Load("BookLibrary.Application"));
            services.AddAutoMapper(typeof(BookLibraryDbContext).Assembly, typeof(Startup).Assembly);
            services.AddScoped<IWriteBookRepository, WriteBookRepository>();
            services.AddScoped<IReadBookRepository, ReadBookRepository>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<BookLibraryDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new()
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = Configuration["JwtAuth:Issuer"],
                     ValidAudience = Configuration["JwtAuth:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuth:Key"]))
                 };
                 options.Events = new JwtBearerEvents()
                 {
                     OnForbidden = tokenForbid =>
                     {
                         throw new ApiException(System.Net.HttpStatusCode.Forbidden, "you are Forbiden");
                     },
                     OnChallenge = challenge =>
                     {
                         if (challenge.AuthenticateFailure != null)
                             throw new ApiException(System.Net.HttpStatusCode.Unauthorized, challenge.AuthenticateFailure);
                         throw new ApiException(System.Net.HttpStatusCode.Unauthorized, "You are unauthorized to access this resource.");

                     },
                     OnAuthenticationFailed = failedContext =>
                     {
                         if (failedContext.Exception != null)
                         {
                             throw new ApiException(System.Net.HttpStatusCode.Unauthorized, "Authentication failed.");
                         }
                         throw new ApiException(System.Net.HttpStatusCode.Unauthorized, "Authentication failed.");

                     }
                 };


             });


            services.AddScoped<JwtService>();


            //rate limit
            #region rate limit
            services.AddMemoryCache();

            ////load general configuration from appsettings.json
            services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));

            //inject counter and rules stores
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion



        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            app.UseCustomExceptionHandler();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseClientRateLimiting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
