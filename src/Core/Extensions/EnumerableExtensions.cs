using System.Globalization;

namespace AutoFixtureSetup.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convert all items in the <see cref="IEnumerable{T}"/> to specified <paramref name="type"/>.
        /// </summary>
        public static IEnumerable<object> ConvertObjectType(this IEnumerable<object> enumerable, Type type)
        {
            return enumerable.Select(v => Convert.ChangeType(v, type, CultureInfo.CurrentCulture));
        }
    }
}
