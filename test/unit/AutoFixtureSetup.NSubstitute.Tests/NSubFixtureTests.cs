using AutoFixture;
using AutoFixtureSetup.NSubstitute.Extensions;
using AutoFixtureSetup.NSubstitute.Tests.Model;
using AutoFixtureSetup.Tests;
using AutoFixtureSetup.Tests.Model;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AutoFixtureSetup.NSubstitute.Tests
{
    public class NSubFixtureTests : AutoFixtureTests
    {
        [Fact]
        public override Task FixtureFactory_ShouldReturnFixture()
        {
            var fixture = new NSubFixtureFactory().Create();

            fixture.Should().NotBeNull();
            fixture.Should().BeOfType<Fixture>();

            return Task.CompletedTask;
        }

        [Theory]
        [AutoNSubData]
        public override Task CreateFixtures_ShouldReturnMockImpl(IFixture fixture)
        {
            // Interface types are always mocked
            var interfaceType = fixture.Create<IHasProperties>();
            interfaceType.IsSubstitute().Should().BeTrue();

            // Abstract types are always mocked
            var abstractType = fixture.Create<HasProperties>();
            abstractType.IsSubstitute().Should().BeTrue();

            // Concrete types are mocks in this case
            var concreteType = new ComplexChildFixture(fixture, true).Valid;
            concreteType.IsSubstitute().Should().BeTrue();
            //concreteType.DictionaryCollection[nameof(SimpleChild)].IsSubstitute().Should().BeTrue();
            concreteType.Should().NotBeOfType<ComplexChild>();

            return Task.CompletedTask;
        }

        [Theory]
        [AutoNSubData]
        public override Task CreateFixtures_ShouldReturnActualImpl(IFixture fixture)
        {
            // Interface types are always mocked.
            var interfaceType = fixture.Create<IHasProperties>();
            interfaceType.IsSubstitute().Should().BeTrue();

            // Abstract types are always mocked.
            var abstractType = fixture.Create<HasProperties>();
            abstractType.IsSubstitute().Should().BeTrue();

            // Concrete types should not be mock when act as sut.
            var concreteType = new ComplexChildFixture(fixture).Valid;
            concreteType.IsSubstitute().Should().BeFalse();
            concreteType.DictionaryCollection[nameof(SimpleChild)].IsSubstitute().Should().BeFalse();
            concreteType.Should().BeOfType<ComplexChild>();

            return Task.CompletedTask;
        }

        [Theory]
        [AutoNSubData]
        public override Task CreateFixture_ShouldHaveNoReceivedCalls(IFixture fixture)
        {
            // Act
            var userFixture = new SimpleChildFixture(fixture, true);

            // Assert
            var simpleChild = fixture.Create<SimpleChild>();
            var valid = userFixture.Valid;
            var invalid = userFixture.Invalid;
            var instances = new[] { simpleChild, valid, invalid };

            foreach (var instance in instances)
            {
                instance.IsSubstitute().Should().BeTrue();
                instance.Received(0).ReturnMethod();
                instance.Received(0).VoidMethod();
            }

            return Task.CompletedTask;
        }

        [Theory]
        [AutoNSubData]
        public override Task Create_OnStructTypes_ShouldReturnValues(IFixture fixture)
        {
            return base.Create_OnStructTypes_ShouldReturnValues(fixture);
        }

        [Theory]
        [AutoNSubData]
        public override Task Create_OnNullableTypes_ShouldReturnNonNullableValues(IFixture fixture)
        {
            return base.Create_OnNullableTypes_ShouldReturnNonNullableValues(fixture);
        }

        [Theory]
        [AutoNSubData]
        public override Task Create_OnDictionaryTypes_ShouldReturnValues(IFixture fixture)
        {
            return base.Create_OnDictionaryTypes_ShouldReturnValues(fixture);
        }

        [Theory]
        [AutoNSubData]
        public override Task Create_OnKeyValuePairTypes_ShouldReturnValues(IFixture fixture)
        {
            return base.Create_OnKeyValuePairTypes_ShouldReturnValues(fixture);
        }

        [Theory]
        [AutoNSubData]
        public override Task FreezeAndInject_ShouldReturnCorrectInstances(IFixture fixture)
        {
            return base.FreezeAndInject_ShouldReturnCorrectInstances(fixture);
        }

        [Theory]
        [AutoNSubData]
        public override Task FreezeAndCreateSequences_ShouldReturnSameInstances(IFixture fixture)
        {
            return base.FreezeAndCreateSequences_ShouldReturnSameInstances(fixture);
        }
    }
}
