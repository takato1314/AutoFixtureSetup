namespace AutoFixtureSetup.Tests.Model
{
    /// <inheritdoc cref="IHasProperties"/>
    public abstract class HasProperties : IHasProperties
    {
        /// <inheritdoc />
        public abstract string Name { get; set; }

        /// <inheritdoc />
        public abstract int Number { get; set; }

        /// <inheritdoc />
        public abstract Guid ConcurrencyStamp { get; set; }

        /// <inheritdoc />
        public virtual string ReturnMethod()
        {
            throw new NotImplementedException("Not implemented on base");
        }

        /// <inheritdoc />
        public virtual void VoidMethod()
        {
            throw new NotImplementedException("Not implemented on base");
        }
    }
}
