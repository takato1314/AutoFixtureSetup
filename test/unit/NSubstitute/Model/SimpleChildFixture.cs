using AutoFixture;
using AutoFixtureSetup.NSubstitute.Extensions;
using AutoFixtureSetup.Tests.Model;
using FluentAssertions;
using Xunit;
using NSubstitute;
using NSubstitute.Extensions;

namespace AutoFixtureSetup.NSubstitute.Tests.Model
{
    public class SimpleChildFixture : NSubFixtureSetup<SimpleChild>
    {
        /// <inheritdoc />
        public SimpleChildFixture(IFixture fixture, bool? isMock = null) : base(fixture, isMock)
        {
        }

        public override SimpleChild Valid
        {
            get
            {
                // Configure the fixture object here
                var obj = base.Valid;
                obj.Name = "validSimpleChild";
                obj.Number = 1;
                obj.ConcurrencyStamp = new Guid("6041611a-40cc-4e49-8a20-407da6ef36b9");
                if (IsMock)
                {
                    obj.Configure().ReturnMethod().Returns(_ => obj.Name);
                    obj.Configure().VoidMethod();
                }

                // Configure dependencies behaviour here

                return obj;
            }
        }

        public override SimpleChild Invalid
        {
            get
            {
                // Configure the fixture object here
                var obj = base.Invalid;
                obj.Name = "invalidSimpleChild";
                obj.ConcurrencyStamp = new Guid("5b2a446e-930b-44a0-be6d-457a5ec08a27");

                // Configure dependencies behaviour here

                return obj;
            }
        }
    }

    public class SimpleChildFixtureTest
    {
        [Theory, AutoNSubData]
        public Task FixtureSetup_ShouldReturnConcreteType(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<SimpleChild>();

            // Assert
            i1.Should().BeSameAs(i2);
            sut.IsMock.Should().BeFalse();
            var instances = new List<SimpleChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeFalse();
                instance.Should().BeOfType<SimpleChild>();

                instance.Name.Should().Be("validSimpleChild");
                instance.Number.Should().Be(1);
                instance.ConcurrencyStamp.Should().Be("6041611a-40cc-4e49-8a20-407da6ef36b9");
                instance.Invoking(_ => _.ReturnMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on base");
                instance.Invoking(_ => _.VoidMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on base");
            }

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task FixtureMockSetup_ShouldReturnMockType(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture, true);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<SimpleChild>();

            // Assert
            i1.Should().BeSameAs(i2);
            sut.IsMock.Should().BeTrue();
            var instances = new List<SimpleChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeTrue();
                instance.Should().NotBeOfType<SimpleChild>();

                instance.Name.Should().Be("validSimpleChild");
                instance.Number.Should().Be(1);
                instance.ConcurrencyStamp.Should().Be("6041611a-40cc-4e49-8a20-407da6ef36b9");
                instance.ReturnMethod().Should().Be(instance.Name);
                instance.Invoking(_ => _.VoidMethod()).Should().NotThrow();
            }

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task FixtureSetup_OverrideData_ShouldReturnOverridenData(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<SimpleChild>();

            i1.Name = "OverridenText";
            i1.Number = 2;
            i1.ConcurrencyStamp = new Guid("6f55a677-c447-45f0-8e71-95c7b73fa889");

            // Assert
            i1.Should().BeSameAs(i2);
            sut.IsMock.Should().BeFalse();
            var instances = new List<SimpleChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeFalse();
                instance.Should().BeOfType<SimpleChild>();

                instance.Name.Should().Be("OverridenText");
                instance.Number.Should().Be(2);
                instance.ConcurrencyStamp.ToString().Should().Be("6f55a677-c447-45f0-8e71-95c7b73fa889");
                instance.Invoking(_ => _.ReturnMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on base");
                instance.Invoking(_ => _.VoidMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on base");
            }

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task FixtureMockSetup_OverrideData_ShouldReturnOverridenData(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture, true);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<SimpleChild>();

            i1.Name = "OverridenText";
            i1.Number = 2;
            i1.ConcurrencyStamp = new Guid("6f55a677-c447-45f0-8e71-95c7b73fa889");

            // Assert
            i1.Should().BeSameAs(i2);
            sut.IsMock.Should().BeTrue();
            var instances = new List<SimpleChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeTrue();
                instance.Should().NotBeOfType<SimpleChild>();

                instance.Name.Should().Be("OverridenText");
                instance.Number.Should().Be(2);
                instance.ConcurrencyStamp.ToString().Should().Be("6f55a677-c447-45f0-8e71-95c7b73fa889");
                instance.ReturnMethod().Should().Be(instance.Name);
                instance.Invoking(_ => _.VoidMethod()).Should().NotThrow();
            }

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task GetDependency_ShouldReturnSameConcreteType(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = sut.Get<SimpleChild>();

            // Assert
            i2.IsSubstitute().Should().BeFalse();
            i2.Should().NotBeEquivalentTo(i0);
            i2.Should().BeSameAs(i1);
            i2.Should().BeOfType<SimpleChild>();

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task GetDependency_ShouldReturnSameMockType(IFixture fixture)
        {
            // Arrange
            var sut = new SimpleChildFixture(fixture, true);

            // Act
            var i0 = new SimpleChild();
            var i1 = sut.Valid;
            var i2 = sut.Get<SimpleChild>();

            // Assert
            i2.IsSubstitute().Should().BeTrue();
            i2.Should().NotBeEquivalentTo(i0);
            i2.Should().BeSameAs(i1);
            i2.Should().NotBeOfType<SimpleChild>();

            return Task.CompletedTask;
        }
    }
}