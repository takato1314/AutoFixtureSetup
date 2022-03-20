using AutoFixture;
using EnsureThat;

namespace AutoFixtureSetup.Extensions
{
    /// <inheritdoc cref="FixtureFreezer"/>
    public static class FixtureFreezerExtensions
    {
        /// <inheritdoc cref="FixtureFreezer.Freeze{T}(IFixture)"/>
        public static object Freeze(this IFixture fixture, Type type)
        {
            Ensure.That(fixture).IsNotNull();

            var value = fixture.Create(type);
            fixture.Inject(value);

            return value;
        }
    }
}
