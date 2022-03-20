namespace AutoFixtureSetup.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get the default value of the specified <paramref name="type"/>. <br/>
        /// See https://stackoverflow.com/a/353073/3104587.
        /// </summary>
        public static object? GetDefaultValue(this Type type)
        {
            return type.IsValueType
                ? Activator.CreateInstance(type)
                : null;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="type"/> implements the <paramref name="interfaceType"/>.
        /// </summary>
        public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return GetGenericInterface(type, interfaceType) != null;
        }

        /// <summary>
        /// Get the specified <paramref name="genericInterface"/> from the <paramref name="type"/>.
        /// </summary>
        public static Type? GetGenericInterface(this Type type, Type genericInterface)
        {
            return type.IsGenericType(genericInterface)
                ? type
                : type.GetInterfaces().FirstOrDefault(t => t.IsGenericType(genericInterface));
        }

        /// <summary>
        /// Determines if <paramref name="type"/> is a specified <paramref name="genericType"/>.
        /// </summary>
        public static bool IsGenericType(this Type type, Type genericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
        }
    }
}
