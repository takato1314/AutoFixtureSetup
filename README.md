# AutoFixtureSetup

Extending [AutoFixture](https://github.com/AutoFixture/AutoFixture) to automatically setup and optionally mock all calls on interfaces, abstract and concrete types.
- Uses [IFixtureSetup](https://github.com/takato1314/AutoFixtureSetup/blob/main/src/Core/IFixtureSetup.cs) and [BaseFixtureSetup](https://github.com/takato1314/AutoFixtureSetup/blob/main/src/Core/BaseFixtureSetup.cs) extensibility point that allows users to setup mock fixtures based on fixed values if intended so.
  - Provide options to setup a type either with its actual implementation instance or as a mock type.
  - Implementations of IFixtureSetup will use recursive mocks by default to provide values whenever possible. Default recursion depth is 1.
  - Implementations of IFixtureSetup will freeze it's instance (as singleton) onto the fixture container. This is done so that any other dependent fixture objects will refer to the same instance.
- All properties and methods can be setup with the usual mocking framework's setup method. 
- Provide random values for Nullable types by referring to its base type by default; else you can also pass in `null` value if [desired so via normal assignments](https://github.com/takato1314/AutoFixtureSetup/blob/main/test/unit/AutoFixtureSetup.NSubstitute.Tests/Model/ComplexChildFixture.cs#L154).
  - Nullable types are [not supported](https://github.com/AutoFixture/AutoFixture/issues/731) by AutoFixture by default.
- Resolve all fixture dependencies by retrieving an existing object instance from the fixture container. This is possible with the use of `Fixture.Inject` and `Fixture.Freeze`.
  - If you need to replace the instance of the object generated from the fixture container, you will need to manually do re-assignment after initializing the `FixtureSetup`.

	```csharp
	var fixtureSetup = new ComplexChildFixture(fixture);
	var i1 = fixtureSetup.Valid;
	
	// Replace the auto-generated instance from fixture container
	i1.SimpleChild = new SimpleChild("newSimpleChild");
	
	// Execute operation

	// Assert
	i1.SimpleChild.Name.Should().Be("newSimpleChild");
	```

## Supported Mocking Frameworks

| Framework | Version |
|---|---|
| AutoFixtureSetup.NSubstitute | 4.17.1 |
| AutoFixtureSetup.Moq | 4.17.1 |

## TO DO

- Allow mock types should respect ComposerTransformation.
- Add options to allow users to determine the object lifetime (currently Singleton) of the IFixtureSetup object. The same goes to the [Inject()](https://github.com/takato1314/autofixture_extensions/blob/main/src/Core/Model/BaseFixtureSetup.cs#L43) method.
	- This would allow user to AutoFixture to randomly generate values for each instances without needing user to manually do re-assignment.
- Add options to [turn off recursive mocks](https://stackoverflow.com/questions/21921789/why-does-autofixture-automoq-make-recursive-mocks-by-default#comment33213527_21921789).
