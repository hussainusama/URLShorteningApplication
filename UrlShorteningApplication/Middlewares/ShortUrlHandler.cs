using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UrlShorteningApplication.Middlewares
{
    public class ShortUrlHandler
    {
        private readonly IHttpClientFactory _clientFactory;

        public ShortUrlHandler(RequestDelegate next, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var encodedString = context.Request.Path.Value.Substring(1);
            var client = _clientFactory.CreateClient("urlshorteningservice");
            var response = await client.GetAsync($"api/url/lengthen?encodedUrl={encodedString}");
            response.EnsureSuccessStatusCode();
            var url = await response.Content.ReadAsStringAsync();
            context.Response.Redirect(url);
        }
    }
}
