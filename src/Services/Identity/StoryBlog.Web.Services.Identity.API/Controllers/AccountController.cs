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
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService interactions;
        private readonly ILoginService<Customer> loginService;
        //private readonly SignInManager<Customer> signInManager;
        private readonly IClientStore clientStore;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly UserManager<Customer> customerManager;
        private readonly ICaptcha captcha;
        private readonly IHostingEnvironment environment;

        //private readonly IEmailSender emailSender;
        private readonly IEventService eventService;
        private readonly IStringLocalizer<AccountController> localizer;
        //private readonly IMapper mapper;
        private readonly ILogger<AccountController> logger;

        public AccountController(
            IIdentityServerInteractionService interactions,
            ILoginService<Customer> loginService,
            //SignInManager<Customer> signInManager,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            UserManager<Customer> customerManager,
            ICaptcha captcha,
            IHostingEnvironment environment,
            //IEmailSender emailSender,
            IEventService eventService,
            //IMapper mapper,
            IStringLocalizer<AccountController> localizer,
            ILogger<AccountController> logger)
        {
            this.interactions = interactions;
            this.loginService = loginService;
            //this.signInManager = signInManager;
            this.clientStore = clientStore;
            this.schemeProvider = schemeProvider;
            this.customerManager = customerManager;
            this.captcha = captcha;
            this.environment = environment;
            //this.emailSender = emailSender;
            this.eventService = eventService;
            this.localizer = localizer;
            //this.mapper = mapper;
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
        public async Task<IActionResult> Signin([FromForm] SigninModel model, [FromForm] string button)
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
                var customer = await customerManager.FindByEmailAsync(model.Email);

                if (null != customer)
                {
                    // signInManager.CheckPasswordSignInAsync(customer, model.Password, true);
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

                        /*var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Sid, customer.Id.ToString()),
                            new Claim(ClaimTypes.Name, customer.UserName),
                            new Claim(ClaimTypes.GivenName, customer.NormalizedUserName)
                        }));
    
                        await HttpContext.SignInAsync(principal, properties);*/

                        await loginService.SigninAsync(customer, properties);

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

                        throw new Exception("Invalid redirect url");
                    }
                }

                await eventService.RaiseAsync(new UserLoginFailureEvent(customer.UserName, "Invalid credentials"));

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

        // POST account/create
        [HttpPost("create")]
        [Consumes("application/x-www-form-urlencoded")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SignupViewModel model)
        {
            if (false == ModelState.IsValid)
            {
                return View("Signup", model);
            }

            await Task.CompletedTask;

            return View("Signup", new SignupViewModel());
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

            return new SigninViewModel
            {
                AllowRememberMe = AccountOptions.AllowRememberMe,
                EnableLocalLogin = canSigninLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
                ExternalProviders = providers,
                CaptchaKey = captcha.Create(HttpContext)
            };
        }

        private async Task<SigninViewModel> CreateSigninModelAsync(SigninModel signin)
        {
            var model = await CreateSigninModelAsync(signin.ReturnUrl);

            model.Email = signin.Email;
            model.RememberMe = signin.RememberMe;

            return model;
        }

        private Task<SignupViewModel> CreateSignupModelAsync(string returnUrl)
        {
            return Task.FromResult(new SignupViewModel());
        }
    }
}