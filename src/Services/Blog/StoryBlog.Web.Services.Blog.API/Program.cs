using AutoMapper;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Filters;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Domain;
using StoryBlog.Web.Services.Blog.Domain.Models;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using StoryBlogSettings = StoryBlog.Web.Services.Blog.API.Infrastructure.StoryBlogSettings;

namespace StoryBlog.Web.Services.Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environment = context.HostingEnvironment;

                    config
                        .SetBasePath(environment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true);

                    if (environment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }

                    config.AddEnvironmentVariables();

                    if (null != args)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddDbContext<StoryBlogDbContext>(options =>
                        {
                            var connectionString = context.Configuration.GetConnectionString("StoryBlog");

                            options.UseSqlite(connectionString, database =>
                            {
                                database.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
                            });
                        });

                    services
                        .AddCors()
                        .AddMvc(options =>
                        {
                            options.Filters.Add<HttpGlobalExceptionFilter>();
                        })
                        .AddControllersAsServices()
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                        .AddJsonOptions(options =>
                        {
                            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        });

                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                    services
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            var section = context.Configuration.GetSection("Jwt");

                            options.Authority = section.GetValue<string>("AuthorityUrl");
                            options.Audience = section.GetValue<string>("Audience");
                            options.RequireHttpsMetadata = false;
                        });

                    services
                        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                        .AddResponseCompression(compression =>
                        {
                            compression.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                            {
                                MediaTypeNames.Application.Octet
                            });
                        });

                    services
                        .AddSingleton<IDateTimeProvider, DateTimeProvider>()
                        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

                    services.AddMediatR(typeof(Program).Assembly);

                    services
                        .AddAutoMapper(config =>
                        {
                            config
                                .CreateMap<Story, FeedStory>()
                                .ForMember(
                                    story => story.Title,
                                    mapping =>
                                    {
                                        mapping.MapFrom(source => $"{source.Title} {source.Slug}");
                                    });
                        });

                    services
                        .AddOptions<StoryBlogSettings>()
                        .Bind(context.Configuration.GetSection(typeof(StoryBlogSettings).Name));
                })
                .Configure(app =>
                {
                    var environment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        var logging = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                        var logger = logging.CreateLogger<Program>();

                        logger.LogDebug("Application run in development mode");

                        app.UseDeveloperExceptionPage();
                    }

                    app
                        .UseForwardedHeaders()
                        .UseCors(options =>
                        {
                            options
                                .WithOrigins("http://localhost:29699")
                                .AllowAnyHeader()
                                .WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put);
                        })
                        .UseAuthentication()
                        .UseMvc()
                        .UseResponseCompression();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<StoryBlogInitializer>>();

                try
                {
                    //var environment = services.GetRequiredService<IHostingEnvironment>();
                    var context = services.GetRequiredService<StoryBlogDbContext>();

                    StoryBlogInitializer.Seed(context, logger);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Application Startup");
                }
            }

            host.Run();
        }
    }
}