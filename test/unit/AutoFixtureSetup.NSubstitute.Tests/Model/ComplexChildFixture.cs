using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;
using AutoFixtureSetup.NSubstitute.Extensions;
using AutoFixtureSetup.Tests.Model;
using FluentAssertions;
using Xunit;

namespace AutoFixtureSetup.NSubstitute.Tests.Model
{
    public class ComplexChildFixture : NSubFixtureSetup<ComplexChild>
    {
        /// <inheritdoc />
        public ComplexChildFixture(IFixture fixture, bool? isMock = null) : base(fixture, isMock)
        {
        }

        // For unit test expectations
        internal static readonly SimpleChild SimpleChild = new(nameof(SimpleChild), 100);
        internal static readonly ComplexChild Instance = new()
        {
            Name = "OverridenComplexChild",
            Number = 3,
            ConcurrencyStamp = new Guid("ac8fd90c-f84e-45ff-88b7-0a971db1ddff"),
            Boolean = false,
            Nullable = 123,
            Function = _ => "FixtureSetupFunction",
            StringCollection = new List<string>(),
            DictionaryCollection = new Dictionary<string, SimpleChild>
            {
                { nameof(SimpleChild), SimpleChild }
            }
        };

        protected override IEnumerable<Type> Dependencies => new[] { typeof(SimpleChildFixture) };

        protected override Func<ICustomizationComposer<ComplexChild>, ISpecimenBuilder> ComposerTransformation =>
            composer =>
            {
                var postProcess = composer
                    .With(_ => _.Name, Instance.Name)
                    .With(_ => _.Number, Instance.Number)
                    .With(_ => _.ConcurrencyStamp, Instance.ConcurrencyStamp)
                    .With(_ => _.Boolean, Instance.Boolean)
                    .With(_ => _.Nullable, Instance.Nullable)
                    .With(_ => _.Function, Instance.Function)
                    .With(_ => _.StringCollection, Instance.StringCollection)
                    .With(_ => _.DictionaryCollection, Instance.DictionaryCollection);

                return postProcess;
            };
    }

