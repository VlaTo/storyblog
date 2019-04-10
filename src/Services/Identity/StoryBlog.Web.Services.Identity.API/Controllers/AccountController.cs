using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Identity.API.Configuration;
using StoryBlog.Web.Services.Identity.API.Data.Models;
using StoryBlog.Web.Services.Identity.API.Extensions;
using StoryBlog.Web.Services.Identity.API.Models;
using StoryBlog.Web.Services.Identity.API.Services;
using StoryBlog.Web.Services.Shared.Captcha;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    [Route("[controller]")]
    public sealed class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService interactions;
        private readonly ILoginService<Customer> loginService;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly UserManager<Customer> customerManager;
        private readonly ICaptcha captcha;
        private readonly IHostingEnvironment environment;
        private readonly ISimpleEmailSender emailSender;
        private readonly IEmailTemplateGenerator templateGenerator;
        private readonly IEventService eventService;
        private readonly IStringLocalizer<AccountController> localizer;
        private readonly ILogger<AccountController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interactions"></param>
        /// <param name="loginService"></param>
        /// <param name="clientStore"></param>
        /// <param name="schemeProvider"></param>
        /// <param name="customerManager"></param>
        /// <param name="captcha"></param>
        /// <param name="environment"></param>
        /// <param name="emailSender"></param>
        /// <param name="templateGenerator"></param>
        /// <param name="eventService"></param>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>
        public AccountController(
            IIdentityServerInteractionService interactions,
            ILoginService<Customer> loginService,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            UserManager<Customer> customerManager,
            ICaptcha captcha,
            IHostingEnvironment environment,
            ISimpleEmailSender emailSender,
            IEmailTemplateGenerator templateGenerator,
            IEventService eventService,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            this.interactions = interactions;
            this.loginService = loginService;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.customerManager = customerManager;
            this.captcha = captcha;
            this.environment = environment;
            this.emailSender = emailSender;
            this.templateGenerator = templateGenerator;
            this.eventService = eventService;
            this.localizer = localizer;
            this.logger = logger;
        }

        // GET /account/signin
        [HttpGet("signin")]
        public async Task<IActionResult> Signin([FromQuery] string returnUrl)
        {
            var model = await CreateSigninModelAsync(returnUrl);

            if (model.IsExternalLoginOnly)
            {
                return RedirectToAction("Challenge", "External", new
                {
                    scheme = model.ExternalAuthenticationScheme,
                    returnUrl
                });
            }

            return View(model);
        }

        // POST account/signin
        [HttpPost("signin")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        [ValidateCaptcha]
        public async Task<IActionResult> Signin([FromForm] SigninInputModel model, [FromForm] string button)
        {
            var context = await interactions.GetAuthorizationContextAsync(model.ReturnUrl);

            if ("signin" != button)
            {
                logger.LogDebug("Signin executing");

                if (null != context)
                {
                    await interactions.GrantConsentAsync(context, ConsentResponse.Denied);

                    if (await clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        return View("Redirect", new RedirectModel { RedirectUrl = model.ReturnUrl });
                    }

                    return Redirect(model.ReturnUrl);
                }

                return Redirect("~/");
            }

            if (ModelState.IsValid)
            {
                var customer = await loginService.FindByEmailAsync(model.Email);

                if (null != customer)
                {
                    var result = await loginService.ValidateCredentialsAsync(customer, model.Password);

                    if (result.IsNotAllowed)
                    {
                        return View();
                    }

                    if (result.IsLockedOut)
                    {
                        return View();
                    }

                    if (result.Succeeded)
                    {
                        await eventService.RaiseAsync(new UserLoginSuccessEvent(
                            IdentityServerConstants.LocalIdentityProvider,
                            customer.NormalizedUserName,
                            customer.UserName,
                            customer.ContactName)
                        );

                        AuthenticationProperties properties = null;

                        if (AccountOptions.AllowRememberMe && model.RememberMe)
                        {
                            properties = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                IssuedUtc = DateTimeOffset.UtcNow,
                                ExpiresUtc = DateTimeOffset.UtcNow + AccountOptions.RememberMeSigninDuration
                            };
                        }

                        await loginService.SigninAsync(customer, properties, IdentityServerConstants.LocalIdentityProvider);

                        if (null != context)
                        {
                            if (await clientStore.IsPkceClientAsync(context.ClientId))
                            {
                                return View("Redirect", new RedirectModel { RedirectUrl = model.ReturnUrl });
                            }

                            return Redirect(model.ReturnUrl);
                        }

                        if (Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        if (String.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        
                        var uri = new Uri(model.ReturnUrl);

                        if (uri.IsAbsoluteUri)
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        throw new Exception("Invalid redirect url");
                    }
                }

                var invalidCredentials = localizer.InvalidCredentials(context?.UiLocales);
                await eventService.RaiseAsync(new UserLoginFailureEvent(model.Email, invalidCredentials));

                ModelState.AddModelError(String.Empty, "Invalid credentials");
            }

            return View(await CreateSigninModelAsync(model));
        }

        // GET account/create
        [HttpGet("create")]
        public async Task<IActionResult> Create([FromQuery] string returnUrl)
        {
            var model = await CreateSignupModelAsync(returnUrl);

            return View("Signup", model);
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SignupModel model)
        {
            if (false == ModelState.IsValid)
            {
                return View("Signup", model);
            }

            if (customerManager.SupportsUserEmail)
            {
                var customer = new Customer
                {

                };
                var token = await customerManager.GenerateEmailConfirmationTokenAsync(customer);
                var template = await templateGenerator.ResolveTemplateAsync("create");
                var context = new MailMessageTemplateContext
                {
                    From = new MailAddress("noreply@storyblog.org"),
                    To =
                    {
                        new MailAddress("test@storyblog.org")
                    },
                    Subject = "",
                    Replacements =
                    {
                        [nameof(token)] = token
                    }
                };

                var message = await template.GenerateAsync(context);
                await emailSender.SendMessageAsync(message);

                ;
            }

            return View("Signup", new SignupViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorId"></param>
        /// <returns></returns>
        [HttpGet("error")]
        public async Task<IActionResult> Error(string errorId)
        {
            // retrieve error details from identityserver
            var message = await interactions.GetErrorContextAsync(errorId);

            if (null == message)
            {
                return View(new ErrorViewModel(null));
            }

            if (false == environment.IsDevelopment())
            {
                // only show in development
                message.ErrorDescription = null;
            }

            return View(new ErrorViewModel(message));
        }

        private async Task<SigninViewModel> CreateSigninModelAsync(string returnUrl)
        {
            var context = await interactions.GetAuthorizationContextAsync(returnUrl);

            if (null != context?.IdP)
            {
                return new SigninViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Email = context.LoginHint,
                    ExternalProviders =
                    {
                        new ExternalProvider
                        {
                            AuthenticationScheme = context.IdP
                        }
                    }
                };
            }

            var schemes = await schemeProvider.GetAllSchemesAsync();
            var comparer = StringComparer.OrdinalIgnoreCase;
            var condition = new Func<AuthenticationScheme, bool>(scheme =>
                null != scheme.DisplayName || comparer.Equals(scheme.Name, AccountOptions.WindowsAuthenticationScheme)
            );
            var canSigninLocal = true;
            var providers = schemes
                .Where(condition)
                .Select(scheme => new ExternalProvider
                {
                    DisplayName = scheme.DisplayName,
                    AuthenticationScheme = scheme.Name
                })
                .ToArray();

            if (null != context?.ClientId)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(context.ClientId);

                if (null != client)
                {
                    canSigninLocal = client.EnableLocalLogin;

                    if (null != client.IdentityProviderRestrictions && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers
                            .Where(provider =>
                                client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)
                            )
                            .ToArray();
                    }
                }
            }

            captcha.Create(HttpContext);

            return new SigninViewModel
            {
                AllowRememberMe = AccountOptions.AllowRememberMe,
                EnableLocalLogin = canSigninLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers
            };
        }

        private async Task<SigninViewModel> CreateSigninModelAsync(SigninInputModel signin)
        {
            var model = await CreateSigninModelAsync(signin.ReturnUrl);

            model.Email = signin.Email;
            model.RememberMe = signin.RememberMe;

            return model;
        }

        private Task<SignupViewModel> CreateSignupModelAsync(string returnUrl)
        {
            var model = new SignupViewModel();

            return Task.FromResult(model);
        }
    }
}