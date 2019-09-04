using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace UrlShorteningApplication.UnitTests.Infrastructure
{
    public class DefaultCustomization : CompositeCustomization
    {
        public DefaultCustomization() : base(
            new AutoNSubstituteCustomization {ConfigureMembers = true})
        {
        }
    }
}