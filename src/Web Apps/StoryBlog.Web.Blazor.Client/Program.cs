using Blazor.Extensions.Logging;
using Blazor.Fluxor;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Blazor.Client.Services;

namespace StoryBlog.Web.Blazor.Client
{
    // http://wrapbootstrap.com/preview/WB0C58SK5
    // idsrv.session=25e0e82acbf8d1afbd9e7b5740cc294e;
    // StoryBlog.Identity=CfDJ8G9jL2ym9TZNkucOWFakkM1W5o1QldSJCPq9EuydziDxTo3ewZ74G9_T6-SQsctRUFIajda3UYzlPIFCACoXF03S2Ozl0gSt3GmkzhiV8YUdsu8sZFxyVN7e3iRqZ0KmvrjmgaOwhjk0YjfHaAPetBqz3RjIAT9sNcU8YcbECwQTU4fH8eSsHYTfqII8pR_iI1dFzIrU8-MuOzMnZCwALDryvJV3N_iU6jzbpM2nUkFkKYeC-UabBvqYMbw1omqlTHdL8I_OKtva3BXqPnHaCcf8lxIHzCkR7LYhyJ2af-_VKvcaABrn9erbJSLfHgEva8TBLNXFHKxNbP4vueyrGzlCt_nbg_kv8KUlHhxbgCuON-bOMyckOr8CFswkz6jef27phn8e3SJ-tiMFDCPV_1lIvRsyjOTlwVaigEw-tzd8Q4Zjty59h7BwaZR8LnAojLxbGsRRj4waT23wrbaOnSbeCqsnwdziRP-EKtsgo5duXE-Qey2xilOoTNZvE3o7m04lPdoRka_p9sVaVS6MMjD-j5y8UekUZg8SQNvS7rKvNwWmQ0aIql2wwcm85tfKV1zwkGdMCTPhBNngfBDSNtpjwwdsT3bJxu9W6gUYfXU1DbnO23d1Py6HEG6dv7VQaAUX7x9lE4Kq75T5U5Wzwj6J0mIpkiGxTnGZYuPvx9bdtIn4tdtyex-KtcZNYx3TDLcG9mwaPOAZlZ51jwP9ftGhOhCXa6IiE2FQaYw9GqemnaQ62rexVS9eIoTyAeFKg3VVjjU6QH62Jm0dlVFwr0Jk54Za23n0kmWyrLkmxfxmwYoOpOLCv7tlQn17uA2f4lc7JW1FV1KZfLtIDJu_xyNZ1frFm7CMyGEznTm1q91uYZnCRY7Z8L_HxGcXoR7XPuaWnOyY2XFx_YXrWK3NDKg
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = BlazorWebAssemblyHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging(options =>
                        options
                            .AddBrowserConsole()
                            .SetMinimumLevel(LogLevel.Trace)
                    );
                    services.AddFluxor(options =>
                        options.UseDependencyInjection(typeof(Program).Assembly)
                    );
                    services
                        .AddSingleton<IBlogApiClient, BlogApiClient>()
                        .AddSingleton<IUserApiClient, UserApiClient>();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
