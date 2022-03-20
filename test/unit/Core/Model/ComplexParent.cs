namespace AutoFixtureSetup.Tests.Model
{
    public class ComplexParent : IHasProperties
    {
        #region ctor
        
        public ComplexParent(ComplexChild complexChild)
        {
            this.ComplexChild = complexChild;
        }

        public ComplexParent(ComplexChild complexChild, SimpleChild simpleChild)
        {
            this.ComplexChild = complexChild;
            this.SimpleChild = simpleChild;
        }

        #endregion

        public ComplexChild ComplexChild { get; }

        public SimpleChild SimpleChild { get; set; } = null!;

        public StructChild StructChild { get; set; }

        /// <inheritdoc />
        public string Name { get; set; } = nameof(ComplexChild);

        /// <inheritdoc />
        public int Number { get; set; }

        /// <inheritdoc />
        public Guid ConcurrencyStamp { get; set; }
        
        /// <inheritdoc />
        public virtual string ReturnMethod()
        {
            throw new NotImplementedException("Not implemented on ComplexParent");
        }

        /// <inheritdoc />
        public virtual void VoidMethod()
        {
            throw new NotImplementedException("Not implemented on ComplexParent");
        }
    }
}
