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
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Shared.Communication;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Shared.Common;

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
                                database.MigrationsAssembly(typeof(StoryBlogDbContext).Assembly.GetName().Name);
                            });
                        });

                    services
                        .AddCors()
                        .AddRouting()
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

                    // remove it
                    IdentityModelEventSource.ShowPII = true;

                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

                    services
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            options.Authority = "http://localhost:63655";
                            options.Audience = "api.blog";
                            options.RequireHttpsMetadata = false;

                            /*var section = context.Configuration.GetSection("Bearer");

                            options.Authority = section.GetValue<string>("Authority");
                            options.Audience = section.GetValue<string>("Audience");
                            options.RequireHttpsMetadata = false;*/
                        });

                    services.AddAuthorization(options =>
                        options.AddPolicy(
                            Policies.Admins,
                            policy => policy.RequireClaim(ClaimTypes.Role, RoleNames.Admin)
                        )
                    );

                    services.AddOidcStateDataFormatterCache("aad");

                    services.AddCors(options =>
                        options.AddPolicy("default", policy =>
                            policy
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowAnyOrigin()
                        )
                    );

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

                    services.AddMediatR(
                        typeof(RequestResult).Assembly
                    );

                    services.AddAutoMapper(config =>
                    {
                        config.AddBlogApplicationTypeMappings();

                        config
                            .CreateMap<Application.Stories.Models.Author, AuthorModel>()
                            .ForMember(
                                landing => landing.Name,
                                mapping => mapping.MapFrom(source => source.UserName)
                            );

                        config
                            .CreateMap<Application.Stories.Models.Comment, CommentModel>()
                            .ForMember(
                                story => story.Id,
                                mapping => mapping.MapFrom(source => source.Id)
                            )
                            .ForMember(
                                story => story.Parent,
                                mapping => mapping.MapFrom(source => source.ParentId)
                            )
                            .ForMember(
                                story => story.Content,
                                mapping => mapping.MapFrom(source => source.Content)
                            )
                            .ForMember(
                                story => story.Author,
                                mapping => mapping.MapFrom(source => source.Author)
                            )
                            .ForMember(
                                story => story.Created,
                                mapping => mapping.MapFrom(source => source.Created)
                            )
                            .ForMember(
                                story => story.Modified,
                                mapping => mapping.MapFrom(source => source.Modified)
                            );

                        config
                            .CreateMap<Application.Landing.Models.HeroStory, HeroStoryModel>()
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
                                story => story.Author,
                                mapping => mapping.MapFrom(source => source.Author)
                            )
                            .ForMember(
                                story => story.Created,
                                mapping => mapping.MapFrom(source => source.Created)
                            )
                            .ForMember(
                                story => story.Modified,
                                mapping => mapping.MapFrom(source => source.Modified)
                            )
                            .ForMember(
                                test => test.Comments,
                                mapping => mapping.MapFrom(source => source.CommentsCount)
                            );

                        config
                            .CreateMap<Application.Stories.Models.Story, StoryModel>()
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
                                story => story.Closed,
                                mapping => mapping.MapFrom((source, dest) => false)
                            )
                            .ForMember(
                                story => story.Author,
                                mapping => mapping.MapFrom(source => source.Author)
                            )
                            .ForMember(
                                story => story.Created,
                                mapping => mapping.MapFrom(source => source.Created)
                            )
                            .ForMember(
                                story => story.Modified,
                                mapping => mapping.MapFrom(source => source.Modified)
                            )
                            .AfterMap((source, story, ctx) =>
                                story.Comments = source.Comments
                                    .Select(comment => ctx.Mapper.Map<CommentModel>(comment))
                                    .ToArray()
                            );

                        config
                            .CreateMap<Application.Stories.Models.FeedStory, FeedStoryModel>()
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
                                story => story.Author,
                                mapping => mapping.MapFrom(source => source.Author)
                            )
                            .ForMember(
                                story => story.Created,
                                mapping => mapping.MapFrom(source => source.Created)
                            )
                            .ForMember(
                                story => story.Modified,
                                mapping => mapping.MapFrom(source => source.Modified)
                            )
                            .ForMember(
                                story => story.Comments,
                                mapping => mapping.MapFrom(source => source.CommentsCount)
                            );

                        config
                            .CreateMap<Application.Landing.Models.Landing, LandingModel>()
                            .ForMember(
                                landing => landing.Title,
                                mapping => mapping.MapFrom(source => source.Title)
                            )
                            .ForMember(
                                landing => landing.Description,
                                mapping => mapping.MapFrom(source => source.Description)
                            )
                            .ForMember(
                                landing => landing.Hero,
                                mapping => mapping.MapFrom(source => source.HeroStory)
                            )
                            .AfterMap((source, landing, ctx) =>
                            {
                                landing.Featured = source.FeaturedStories.Select(
                                    story => ctx.Mapper.Map<FeedStoryModel>(story)
                                );
                                landing.Feed = source.FeedStories.Select(
                                    story => ctx.Mapper.Map<FeedStoryModel>(story)
                                );
                            });

                        /*config
                            .CreateMap<Application.Stories.Models.FeedStory, StoryModel>()
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
                            );*/
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
                        .UseCors("default")
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
                    //StoryBlogInitializer.Seed(context, Assembly.GetAssembly(typeof(Program)), logger);
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