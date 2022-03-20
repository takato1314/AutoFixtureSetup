using AutoFixture;

namespace AutoFixtureSetup
{
    /// <summary>
    /// Setup and seed valid data to be injected into <see cref="AutoFixture"/> container by default. <br/>
    /// To override the default injected type <see cref="T"/> instance, please use the <see cref="FixtureRegistrar.Inject{T}"/>.
    /// </summary>
    public interface IFixtureSetup<out T> where T : class
    {
        /// <summary>
        /// Default valid data.
        /// </summary>
        public T Valid { get; }

        /// <summary>
        /// Default invalid data.
        /// </summary>
        public T Invalid { get; }

        /// <summary>
        /// Indicates if the current created fixture should be a mock or not. <br/>
        /// Default: true
        /// </summary>
        public bool IsMock { get; }

        /// <summary>
        /// Contains all nested <see cref="IFixtureSetup{T}"/> dependencies.
        /// </summary>
        public Dictionary<Type, IFixtureSetup<dynamic>> Fixtures { get; }
    }
}
