using NSubstitute.Core;

namespace AutoFixtureSetup.NSubstitute.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Checks if <see cref="obj"/> is mocked with NSubstitute.<br/>
        /// See https://github.com/nsubstitute/NSubstitute/issues/659#issuecomment-900071159.
        /// </summary>
        public static bool IsSubstitute(this object obj)
        {
            return obj is ICallRouterProvider;
        }
    }
}
