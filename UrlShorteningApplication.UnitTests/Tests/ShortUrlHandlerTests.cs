using System.Net;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using NSubstitute;
using UrlShorteningApplication.Middlewares;
using UrlShorteningApplication.UnitTests.Infrastructure;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoFixture;
using AutoFixture.Kernel;

namespace UrlShorteningApplication.UnitTests.Tests
{
    public class ShortUrlHandlerTests
    {
        [Theory, ShortHanderAutoData]
        public async void Invoke_ServiceReturnsSuccessfulStatusCodeWithOriginalUrl_RedirectsToOriginalUrl(
           [Frozen]DummyContext context,
           [Frozen]HttpResponseMessage responseMessage,
           [Frozen]FakeHttpMessageHandler fakeHttpMessageHandler,
           [Frozen]HttpClient fakeHttpClient,
           [Frozen]IHttpClientFactory clientFactory,
           string url,
           ShortUrlHandler sut)
        {
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new StringContent(url);
            context.Request.Path = new PathString("/2");
            clientFactory.CreateClient("urlshorteningservice").Returns(fakeHttpClient);

            await sut.Invoke(context);

            context.Response.Received().Redirect(url);
        }

        [Theory, ShortHanderAutoData]
        public async void Invoke_ServiceReturnsUnsuccessfulStatusCode_ThrowsException(
            [Frozen]DummyContext context,
            [Frozen]HttpResponseMessage responseMessage,
            [Frozen]FakeHttpMessageHandler fakeHttpMessageHandler,
            [Frozen]HttpClient fakeHttpClient,
            [Frozen]IHttpClientFactory clientFactory,
            ShortUrlHandler sut)
        {
            responseMessage.StatusCode = HttpStatusCode.InternalServerError;
            context.Request.Path = new PathString("/2");
            clientFactory.CreateClient("urlshorteningservice").Returns(fakeHttpClient);

            await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.Invoke(context));
   
        }
    }

    public class DummyContext : DefaultHttpContext
    {
        public DummyContext()
        {
            Response = Substitute.For<HttpResponse>();
        }
        
        public override HttpResponse Response { get; }
    }

    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private HttpResponseMessage _fakeResponse;

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }

    public class ShortHanderAutoDataAttribute : AutoDataAttribute
    {
        public ShortHanderAutoDataAttribute()
            : base(() =>
                new Fixture().Customize(new DefaultCustomization()).Customize(new ShortHanderAutoDataCustomization()))
        {
        }
    }

    public class ShortHanderAutoDataCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<HttpClient>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        }
    }
}

