# SimpleValueObjects

[![AppVeyor](https://img.shields.io/appveyor/ci/redss/simplevalueobjects.svg)](https://ci.appveyor.com/project/redss/simplevalueobjects)
[![NuGet](https://img.shields.io/nuget/v/SimpleValueObjects.svg)](https://www.nuget.org/packages/SimpleValueObjects)

_This library is a work in progress. Existing functionality is pretty much done, but public interfaces may change in the future._

`SimpleValueObjects` is a .NET Standard library, that aims to provide a toolkit for simple creation of Value Objects.

I believe such library can be useful, since .NET doesn't make implementation of proper Value Objects easy, with all it's nulls, operator overloading, non-generic methods overriding, etc. In other words, creating a valid Value Object by hand is tricky and can be quite a hassle, not to mention code and test duplication it produces.

This library means to cover cases I've found most common when developing applications.

## Value Objects

A Value Object is an object:
* that is immutable, and
* whose equality is based on value, rather than identity.

This seemingly simple pattern has a wide range of applications. Actually, if you think about it, you're probably using ones on a daily basis - think of `string` or `DateTime`. Other popular examples are amounts of money, date ranges or geographical coordinates.

Domain Driven Design promotes using Value Object for representation of domain concepts, e. g. user names, product prices or ZIP codes.

## Using `SimpleValueObjects`

### Equitable Value Objects

The core idea of Value Objects is that their equality is based on their values. The concept of equality in .NET can be represented in multiple ways though:

* every object implements an `Equals` method,
* there is a `GetHashCode` method that matches `Equals`,
* there are equality operators: `==` and `!=`,
* it is also common to implement `Equals` method in `IEquitable<T>` interface.

Obviously, we want our Value Objects to determine equality in a consistent way, no matter which method is used. Achieving this in unfortunately quite a hassle, especially taking nulls, types and hash code generation into consideration.

Using every base class in `SimpleValueObjects` will guarantee following things:

* overloaded `==` and `!=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>.Equals` implementation,
* null handling: no value is equal to null and two nulls are always equal,
* type handling: different types are never equal.

#### `AutoEquitableObject`

In most cases, a Value Object consists of a few fields. Obviously, we consider such objects equal, when their field values are also equal.

When you inherit from `AutoEquitableObject`, it will automatically implement equality in a way, that two objects will be equal when values of their fields are equal.

```cs
public class Money : AutoEquitableObject<Money>
{
    public Currency Currency { get; }
    public decimal Amount { get; }

    public Money(Currency currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;

        if (amount < 0)
        {
            throw new ArgumentException(
                $"Money cannot have negative amount, but got {amount}.");
        }
    }

    public bool IsNothing => Amount == 0 || Currency == Currency.Blemflarcks;

    public override string ToString() => $"{Amount} {Currency}";
}
```

Remarks:

* `AutoEquitableObject` uses reflection to get fields' values, which means it might be slow at times. I believe in most cases that is not a huge problem, however if you use your Value Object extensively you might want to use `EquitableObject`, which let's you implement equality comparison by hand.
* Also, `AutoEquitableObject` _doesn't perform deep comparison_. It means, that if you want your Value Object to be more structured, you should either compose it from other Value Objects, or extend `EquitableObject` instead and implement equality comparison by hand.

#### `WrapperEquitableObject`

Sometimes you'll want to use some more common Value Object, like `string` or `int`, and give them additional context. For instance, user name in a system can just be a string, but with limited length and consisting only of characters subset. Also, it makes sure, that no one passes "just any string" to some method instead of a valid username by accident.

If that's the case, you can use `WrapperEquitableObject`, which wraps exactly one value and will use it for equality comparison and hash code computation.

```cs
public class UserName : WrapperEquitableObject<UserName, string>
{
    public UserName(string userName)
        : base(userName)
    {
        if (userName == null)
        {
            throw new ArgumentException(
                "User name cannot be null.");
        }

        if (!_userNamePattern.IsMatch(userName))
        {
            throw new ArgumentException(
                $"User name should match pattern {_userNamePattern}, " +
                $"but found '{userName}' instead.");
        }
    }

    private readonly Regex _userNamePattern = new Regex("[a-z0-9-]{5,25}");
}
```

Remarks:

* `WrapperEquitableObject` _doesn't perform deep comparison_. It means, that wrapped type should also be a Value Object.

#### `EquitableObject`

When you want to implement equality comparison logic by yourself, you can use `EquitableObject`.

```cs
public class IntRange : EquitableObject<IntRange>
{
    public int From { get; }
    public int To { get; }

    public IntRange(int from, int to)
    {
        From = from;
        To = to;

        if (From > To)
        {
            throw new InvalidOperationException(
                $"From cannot be greater than To in IntRange, but got: {From}-{To}.");
        }
    }

    protected override bool EqualsNotNull(IntRange notNullOther)
    {
        return From == notNullOther.From && To == notNullOther.To;
    }

    protected override int GenerateHashCode()
    {
        return HashCodeCalculator.CalculateFromValues(From, To);
    }
}
```

### Comparable Value Objects

Sometimes we want values that not only can be determined equal, but also can be placed in a certain order. Most commonly used cases would be an `int` or a `DateTime`. We can certainly say, that values of such types can be lesser, equal or greater that another values.

More custom examples would be: temperature, user rating or month. Again, they can be put in an order: 5 star rating is greater than just 3 stars, or February comes after January.

In .NET such concept can be represented by:
* `IComparable` and `IComparable<T>` implementations and
* relational operators, i. e. `<`, `<=`, `>` and `>=`.

Following base classes will give you:

* equivalent comparison and equality comparison,
* overloaded `<`, `<=`, `==`, `!=`, `>` and `>=` operators,
* overridden `Object.Equals`,
* `IEquitable<T>`, `IComparable` and `IComparable<T>` implementations,
* null handling: every value is greater than null, no value is equal to null and two nulls are always equal,
* type handling: different types are never equal and comparing objects of different types will throw an exception.

#### `WrapperComparableObject`

Like with `WrapperEquitableObject`, sometimes you'll want to wrap a simpler value to give it additional context. You can do that, if wrapped type implements `IComparable<T>` (where `T` is itself), which then will be used for comparison.

```cs
public class MovieRating : WrapperComparableObject<MovieRating, int>
{
    public MovieRating(int stars)
        : base(stars)
    {
        if (stars < 1 || stars > 5)
        {
            throw new ArgumentException(
                $"UserRating should be between 1 and 5 stars, but got {stars} stars.");
        }
    }

    public static readonly MovieRating Lowest = new MovieRating(1);

    public static readonly MovieRating Highest = new MovieRating(5);

    public override string ToString() => new string('★', count: Value);
}
```

#### `ComparableObject`

With `ComparableObject` you only implement comparison once, and all comparison and equality comparison logic are handled consistently. Again, you have to implement generating hash code yourself.

```cs
public class YearMonth : ComparableObject<YearMonth>
{
    public int Year { get; }
    public Month Month { get; }

    public YearMonth(int year, Month month)
    {
        Year = year;
        Month = month;

        if (!Enum.IsDefined(typeof(Month), month))
        {
            throw new ArgumentException($"Month {month} is not valid.");
        }
    }

    public YearMonth Next()
    {
        return Month == Month.December
            ? new YearMonth(Year + 1, Month.January)
            : new YearMonth(Year, Month + 1);
    }

    protected override int CompareToNotNull(YearMonth notNullOther)
    {
        return Year != notNullOther.Year
            ? Year - notNullOther.Year
            : Month - notNullOther.Month;
    }

    protected override int GenerateHashCode() => HashCodeCalculator.CalculateFromValues(Year, Month);

    public override string ToString() => $"{Month} {Year}";
}
```

## Generating hash codes

Sometimes you'll need to generate a hash code by hand, e.g. when extending `EquitableObject` or `ComparableObject`. If you just want to generate a hash from a bunch of different values, you can use `HashCodeCalculator.CalculateFromValues` method.

_Remember, that when to Value Objects are equal, their hash code should also be equal._

Also, it might be worth checking out some offical sources:

https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netframework-4.7.1#Remarks

## Resources

Some interesting resouces related to Value Objects:
* https://martinfowler.com/bliki/ValueObject.html
* http://wiki.c2.com/?ValueObject
* https://refactoring.guru/smells/primitive-obsession