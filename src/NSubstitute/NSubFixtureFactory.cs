using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixtureSetup.Specification;

namespace AutoFixtureSetup.NSubstitute
{
    /// <inheritdoc />
    internal class NSubFixtureFactory : FixtureFactory
    {
        public NSubFixtureFactory(IFixtureOptions? options) : base(options)
        {
        }

        internal override IFixture Create()
        {
            var fixtureOptions = Options as NSubFixtureOptions ?? NSubFixtureOptions.Default;

            // Extra care required when mocking non-abstract classes. See https://nsubstitute.github.io/help/creating-a-substitute/.
            // Here we use Virtuosity.Fody to make all existing non-abstract public class to be virtual when compiling in Debug mode.
            var nSub = new AutoNSubstituteCustomization
            {
                ConfigureMembers = fixtureOptions.ConfigureMembers,
                GenerateDelegates = fixtureOptions.GenerateDelegates,
                Relay = new SubstituteRelay(new TypeRequestSpecification())
            };
            var fixture = new Fixture().Customize(nSub);
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}