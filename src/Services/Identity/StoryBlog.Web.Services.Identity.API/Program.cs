using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using StoryBlog.Web.Services.Identity.API.Configuration;
using StoryBlog.Web.Services.Identity.API.Data;
using StoryBlog.Web.Services.Identity.API.Data.Models;
using StoryBlog.Web.Services.Identity.API.Extensions;
using StoryBlog.Web.Services.Identity.API.Services;
using StoryBlog.Web.Services.Shared.Captcha;
using StoryBlog.Web.Services.Shared.Captcha.Extensions;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mime;

namespace StoryBlog.Web.Services.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Identity.API";

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
                    var environment = context.HostingEnvironment;
                    var connectionString = context.Configuration.GetConnectionString("StoryBlog");
                    var migrationAssemblyName = typeof(Program).Assembly.GetName().Name;

                    services
                        .AddDbContext<StoryBlogIdentityDbContext>(options =>
                        {
                            options.UseSqlite(connectionString, database =>
                            {
                                database.MigrationsAssembly(migrationAssemblyName);
                            });
                        })
                        .AddIdentity<Customer, CustomerRole>(options =>
                        {
                            options.User.RequireUniqueEmail = true;
                            options.User.AllowedUserNameCharacters =
                                "abcdefghijklmnopqrstuvwxyz" +
                                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                "0123456789" +
                                "-._@+" +
                                "абвгдеёжзийклмнопрстуфхцчшщьъэюя" +
                                "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧЬШЩЪЭЮЯ";
                        })
                        .AddEntityFrameworkStores<StoryBlogIdentityDbContext>()
                        .AddDefaultTokenProviders();

                    if (context.Configuration.GetValue<bool>("Environment:IsClustered"))
                    {
                        /*services.AddDataProtection(options =>
                        {
                            options.ApplicationDiscriminator = "storyblog.identity";
                        });*/
                    }

                    services
                        .AddResponseCompression(compression =>
                        {
                            compression.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                            {
                                MediaTypeNames.Application.Octet
                            });
                        })
                        .AddLocalization(options =>
                        {
                            options.ResourcesPath = "Resources";
                        })
                        .AddCors(options =>
                        {
                            options.AddDefaultPolicy(policy =>
                                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()
                            );
                        })
                        .AddMvc()
                        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                        .AddJsonOptions(options =>
                        {
                            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        })
                        .AddViewLocalization(
                            LanguageViewLocationExpanderFormat.Suffix,
                            options =>
                            {
                                options.ResourcesPath = "Resources";
                            }
                        )
                        .AddDataAnnotationsLocalization();

                    services
                        .AddIdentityServer(options =>
                        {
                            options.Endpoints = new EndpointsOptions
                            {
                                EnableDiscoveryEndpoint = true,
                                EnableAuthorizeEndpoint = true,
                                EnableCheckSessionEndpoint = true,
                                EnableEndSessionEndpoint = true,
                                EnableUserInfoEndpoint = true
                            };

                            /*options.Authentication = new AuthenticationOptions
                            {
                                CookieLifetime = TimeSpan.FromSeconds(300.0d),
                                CookieAuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme
                            };*/

                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseFailureEvents = true;
                            options.Events.RaiseInformationEvents = true;
                            options.Events.RaiseSuccessEvents = true;

                            //options.IssuerUri = null;

                            options.UserInteraction.LoginUrl = "/account/signin";
                            options.UserInteraction.ConsentUrl = "/consent/confirm";
                            options.UserInteraction.ErrorUrl = "/account/error";
                        })
                        .AddDeveloperSigningCredential()
                        //.AddSigningCredential(Certificate.Get())
                        .AddAspNetIdentity<Customer>()
                        .AddConfigurationStore(options =>
                            options.ConfigureDbContext = builder =>
                            {
                                builder.UseSqlite(connectionString, database =>
                                {
                                    database.MigrationsAssembly(migrationAssemblyName);
                                });
                            }
                        )
                        .AddOperationalStore(options =>
                            options.ConfigureDbContext = builder =>
                            {
                                builder.UseSqlite(connectionString, database =>
                                {
                                    database.MigrationsAssembly(migrationAssemblyName);
                                });
                            }
                        );

                    if (environment.IsDevelopment())
                    {
                        /*identityServerBuilder
                            .AddSigningCredential(Certificate.Get())
                            .AddInMemoryPersistedGrants()
                            .AddInMemoryIdentityResources(Config.GetIdentityResources())
                            .AddInMemoryApiResources(Config.GetApiResources())
                            .AddInMemoryClients(Config.GetClients())
                            .AddAspNetIdentity<Customer>()
                            ;*/
                    }
                    else
                    {
                        /*identityServerBuilder
                            .AddSigningCredential((SigningCredentials)null)
                            .AddAspNetIdentity<Customer>()
                            .AddConfigurationStore(options =>
                                options.ConfigureDbContext = builder =>
                                {
                                    builder.UseSqlite(connectionString, database =>
                                    {
                                        database.MigrationsAssembly(migrationAssemblyName);
                                    });
                                }
                            )
                            .AddOperationalStore(options =>
                                options.ConfigureDbContext = builder =>
                                {
                                    builder.UseSqlite(connectionString, database =>
                                    {
                                        database.MigrationsAssembly(migrationAssemblyName);
                                    });
                                }
                            );*/
                    }

                    services
                        .AddDistributedMemoryCache()
                        .AddOidcStateDataFormatterCache()
                        .AddAuthentication(IdentityConstants.ApplicationScheme)
                        .AddGoogle(options =>
                        {
                            var google = context.Configuration.GetSection("Authentication:Google");

                            options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                            options.CallbackPath = "/callback/google/authorize";
                            options.ClientId = google.GetValue<string>("ClientId");
                            options.ClientSecret = google.GetValue<string>("ClientSecret");
                        });

                    services
                        /*.ConfigureApplicationCookie(options =>
                        {
                            options.Cookie.Name = "StoryBlog.Identity";
                            options.Cookie.HttpOnly = true;
                            options.Cookie.SameSite = SameSiteMode.Lax;
                            options.ExpireTimeSpan = TimeSpan.FromHours(1.0d);
                            options.SlidingExpiration = true;
                        })*/
                        .AddAntiforgery();

                    // remove it
                    //IdentityModelEventSource.ShowPII = true;

                    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    services.TryAddTransient<IEventService, DefaultEventService>();
                    services.TryAddTransient<IEventSink, DefaultEventSink>();

                    services
                        .AddTransient<ILoginService<Customer>, EntityFrameworkLoginService>()
                        .AddTransient<IProfileService, EntityFrameworkProfileService>()
                        .AddSimpleEmailSender(options =>
                        {
                            var settings = context.Configuration.GetSection("EmailSender");

                            options.Credentials = new NetworkCredential(
                                settings.GetValue<string>("Credentials:User"),
                                settings.GetValue<string>("Credentials:Password")
                            );
                        })
                        .AddOptions<StoryBlogIdentityOptions>()
                        .Bind(context.Configuration.GetSection("StoryBlog:Options")); ;

                    services
                        .AddResponseCompression(compression =>
                        {
                            compression.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                            {
                                MediaTypeNames.Application.Octet
                            });
                        });

                    services
                        .AddTransient<StoryBlogIdentitySetup>()
                        .AddCaptcha(options =>
                        {
                            options.CaptchaLength = 5;
                            options.Comparison = CaptchaComparisonMode.CaseInsensitive;
                            options.AllowedChars = "abcdefghijklmnopqrstuvwxyz" +
                                                   "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                                   "1234567890";
                            options.Timeout = TimeSpan.FromMinutes(15.0d);
                            options.RequestPath = new PathString("/api/v1/captcha");
                            options.Image.Size = new Size(200, 60);
                            options.Cookie.Domain = "localhost";
                            options.Cookie.HttpOnly = true;
                            options.Cookie.SameSite = SameSiteMode.Strict;
                        })
                        /*
                            .AddAutoMapper(config =>
                            {
                                config
                                    .CreateMap<Customer, SigninResultModel>()
                                    .ForMember(customer => customer.UserName,
                                        mapping =>
                                        {
                                            mapping.ResolveUsing(source => $"{source.UserName} {source.ContactName}");
                                        });
                            });*/
                        ;
                })
                .Configure(app =>
                {
                    var environment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }

                    app
                        .UseResponseCompression()
                        .UseForwardedHeaders()
                        .UseCors()
                        .UseRequestLocalization(options =>
                        {
                            var cultures = new[]
                            {
                                new CultureInfo("en-US"),
                                new CultureInfo("ru-RU")
                            };

                            options.SupportedCultures = cultures;
                            options.SupportedUICultures = cultures;
                        })
                        .UseStaticFiles()
                        .UseAuthentication()
                        .UseIdentityServer()
                        .UseCaptcha()
                        .UseMvc();
                })
                .Build();

            /*using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var identitySetup = services.GetRequiredService<StoryBlogIdentitySetup>();

                try
                {
                    //services.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                    //services.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                    //services.GetRequiredService<StoryBlogIdentityDbContext>().Database.Migrate();
                    identitySetup.SeedAsync().Wait();
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Application Startup");
                }
            }*/

            host.Run();
        }
    }
}
