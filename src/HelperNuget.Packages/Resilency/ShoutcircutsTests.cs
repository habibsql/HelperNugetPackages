using Polly;
using Polly.Retry;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Utitlity.Nuget.Packages.ShortCircuts
{
    /// <summary>
    /// Http call retry
    /// </summary>
    public class ShoutcircutsTests
    {
        [Fact]
        public async Task ShouldRetry()
        {
            AsyncRetryPolicy policy = Policy.Handle<ArgumentNullException>().RetryAsync(3); // the Action method will call 3 times if failed

            await policy.ExecuteAsync(async () =>
            {
                await Action();               
            });

            Thread.Sleep(10000);
        }

        private async Task<HttpResponseMessage> Action()
        {
            using var client = new HttpClient();
            var uri = new Uri("https://www.google.com/gmailfake");
            var response = await client.GetAsync(uri);

            return response;
        }
    }
}
