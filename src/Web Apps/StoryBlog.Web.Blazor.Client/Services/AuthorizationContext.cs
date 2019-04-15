using System;
using StoryBlog.Web.Blazor.Reactive;

namespace StoryBlog.Web.Blazor.Client.Services
{
    public sealed class AuthorizationContext : IObservable<AuthorizationToken>
    {
        private readonly Subject<AuthorizationToken> subject;
        private AuthorizationToken token;

        public AuthorizationToken Token
        {
            get => token;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException();
                }

                if (token == value)
                {
                    return;
                }

                token = value;
                subject.OnNext(value);
            }
        }

        public AuthorizationContext()
        {
            subject = new Subject<AuthorizationToken>();
        }

        public IDisposable Subscribe(IObserver<AuthorizationToken> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}