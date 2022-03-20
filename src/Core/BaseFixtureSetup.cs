using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureSetup.Extensions;

namespace AutoFixtureSetup
{
    /// <summary>
    /// Base implementation of <see cref="IFixtureSetup{T}"/>
    /// </summary>
    public abstract class BaseFixtureSetup<T> : IFixtureSetup<T> where T : class
    {
        /// <inheritdoc cref="BaseFixtureSetup{T}"/>
        protected BaseFixtureSetup(IFixture fixture, bool? isMock = false)
        {
            this.Fixture = fixture;
            this.IsMock = isMock ?? false;
            this.Register();
        }

        #region Properties


        /// <inheritdoc />
        public virtual T Valid => this.Default.Value;

        /// <inheritdoc />
        public virtual T Invalid => this.Default.Value;

        /// <summary>
        /// Indicates if the current created fixture should be a mock or not. <br/>
        /// Default: false
        /// </summary>
        public bool IsMock { get; }

        /// <inheritdoc />
        public Dictionary<Type, IFixtureSetup<dynamic>> Fixtures { get; } = new();

        /// <summary>
        /// Specify the dependencies for this fixture.
        /// </summary>
        protected virtual IEnumerable<Type> Dependencies => Array.Empty<Type>();

        #endregion

        /// <summary>
        /// Returns an object from the <see cref="IFixture"/> container
        /// </summary>
        public TDep Get<TDep>()
        {
            var dep = this.Fixture.Freeze<TDep>();
            return dep;
        }

        /// <summary>
        /// The <see cref="IFixture"/> instance
        /// </summary>
        protected IFixture Fixture { get; }

        /// <summary>
        /// Customization transformation steps.
        /// </summary>
        protected virtual Func<ICustomizationComposer<T>, ISpecimenBuilder>? ComposerTransformation => null;

        /// <summary>
        /// The default fixture object for type <see cref="T"/>. <br/>
        /// Depending on the value of <see cref="IsMock"/>, the created fixture object maybe a mock object or not.
        /// </summary>
        protected virtual Lazy<T> Default => new(() => this.Fixture.Freeze<T>());
        
        /// <summary>
        /// Resolve all dependencies and register <see cref="Valid"/> as the default fixture for type <see cref="T"/>.
        /// </summary>
        private void Register()
        {
            // Register dependencies
            foreach (var dependency in this.Dependencies)
            {
                if (!dependency.ImplementsGenericInterface(typeof(IFixtureSetup<>)))
                {
                    this.Fixture.Freeze(dependency);
                    continue;
                }

                if (Activator.CreateInstance(dependency, this.Fixture, true) is IFixtureSetup<dynamic> fixture)
                {
                    FixtureRegistrar.Inject(this.Fixture, fixture.Valid);
                    this.Fixtures.Add(dependency, fixture);
                    foreach (var depFixture in fixture.Fixtures)
                    {
                        this.Fixtures.Add(depFixture.Key, depFixture.Value);
                    }
                }
            }

            // Register default fixture for T
            if (this.ComposerTransformation != null)
            {
                this.Fixture.Customize(this.ComposerTransformation);
            }
            this.Fixture.Inject(this.Valid);
        }
    }
}