    public class ComplexChildFixtureTest
    {
        [Theory, AutoNSubData]
        public Task FixtureSetup_ShouldReturnConcreteType(IFixture fixture)
        {
            // Arrange
            var sut = new ComplexChildFixture(fixture);

            // Act
            var i0 = new ComplexChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<ComplexChild>();

            // Assert
            i1.Should().NotBeNull();
            i1.Should().NotBeEquivalentTo(i0);
            i1.Should().BeSameAs(i2);

            // Should be a mock
            var instances = new List<ComplexChild> { i1, i2 };
            sut.IsMock.Should().BeFalse();
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeFalse();
                instance.Should().BeEquivalentTo(ComplexChildFixture.Instance);

                instance.Name.Should().Be("OverridenComplexChild");
                instance.Number.Should().Be(3);
                instance.Nullable.Should().Be(123);
                instance.ConcurrencyStamp.Should().Be("ac8fd90c-f84e-45ff-88b7-0a971db1ddff");
                instance.StringCollection.Should().BeEmpty();
                instance.DictionaryCollection[nameof(SimpleChild)].Should().NotBeEquivalentTo(sut.Fixtures[typeof(SimpleChildFixture)].Valid);
                instance.DictionaryCollection[nameof(SimpleChild)].Should().Be(ComplexChildFixture.SimpleChild);

                instance.Function(string.Empty).Should().Be("FixtureSetupFunction");
                instance.Invoking(_ => _.ReturnMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on ComplexChild");
                instance.Invoking(_ => _.VoidMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on ComplexChild");
            }

            return Task.CompletedTask;
        }

        [Theory(Skip = "Substitute should process ComposerTransformation"), AutoNSubData]
        public Task FixtureMockSetup_ShouldReturnMockType(IFixture fixture)
        {
            // Arrange
            var sut = new ComplexChildFixture(fixture, true);

            // Act
            var i0 = new ComplexChild();
            var i1 = sut.Valid;
            var i2 = fixture.Create<ComplexChild>();

            // Assert
            var instances = new List<ComplexChild> { i1, i2 };
            sut.IsMock.Should().BeTrue();
            foreach (var instance in instances)
            {
                instance.Should().NotBeNull();
                instance.Should().NotBeEquivalentTo(i0);
                instance.IsSubstitute().Should().BeTrue();
                instance.Should().BeEquivalentTo(ComplexChildFixture.Instance);

                instance.Name.Should().Be("OverridenComplexChild");
                instance.Number.Should().Be(3);
                instance.Nullable.Should().Be(123);
                instance.ConcurrencyStamp.Should().Be("ac8fd90c-f84e-45ff-88b7-0a971db1ddff");
                instance.StringCollection.Should().BeEmpty();
                instance.DictionaryCollection[nameof(SimpleChild)].Should().NotBeEquivalentTo(sut.Fixtures[typeof(SimpleChildFixture)].Valid);
                instance.DictionaryCollection[nameof(SimpleChild)].Should().Be(ComplexChildFixture.SimpleChild);

                instance.Function(string.Empty).Should().Be("FixtureSetupFunction");
                instance.ReturnMethod().Should().Be(instance.Name);
                instance.Invoking(_ => _.VoidMethod()).Should().NotThrow();
            }

            return Task.CompletedTask;
        }

        [Theory, AutoNSubData]
        public Task FixtureSetup_OverrideData_ShouldReturnOverridenData(IFixture fixture)
        {
            // Arrange
            var sut = new ComplexChildFixture(fixture);

            // Act
            var i1 = sut.Valid;
            var i2 = fixture.Create<ComplexChild>();

            i1.Name = "OverridenText";
            i1.Number = 2;
            i1.ConcurrencyStamp = new Guid("6f55a677-c447-45f0-8e71-95c7b73fa889");
            i1.Nullable = 456;

            // Assert
            var instances = new List<ComplexChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.IsSubstitute().Should().BeFalse();
                instance.Should().BeOfType<ComplexChild>();

                instance.Name.Should().Be("OverridenText");
                instance.Number.Should().Be(2);
                instance.Nullable = 456;
                instance.ConcurrencyStamp.ToString().Should().Be("6f55a677-c447-45f0-8e71-95c7b73fa889");
                instance.StringCollection.Should().BeEmpty();
                instance.DictionaryCollection[nameof(SimpleChild)].Should().NotBeEquivalentTo(sut.Fixtures[typeof(SimpleChildFixture)].Valid);
                instance.DictionaryCollection[nameof(SimpleChild)].Should().Be(ComplexChildFixture.SimpleChild);

                instance.Function(string.Empty).Should().Be("FixtureSetupFunction");
                instance.Invoking(_ => _.ReturnMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on ComplexChild");
                instance.Invoking(_ => _.VoidMethod())
                    .Should()
                    .Throw<NotImplementedException>()
                    .WithMessage("Not implemented on ComplexChild");
            }

            return Task.CompletedTask;
        }

        [Theory(Skip = "Substitute should process ComposerTransformation"), AutoNSubData]
        public Task FixtureMockSetup_OverrideData_ShouldReturnOverridenData(IFixture fixture)
        {
            // Arrange
            var sut = new ComplexChildFixture(fixture, true);

            // Act
            var i1 = sut.Valid;
            var i2 = fixture.Create<ComplexChild>();

            i1.Name = "OverridenText";
            i1.Number = 2;
            i1.ConcurrencyStamp = new Guid("6f55a677-c447-45f0-8e71-95c7b73fa889");
            i1.Nullable = 456;

            // Assert
            var instances = new List<ComplexChild> { i1, i2 };
            foreach (var instance in instances)
            {
                instance.IsSubstitute().Should().BeTrue();
                instance.Should().NotBeOfType<ComplexChild>();
                instance.Name.Should().Be("OverridenText");
                instance.Number.Should().Be(2);
                instance.Nullable = 456;
                instance.ConcurrencyStamp.ToString().Should().Be("6f55a677-c447-45f0-8e71-95c7b73fa889");
                instance.StringCollection.Should().BeEmpty();
                instance.DictionaryCollection[nameof(SimpleChild)].Should().NotBeEquivalentTo(sut.Fixtures[typeof(SimpleChildFixture)].Valid);
                instance.DictionaryCollection[nameof(SimpleChild)].Should().Be(ComplexChildFixture.SimpleChild);

                instance.Function(string.Empty).Should().Be("FixtureSetupFunction");
                instance.Invoking(_ => _.ReturnMethod()).Should().NotThrow();
                instance.Invoking(_ => _.VoidMethod()).Should().NotThrow();
            }

            return Task.CompletedTask;
        }
    }
}