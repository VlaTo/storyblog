using System;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class AuthorizationContext : Obser IObservable<string>
    {
        private string token;

        public string Token
        {
            get => token;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                token = value;

            }
        }

        public IDisposable Subscribe(IObserver<string> observer)
        {
            throw new NotImplementedException();
        }
    }
}