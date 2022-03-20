using AutoFixture;
using NSubstitute;

namespace AutoFixtureSetup.NSubstitute
{
    /// <summary>
    /// NSubstitute implementation of <see cref="IFixtureSetup{T}"/>
    /// </summary>
    public abstract class NSubFixtureSetup<T> : BaseFixtureSetup<T> where T : class
    {
        /// <inheritdoc cref="NSubFixtureSetup{T}"/>
        protected NSubFixtureSetup(IFixture fixture, bool? isMock) : base(fixture, isMock)
        {
        }
        
        /// <inheritdoc />
        protected override Lazy<T> Default => new(() =>
        {
            if (this.IsMock)
            {
                this.Fixture.Inject(Substitute.For<T>());
                return this.Fixture.Freeze<T>();
            }

            return this.Fixture.Freeze<T>();
        });
    }
}
