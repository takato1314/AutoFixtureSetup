using AutoFixture;

namespace AutoFixtureSetup
{
    /// <summary>
    /// Factory method to declare a single <see cref="IFixture"/> for application tests. <br/>
    /// See https://autofixture.github.io.
    /// </summary>
    public abstract class FixtureFactory
    {
        /// <inheritdoc cref="FixtureFactory"/>
        protected FixtureFactory(IFixtureOptions? options)
        {
            Options = options;
        }

        /// <inheritdoc cref="IFixtureOptions"/>
        public IFixtureOptions? Options { get; set; }

        /// <summary>
        /// Creates an instances of <see cref="IFixture"/>
        /// </summary>
        internal abstract IFixture Create();
    }
}
