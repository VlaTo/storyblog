using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Identity.API.Extensions;
using StoryBlog.Web.Services.Identity.API.Infrastructure;
using StoryBlog.Web.Services.Identity.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[SecurityHeaders]
    [Authorize]
    [Route("[controller]")]
    public sealed class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IResourceStore resourceStore;
        private readonly IEventService eventService;
        private readonly ILogger<ConsentController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interaction"></param>
        /// <param name="clientStore"></param>
        /// <param name="resourceStore"></param>
        /// <param name="eventService"></param>
        /// <param name="logger"></param>
        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService eventService,
            ILogger<ConsentController> logger)
        {
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.resourceStore = resourceStore;
            this.eventService = eventService;
            this.logger = logger;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);

            if (vm != null)
            {
                return View(vm);
            }

            return View("Error");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("confirm")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm([FromForm] ConsentInputModel model)
        {
            var result = await ProcessConsent(model);

            if (result.IsRedirect)
            {
                if (await clientStore.IsPkceClientAsync(result.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectModel { RedirectUrl = result.RedirectUri });
                }

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError(string.Empty, result.ValidationError);
            }

            if (result.ShowView)
            {
                return View(result.ViewModel);
            }

            return View("Error");
        }

        private async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            // validate return url is still valid
            var request = await interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            if (null == request)
            {
                return result;
            }

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model?.Button == "no")
            {
                grantedConsent = ConsentResponse.Denied;

                // emit event
                await eventService.RaiseAsync(new ConsentDeniedEvent(
                    User.GetSubjectId(),
                    request.ClientId,
                    request.ScopesRequested)
                );
            }
            // user clicked 'yes' - validate the data
            else if (model?.Button == "yes")
            {
                // if the user consented to some scope, build the response model
                if (null != model.ScopesConsented && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;

                    if (false == ConsentOptions.EnableOfflineAccess)
                    {
                        scopes = scopes.Where(
                            x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess
                        );
                    }

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };

                    // emit event
                    await eventService.RaiseAsync(new ConsentGrantedEvent(
                        User.GetSubjectId(),
                        request.ClientId,
                        request.ScopesRequested,
                        grantedConsent.ScopesConsented,
                        grantedConsent.RememberConsent)
                    );
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (null != grantedConsent)
            {
                // communicate outcome of consent back to identityserver
                await interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
                result.ClientId = request.ClientId;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await interaction.GetAuthorizationContextAsync(returnUrl);

            if (null != request)
            {
                var client = await clientStore.FindEnabledClientByIdAsync(request.ClientId);

                if (null != client)
                {
                    var resources = await resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

                    if (null != resources && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnUrl, request, client, resources);
                    }

                    logger.LogError("No scopes matching: {0}", request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                }
                else
                {
                    logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            }
            else
            {
                logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(
            ConsentInputModel model,
            string returnUrl,
            AuthorizationRequest request,
            Client client, 
            Resources resources)
        {
            var viewModel = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ReturnUrl = returnUrl,

                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent
            };

            viewModel.IdentityScopes = resources.IdentityResources
                .Select(x => CreateScopeViewModel(x, viewModel.ScopesConsented.Contains(x.Name) || model == null))
                .ToArray();
            viewModel.ResourceScopes = resources.ApiResources
                .SelectMany(x => x.Scopes)
                .Select(x => CreateScopeViewModel(x, viewModel.ScopesConsented.Contains(x.Name) || model == null))
                .ToArray();

            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                viewModel.ResourceScopes = viewModel.ResourceScopes
                    .Union(new[] {
                    GetOfflineAccessScope(
                        viewModel.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return viewModel;
        }

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public class ProcessConsentResult
        {
            public bool IsRedirect => RedirectUri != null;

            public string RedirectUri { get; set; }
            public string ClientId { get; set; }

            public bool ShowView => ViewModel != null;
            public ConsentViewModel ViewModel { get; set; }

            public bool HasValidationError => ValidationError != null;
            public string ValidationError { get; set; }
        }

    }
}