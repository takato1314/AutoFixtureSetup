using AutoFixture.Xunit2;

namespace AutoFixtureSetup.NSubstitute
{
    /// <inheritdoc />
    /// <remarks>
    /// See https://autofixture.github.io/docs/quick-start/
    /// </remarks>
    public class AutoNSubDataAttribute : AutoDataAttribute
    {
        /// <inheritdoc cref="AutoNSubDataAttribute"/>
        public AutoNSubDataAttribute() : base(() => new NSubFixtureFactory().Create())
        {
        }

        /// <inheritdoc cref="AutoNSubDataAttribute"/>
        public AutoNSubDataAttribute(NSubFixtureOptions options) : base(() => new NSubFixtureFactory(options).Create())
        {
        }
    }
}