using AutoFixture.Xunit2;

namespace UrlShorteningApplication.UnitTests.Infrastructure
{
    public class DefaultInlineAutoDataAttribute : InlineAutoDataAttribute
    {
        public DefaultInlineAutoDataAttribute(params object[] args) : base(new DefaultAutoDataAttribute(), args)
        {
        }
    }
}