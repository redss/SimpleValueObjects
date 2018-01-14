# SimpleValueObjects

[![AppVeyor](https://img.shields.io/appveyor/ci/redss/simplevalueobjects.svg)](https://ci.appveyor.com/project/redss/simplevalueobjects)
[![NuGet](https://img.shields.io/nuget/v/SimpleValueObjects.svg)](https://www.nuget.org/packages/SimpleValueObjects)

_This library is a work in progress. Existing functionality is pretty much done, but public interfaces may change._

`SimpleValueObjects` is a toolkit that let's you create Value Objects easily. It's a .NET Standard library, so you can use it both on .NET Core and on good ol' .NET Framework.

## Quickstart

So you want to create a Value Object.

First, install `SimpleValueObjects`:

```
PM> Install-Package SimpleValueObjects
```

Implement Value Object of your choice:

```cs
using SimpleValueObjects;

public class Position : AutoValueObject<Position>
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

Lastly, enjoy hassle-free equality comparison:

```cs
var first = new Position(5, 5);
var second = new Position(5, 5);

Console.WriteLine(first == second); // True
Console.WriteLine(first != second); // False
Console.WriteLine(first.Equals(second)); // True
```

Also, since both equality comparison and generating hash codes are taken care of, you can use your Value Objects in all sorts of standard collections and LINQ methods:

```cs
private HashSet<Position> _visitedLocations;

public bool WasLocationVisited(Position position)
{
    return _visitedLocations.Contains(position);
}

public IEnumerable<Position> NotVisitedLocations(IEnumerable<Position> positions)
{
    return positions.Except(_visitedLocations);
}
```

<!-- todo: totally rethink this section...  -->

## Library genesis

I believe such library can be useful, since .NET doesn't make implementation of proper Value Objects easy, with all it's nulls, operator overloading, non-generic methods overriding, etc. In other words, creating a valid Value Object by hand is tricky and can be quite a hassle, not to mention code and test duplication it produces.

This library means to cover cases I've found most common when developing applications.

## Introduction to Value Objects

<!-- todo: finish this section -->

A Value Object is an object, that:

* represents a value,
* is immutable and
* whose equality is based on value, rather than identity or reference.

This seemingly simple pattern has a wide range of applications. Actually, if you think about it, you're probably using ones on a daily basis - think of `string` or `DateTime`.

Other popular examples are amounts of money, date ranges or geographical coordinates.

<!-- todo: expand DDD section -->

Domain Driven Design promotes using Value Object for representation of domain concepts, e. g. user names, product prices or ZIP codes.

It seems pretty useful! However, .NET doesn't make implementation of proper Value Objects easy, with all it's nulls, operator overloading, non-generic methods overriding, etc. In other words, creating a valid Value Object by hand is tricky and can be quite a hassle, not to mention code and test duplication it produces.

This library means to cover cases I've found most common when developing applications.

<!-- todo: focus on more example/benefit based approach -->

<!-- You've probably noticed, that these objects have some things in common:
* they're always valid - you cannot create an instance of `DateTime` which points to 32nd of July;
* you can compare their equality in a consistent way: comparing `string` instances using `==` will yield the same results as calling `Equals`, even though `string` is a reference type;
* they implement `GetHashCode` correctly, meaning you can use them as keys in a `Dictionary<T>` or store them in `HashSet<T>`. -->

<!-- todo: explain briefly why immutability is good for you -->

## Using `SimpleValueObjects`

The library let's you create Value Objects using a few base classes which are described below. They're split into two groups:

* Value Objects, which are equality compared and
* Comparable Value Object, which are order compared - they can be lesser, equal or greater than one another.

### Value Objects

Using Value Object base classes will guarantee following things:

* overloaded `==` and `!=` operators,
* overridden `Object.Equals`,
* `IEquatable<T>.Equals` implementation,
* null handling: no value is equal to null and two nulls are always equal,
* type handling: different types are never equal.

#### `AutoValueObject`

In most cases, a Value Object consists of a few fields. Obviously, we consider such objects equal, when their field values are also equal.

`AutoValueObject` will automatically implement equality and hash code generation using reflection.

```cs
public class Money : AutoValueObject<Money>
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

* `AutoValueObject` uses reflection to get fields' values, which means it might be slow at times. I believe in most cases that is not a huge problem, however if you use your Value Object extensively you might want to use `ValueObject`, which let's you implement equality comparison by hand.
* Also, `AutoValueObject` _doesn't perform deep comparison_. It means, that if you want your Value Object to be more structured, you should either compose it from other Value Objects, or extend `ValueObject` instead and implement equality comparison by hand.

#### `WrapperValueObject`

Sometimes you'll just want to use some more common value, like `string` or `int`, and give it additional context.

For instance, _user name_ in a system can be just a string, but with limited length and consisting only of alphanumeric characters.

In that case ypu should use `WrapperValueObject`, which wraps exactly one value and will use it for equality comparison and hash code generation.

```cs
public class UserName : WrapperValueObject<UserName, string>
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

* `WrapperValueObject` _doesn't perform deep comparison_. It means, that wrapped type should also be a Value Object.

#### `ValueObject`

When you want to implement equality comparison logic by yourself, you can use `ValueObject`.

```cs
public class IntRange : ValueObject<IntRange>
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

More custom examples would be: temperature, user rating or a month. Again, they can be put in an order: 5 star rating is greater than just 3 stars, or February comes after January.

Using Comparable Value Object base classes will guarantee following things:

* equivalent order comparison and equality comparison,
* overloaded `<`, `<=`, `==`, `!=`, `>` and `>=` operators,
* overridden `Object.Equals`,
* `IEquatable<T>`, `IComparable` and `IComparable<T>` implementations,
* null handling: every value is greater than null, no value is equal to null and two nulls are always equal,
* type handling: different types are never equal and comparing objects of different types will throw an exception.

#### `WrapperComparableObject`

Like with `WrapperValueObject`, sometimes you'll want to wrap a simpler value to give it additional context. You can do that, if wrapped type implements `IComparable<T>` (where `T` is itself), which then will be used for comparison.

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

Sometimes you'll need to generate a hash code by hand, e.g. when using `ValueObject` or `ComparableObject`. If you just want to generate a hash from a bunch of different values, you can use `HashCodeCalculator.CalculateFromValues` method:

```cs
protected override int GenerateHashCode() => HashCodeCalculator.CalculateFromValues(Year, Month);
```

_Remember: when two Value Objects are equal, their hash codes should also be equal._ 

If you're curious, you can check some [official sources](https://docs.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=netframework-4.7.1#Remarks).

## Resources

Some interesting resouces related to Value Objects:
* https://martinfowler.com/bliki/ValueObject.html
* http://wiki.c2.com/?ValueObject
* https://refactoring.guru/smells/primitive-obsession