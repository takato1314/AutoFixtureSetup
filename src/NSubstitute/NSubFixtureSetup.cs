using AutoFixture;
using AutoFixtureSetup.Extensions;
using Castle.DynamicProxy;
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

        private static T? _instance;
        /// <inheritdoc />
        protected override Lazy<T> Default => new(() =>
        {
            if (IsMock)
            {
                var ctor = typeof(T).GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault();
                if (ctor == null)
                {
                    throw new InvalidProxyConstructorArgumentsException("Could not find a constructor to initialize object.", null, typeof(T));
                }

                var args = ctor.GetParameters();
                var argObjs = args.Select(arg => Fixture.Freeze(arg.ParameterType)).ToArray();

                _instance ??= Substitute.For<T>(argObjs);
                Fixture.Inject(_instance);
            }

            return Fixture.Freeze<T>();
        });
    }
}
