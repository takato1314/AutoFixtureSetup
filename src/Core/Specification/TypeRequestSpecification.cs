using AutoFixture.Kernel;

namespace AutoFixtureSetup.Specification
{
    internal class TypeRequestSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            return request is Type;
        }
    }
}
