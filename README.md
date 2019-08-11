## PasswordGeneratorSharp
A basic password generator library made in C#

### Usage

To immediately start using it, create a new Instance of a `PasswordGenerator` and call the `Generate()` function from it.

```
using PasswordGeneratorSharp

// ...

PasswordGenerator generator = new PasswordGenerator();
string newPassword = generator.Generate();
```

Though, it will create a password using default properties.<br>
You can modify the kind of passwords it can generator by using an instance of `GenerationProperties`, and passing it into the constructor of `PasswordGenerator` like so:

```
GenerationProperties passGenerationProperties = new GenerationProperties {
    MinimumNumberOfLowerCaseCharacters = 1,
    MinimumNumberOfNumericCharacters = 1,
    MinimumNumberOfUpperCaseCharacters = 1,
    MinimumNumberOfSpecialCharacters = 1,
    PasswordLength = 30,
    SpecialCharacters = new char[3] { '*', '&', '(' }
};
// Alternative to the above is to use the other overload constructor.
// Which you can also pass in a 'RandomNumberGenerator'

PasswordGenerator generator = new PasswordGenerator(passGenerationProperties);
```

You can set the [`RandomNumberGenerator`](https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=netframework-4.8) of the `GenerationProperties` using `SetRandomNumberGenerator(RandomNumberGenerator)` on the instance.

>*Note*: Changing the properties in `GenerationProperties` will affect the respective `PasswordGenerator` that was constructed using the instance of that `GenerationProperties`.<br>Though this does not apply to `SpecialCharacters` and `RandomNumberGenerator` property.
