using MediatR;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.Application.Models;
using StoryBlog.Web.Services.Identity.Application.Services;
using StoryBlog.Web.Services.Identity.Application.Signup.Commands;
using StoryBlog.Web.Services.Identity.Persistence.Models;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Signup.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public sealed class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand>
    {
        private readonly UserManager<Customer> customerManager;
        private readonly ISimpleEmailSender emailSender;
        private readonly IEmailTemplateGenerator templateGenerator;

        public SendConfirmationEmailCommandHandler(
            UserManager<Customer> customerManager,
            ISimpleEmailSender emailSender,
            IEmailTemplateGenerator templateGenerator)
        {
            this.customerManager = customerManager;
            this.emailSender = emailSender;
            this.templateGenerator = templateGenerator;
        }

        public async Task<Unit> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            if (false == customerManager.SupportsUserEmail)
            {
                return Unit.Value;
            }

            var token = await customerManager.GenerateEmailConfirmationTokenAsync(request.Customer);
            var template = await templateGenerator.ResolveTemplateAsync("create");
            var context = new MailMessageTemplateContext
            {
                From = new MailAddress("noreply@storyblog.org"),
                To =
                {
                    new MailAddress(request.Customer.Email)
                },
                Subject = "",
                Replacements =
                {
                    [nameof(token)] = token
                }
            };

            var message = await template.GenerateAsync(context);
            await emailSender.SendMessageAsync(message);

            return Unit.Value;
        }
    }
}