namespace AutoFixtureSetup.Tests.Model
{
    /// <summary>
    /// A simple interface used to test AutoFixture features to ensure there are no breaking changes.
    /// </summary>
    public interface IHasProperties
    {
        /// <summary>
        /// Normal string
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Normal integer
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Default implementation for Guid
        /// </summary>
        Guid ConcurrencyStamp { get; set; }

        public string ReturnMethod();
        
        public void VoidMethod();
    }
}
