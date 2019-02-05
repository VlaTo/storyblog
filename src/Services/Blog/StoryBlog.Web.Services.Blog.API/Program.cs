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
using StoryBlog.Web.Services.Blog.Application.Extensions;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Persistence;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;

namespace StoryBlog.Web.Services.Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder()
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
                                //database.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
                                database.MigrationsAssembly(typeof(StoryBlogDbContext).Assembly.GetName().Name);
                            });
                        });

                    services
                        .AddCors()
                        .AddRouting(/*options =>
                        {
                            options.ConstraintMap["cursor"] = typeof(NavigationCursorRouteConstraint);
                        }*/)
                        .AddMvc(options =>
                        {
                            options.Filters.Add<HttpGlobalExceptionFilter>();
                            options.Conventions.Add(new CommaSeparatedFlagsQueryStringConvention());
                        })
                        .AddControllersAsServices()
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
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
                            var section = context.Configuration.GetSection("Bearer");

                            options.Authority = section.GetValue<string>("Authority");
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
                        .AddSingleton<ISlugGenerator, SlugTextGenerator>()
                        .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>))
                        .AddSingleton<ICommandBus, NullCommandBus>();

                    services.AddMediatR();

                    services
                        .AppBlogApplicationDependencies()
                        .AddAutoMapper(config =>
                        {
                            config
                                .CreateMap<Application.Stories.Models.Author, Common.Models.AuthorModel>()
                                /*.ConvertUsing(source => new Common.Models.AuthorModel
                                {
                                    Id = source.Id,
                                    Name = source.UserName
                                })*/
                                .ForMember(
                                    story => story.Id,
                                    mapping => mapping.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    story => story.UserName,
                                    mapping => mapping.MapFrom(source => source.UserName)
                                );

                            config
                                .CreateMap<Application.Stories.Models.Comment, Common.Models.CommentModel>()
                                .ForMember(
                                    story => story.Id,
                                    mapping => mapping.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    story => story.Content,
                                    mapping =>mapping.MapFrom(source => source.Content))
                                .ForMember(
                                    story => story.Author,
                                    mapping =>
                                    {
                                        mapping.AllowNull();
                                        mapping.MapFrom(source => source.Author);
                                    })
                                .ForMember(
                                    story => story.Created,
                                    mapping => mapping.MapFrom(source => source.Created)
                                )
                                .ForMember(
                                    story => story.Modified,
                                    mapping =>
                                    {
                                        mapping.AllowNull();
                                        mapping.MapFrom(source => source.Modified);
                                    });

                            config
                                .CreateMap<Application.Stories.Models.Story, Common.Models.StoryModel>()
                                .ForMember(
                                    story => story.Id,
                                    mapping => mapping.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    story => story.Title,
                                    mapping => mapping.MapFrom(source => source.Title)
                                )
                                .ForMember(
                                    story => story.Slug,
                                    mapping => mapping.MapFrom(source => source.Slug)
                                )
                                .ForMember(
                                    story => story.Content,
                                    mapping => mapping.MapFrom(source => source.Content)
                                )
                                .ForMember(
                                    story => story.IsPublic,
                                    mapping => mapping.MapFrom(source => source.IsPublic)
                                )
                                .ForMember(
                                    story => story.Author,
                                    mapping =>
                                    {
                                        mapping.AllowNull();
                                        mapping.MapFrom(source => source.Author);
                                    })
                                .ForMember(
                                    story => story.Comments,
                                    mapping => mapping.MapFrom(source => source.Comments)
                                )
                                .ForMember(
                                    story => story.Created,
                                    mapping => mapping.MapFrom(source => source.Created)
                                )
                                .ForMember(
                                    story => story.Modified,
                                    mapping =>
                                    {
                                        mapping.AllowNull();
                                        mapping.MapFrom(source => source.Modified);
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
                        //var logging = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                        //var logger = logging.CreateLogger<Program>();

                        //logger.LogDebug("Application run in development mode");

                        //app.UseDeveloperExceptionPage();
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
                    var environment = services.GetRequiredService<IHostingEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        ;
                    }

                    //var context = services.GetRequiredService<StoryBlogDbContext>();

                    //context.Database.Migrate();
                    //StoryBlogInitializer.Seed(context, logger);
                }
                catch (Exception exception)
                {
                    logger.LogCritical(exception, "Application Startup");
                }
            }

            host.Run();
        }
    }
}