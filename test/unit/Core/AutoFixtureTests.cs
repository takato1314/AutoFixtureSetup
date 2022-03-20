using System.Collections.ObjectModel;
using AutoFixture;
using AutoFixtureSetup.Tests.Model;
using FluentAssertions;

namespace AutoFixtureSetup.Tests
{
    public abstract class AutoFixtureTests
    {
        public abstract Task FixtureFactory_ShouldReturnFixture();

        public abstract Task CreateFixtures_ShouldReturnMockImpl(IFixture fixture);

        public abstract Task CreateFixtures_ShouldReturnActualImpl(IFixture fixture);

        public abstract Task CreateFixture_ShouldHaveNoReceivedCalls(IFixture fixture);

        public virtual Task Create_OnNullableTypes_ShouldReturnNonNullableValues(IFixture fixture)
        {
            // Act
            // See https://github.com/AutoFixture/AutoFixture/issues/731
            var intVal = fixture.Create<int?>();
            var stringVal = fixture.Create<int?>();
            var boolVal = fixture.Create<int?>();

            // Assert
            intVal.Should().NotBe(default);
            stringVal.Should().NotBe(default);
            boolVal.Should().NotBe(default);

            return Task.CompletedTask;
        }

        public virtual Task Create_OnStructTypes_ShouldReturnValues(IFixture fixture)
        {
            // Act
            var structChild = fixture.Create<StructChild>();

            // Assert
            structChild.Should().NotBe(default);
            structChild.HasValue.Should().BeTrue();
            structChild.Host.Should().NotBeNullOrEmpty();
            structChild.Port.Should().BeNull();

            return Task.CompletedTask;
        }

        public virtual Task Create_OnDictionaryTypes_ShouldReturnValues(IFixture fixture)
        {
            // Act
            var dictionary = fixture.Create<Dictionary<string, int>>();

            // Assert
            dictionary.Should().NotBeNullOrEmpty();
            dictionary.Keys.Should().NotBeNullOrEmpty();
            dictionary.Values.Should().NotBeNullOrEmpty();

            return Task.CompletedTask;
        }

        public virtual Task Create_OnKeyValuePairTypes_ShouldReturnValues(IFixture fixture)
        {
            // Act
            var kvp = fixture.Create<KeyValuePair<string, int>>();

            // Assert
            kvp.Should().NotBeNull();
            kvp.Key.Should().NotBeNullOrEmpty();
            kvp.Value.Should().BeGreaterThan(0);

            return Task.CompletedTask;
        }

        public virtual Task FreezeAndInject_ShouldReturnCorrectInstances(IFixture fixture)
        {
            var i1 = new SimpleChild
            {
                Number = 10,
                Name = "RandomText10"
            };
            var i2 = fixture.Freeze<SimpleChild>();
            var i3 = fixture.Create<SimpleChild>();

            // Before injection 
            i1.Should().NotBeSameAs(i2);
            i1.Should().NotBeSameAs(i3);
            i2.Should().BeSameAs(i3);

            // After injection
            fixture.Inject(i1);
            var i4 = fixture.Freeze<SimpleChild>();
            var i5 = fixture.Create<SimpleChild>();
            i1.Should().NotBeSameAs(i2);
            i1.Should().NotBeSameAs(i3);
            i2.Should().NotBeSameAs(i4);
            i2.Should().NotBeSameAs(i5);
            i3.Should().NotBeSameAs(i4);
            i3.Should().NotBeSameAs(i5);
            i1.Should().BeSameAs(i4);
            i1.Should().BeSameAs(i5);
            i2.Should().BeSameAs(i3);
            i4.Should().BeSameAs(i5);

            // Since i6 == i1, changing its properties also changes other references (After injection)
            // Since i2 and i3 are create separately (Before injection), they should not be affected
            var i6 = fixture.Create<SimpleChild>();
            i4.Should().BeSameAs(i6);
            i5.Should().BeSameAs(i6);
            i6.Number = 20;
            i6.Name = "RandomText20";
            i2.Should().NotBeSameAs(i6);
            i3.Should().NotBeSameAs(i6);
            i1.Should().BeSameAs(i6);
            i4.Should().BeSameAs(i6);
            i5.Should().BeSameAs(i6);

            // Since i7 is now injected with custom object, it should differ from all of the previously created objects
            fixture.Inject(new SimpleChild
            {
                Number = 30,
                Name = "RandomText30"
            });
            var i7 = fixture.Create<SimpleChild>();
            i1.Should().NotBeSameAs(i7);
            i2.Should().NotBeSameAs(i7);
            i3.Should().NotBeSameAs(i7);
            i4.Should().NotBeSameAs(i7);
            i5.Should().NotBeSameAs(i7);
            i6.Should().NotBeSameAs(i7);

            return Task.CompletedTask;
        }

        public virtual Task FreezeAndCreateSequences_ShouldReturnSameInstances(IFixture fixture)
        {
            var seq = fixture.Freeze<IEnumerable<int>>().ToList();
            var list = fixture.Create<List<int>>();
            var iList = fixture.Create<IList<int>>();
            var collection = fixture.Create<Collection<int>>();

            seq.Should().NotBeEmpty();
            seq.Should().BeEquivalentTo(list);
            seq.Should().BeEquivalentTo(iList);
            seq.Should().BeEquivalentTo(collection);

            return Task.CompletedTask;
        }
    }
}
