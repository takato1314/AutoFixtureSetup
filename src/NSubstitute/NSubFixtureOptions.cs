using AutoFixture;

namespace AutoFixtureSetup.NSubstitute
{
    /// <inheritdoc />
    public class NSubFixtureOptions : IFixtureOptions
    {
        /// <summary>
        /// The default fixture options.
        /// </summary>
        public static NSubFixtureOptions Default = new()
        {
            ConfigureMembers = true,
            GenerateDelegates = true
        };

        /// <summary>
        /// Specifies whether members of a substitute will be automatically configured to retrieve the return values from a fixture.
        /// </summary>
        public bool ConfigureMembers { get; set; }

        /// <summary>
        /// If value is <c>true</c>, delegate requests are intercepted and created by NSubstitute.
        /// Otherwise, if value is <c>false</c>, delegates are created by the AutoFixture kernel.
        /// </summary>
        public bool GenerateDelegates { get; set; }
    }
}
