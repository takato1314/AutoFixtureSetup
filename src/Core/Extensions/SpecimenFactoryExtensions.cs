using AutoFixture;
using AutoFixture.Kernel;

namespace AutoFixtureSetup.Extensions
{
    /// <inheritdoc cref="SpecimenFactory"/>
    public static class SpecimenFactoryExtensions
    {
        /// <inheritdoc cref="SpecimenFactory.Create{T}(ISpecimenContext)"/>
        public static object Create(this ISpecimenContext context, Type type)
        {
            return context != null ? 
                context.Resolve(new SeededRequest(type, default)) : 
                throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc cref="SpecimenFactory.Create{T}(ISpecimenBuilder)"/>
        public static object Create(this ISpecimenBuilder builder, Type type)
        {
            return builder.CreateContext().Resolve(type);
        }

        /// <inheritdoc cref="SpecimenFactory.CreateMany{T}(ISpecimenContext)"/>
        public static IEnumerable<object> CreateMany(ISpecimenContext context, Type type)
        {
            return ((IEnumerable<object>)context
                    .Resolve(new MultipleRequest(new SeededRequest(type, type.GetDefaultValue()))))
                .ConvertObjectType(type);
        }

        /// <summary>
        /// Factory method for creating <see cref="ISpecimenContext"/>.
        /// </summary>
        private static ISpecimenContext CreateContext(this ISpecimenBuilder builder)
        {
            return new SpecimenContext(builder);
        }
    }
}
