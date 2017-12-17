_This library is a work in progress. Existing functionality is pretty much done, but public interfaces may change in the future._

`SimpleValueObjects` is a .NET Standard library, that aims to provide a toolkit for simple creation of Value Objects.

I believe such library can be useful, since .NET doesn't make implementation of proper Value Objects easy, with all it's nulls, operator overloading, non-generic methods overriding, etc. In other words, creating a valid value object by hand is tricky and can be quite a hassle, not to mention code and test duplication it produces.

This library means to cover cases I've found most common when developing applications.

<!-- include some "just show me the code" example -->

# Value Objects

A Value Object is an object:
* that is immutable, and
* whose equality is based on value, rather than identity.

This seemingly simple pattern has a wide range of applications. Actually, if you think about it, you're probably using ones on a daily basis - think of `string` or `DateTime`. Other popular examples are amounts of money, date ranges or geographical coordinates.

Domain Driven Design promotes using Value Object for representation of domain concepts, e. g. user names, product prices or ZIP codes.

# Using `SimpleValueObjects`

## Equitable Value Objects

### `AutoEquitableObject`

In most cases, a value object cosists of a few fields. Obviously, we consider two such objects equal, when their field values are also equal.

When you inherit from `AutoEquitableObject`, it will autimatically implement equality in a way, that two objects will be equal when values of their fields are equal.

What do you need to do:
* inherit from `AutoEquitableObject`.

What do you get:
* consistent equality comparison based on its fields' values,
* overloaded `==` and `!=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>.Equals` implementation,
* null handling: no value is equal to null and two nulls are always equal,
* type handling: different types are never equal,
* generating hash code based on its fields' values.

Sample applications:
* `DateTime` range, which consists of from and to dates,
* money object, which consists of currency and amount.

Remarks:
* `AutoEquitableObject` uses reflection to get fields' values, which means it might be slow at times. I believe in most cases that is not a huge problem, however if you use your Value Object extensively you might want to use `EquitableObject`, which let's you implement equality comparison by hand.
* Also, `AutoEquitableObject` _doesn't perform deep comparison_. It means, that if you want your value object to be more structured, you should either compose it from other value objects, or extend `EquitableObject` instead and implement equality comparison by hand.

### `WrapperEquitableObject`

Sometimes you'll want to use some more common Value Object, like `string` or `int`, and give them additional context. For instance, user name in a system can just be a string, but with limited length and consisting only of alphanumeric characters. Also, it makes sure, that noone uses "just any string" instead of a valid username by accident.

If that's the case, you can use `WrapperEquitableObject`. This way, equality comparison and hash code generation will work exactly like in a wrapped object.

What do you need to do:
* inherit from `WrapperEquitableObject`,
* pass some value to a base constructor.

What do you get:
* consistend equality comparison based on wrapped value,
* generating hash code based on wrapped value,
* overloaded == and != operators,
* overridden `Object.Equals`,
* `IEquitable<T>.Equals` implementation,
* null handling: no value is equal to null and two nulls are always equal,
* type handling: different types are never equal.

Remarks:
* `WrapperEquitableObject` _doesn't perform deep comparison_. It means, that wrapped type should also be a Value Object.

### `EquitableObject`

When you want to implement equality comparison logic by yourself, you can use `EquitableObject`. You only need to implement equality comparison and generating hash code once, and it will become consistant across different usages.

What do you need to do:
* inherit from `EquitableObject`,
* implement `EqualsToNull`,
* implement `GenerateHashCode`, (see _Generating hash codes_ section).

What do you get:
* overloaded `==` and `!=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>.Equals` implementation,
* null handling: no value is equal to null and two nulls are always equal,
* type handling: different types are never equal.

## Comparable Value Objects

Sometimes we want values that not only can be determined equal, but also can be placed in a certain order. Think of values like temperature, user rating or month. We can certainly say, that values of such types can be lesser, equal or greater that another values, e. g. 5 star rating is greater than 3 stars, or February comes after January.

In .NET such concept can be represented by:
* `IComparable` and `IComparable<T>` implementations and
* relational operators, i. e. `<`, `<=`, `>` and `>=`.

Following base classes do just that: implement `IComparable` interfaces and overload rlational operators. Besides that, they handle equality in similar fashon as previous base classes.

### `WrapperComparableObject`

Like with `WrapperEquitableObject`, sometimes you'll want to wrap a simpler value to give it additional context. You can do that, if wrapped type implements `IComparable<T>`, which then will be used for comparison.

What do you need to do:
* inherit from `WrapperComparableObject`,
* pass some value to a base constructor.

What do you get:
* overloaded `<`, `<=`, `==`, `!=`, `>` and `>=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>`, `IComparable` and `IComparable<T>` implementations,
* null handling: every value is greater than null, no value is equal to null and two nulls are always equal,
* type handling: different types are never equal and comparing objects of different types will throw an exception.

### `ComparableObject`

With `ComparableObject` you only implement comparison once, and all comparison and equality comparison logic are handled consistencly. Again, you have to implement generating hash code yourself.

What do you need to do:
* inherit from `ComparableObject`,
* implement `CompareToNotNull`,
* implement `GenerateHashCode` (see _Generating hash codes_ section)

What do you get:
* overloaded `<`, `<=`, `==`, `!=`, `>` and `>=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>`, `IComparable` and `IComparable<T>` implementations,
* null handling: every value is greater than null, no value is equal to null and two nulls are always equal,
* type handling: different types are never equal and comparing objects of different types will throw an exception.

# Generating hash codes

Sometimes you'll need to generate a hash code by hand, e.g. when extending `EquitableObject` or `ComparableObject`. If you just want to generate a hash from a bunch of different values, you can use `HashCodeCalculator.CalculateFromValues` method.

_Remember, that when to Value Objects are equal, their hash code should also be equal._

Also, it might be worth checking out some offical souces:

https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netframework-4.7.1#Remarks

# Resources

Some interesting resouces related to Value Objects:
* https://martinfowler.com/bliki/ValueObject.html
* http://wiki.c2.com/?ValueObject
* https://refactoring.guru/smells/primitive-obsession