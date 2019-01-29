using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using StoryBlog.Web.Blazor.Shared.Cart;

namespace StoryBlog.Web.Blazor.Client.Core
{
    public class ApiClient : IApiClient
    {
        private readonly CancellationTokenSource cts;
        private readonly HttpClient http;

        public ApiClient(HttpClient http)
        {
            this.http = http;
            this.http.BaseAddress = new Uri("http://localhost:61601/api/");
            this.http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(BlazorMediaTypeNames.Application.Json)
            );

            cts = new CancellationTokenSource();
        }

        public async Task<Product[]> LoadCartProductsAsync(Guid cartId)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"cart/{cartId}"))
            {
                using (var response = await http.SendAsync(request, cts.Token))
                {
                    if (false == response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var target = Json.Deserialize<Product[]>(json);

                    return target;
                }
            }
        }

        /*public async Task<IEnumerable<FeedStory>> LoadStoriesAsync(int pageNumber, int pageSize)
        {
            return await http.GetJsonAsync<FeedStory[]>($"feed/{pageNumber}?size={pageSize}");
        }*/


        /*public async Task<SigninResult> SigninAsync(string email, string password, bool rememberMe)
        {
            return await http.PostJsonAsync<SigninResult>("signin", new SigninCredentials
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            });
        }*/
    }
}