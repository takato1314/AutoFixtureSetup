namespace AutoFixtureSetup.Tests.Model
{
    public class SimpleChild : HasProperties
    {
        #region ctor

        public SimpleChild()
        {
        }

        public SimpleChild(string name)
        {
            Name = name;
        }

        public SimpleChild(string name, int number)
        {
            Name = name;
            Number = number;
        }

        #endregion

        /// <inheritdoc />
        public override string Name { get; set; } = nameof(SimpleChild);

        /// <inheritdoc />
        public override int Number { get; set; } = 123;

        /// <inheritdoc />
        public override Guid ConcurrencyStamp { get; set; } = new("e8ccdea7-df59-49ff-8109-03dfb6f798e8");
    }
}
