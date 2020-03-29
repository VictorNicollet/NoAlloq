# NoAlloq : LINQ for `Span<T>`

The [official stance](https://github.com/dotnet/runtime/issues/26082) for using LINQ with System.Memory is to convert a `Memory<T>` with `MemoryMarshal.ToEnumerable()`, which does not help with `Span<T>` itself. 

NoAlloq aims to provide basic LINQ functionality on top of `Span<T>`, **without** allocating any memory. Since some LINQ features (such as `.Reverse()` or `.OrderBy()`) require storing the entire sequence in memory, NoAlloq requires the user to provide their own memory backing.

NuGet Package : `NoAlloq`

You can contact the author at: victor@nicollet.net

## Usage examples

All operations can be performed on top of a `ReadOnlySpan<T>`, including one on the stack or obtained from a `string`: 

```csharp
using NoAlloq;

int CountPrimeDivisors(int n)
{
    ReadOnlySpan<int> primes = stackalloc int[] { 2, 3, 5, 7, 11, 13, 17, 19 };
    return primes.Count(p => n % p == 0);
}

char FirstUppercaseChar(string s) =>
    s.AsSpan().FirstOrDefault(c => c.IsUpper);
```

### Without allocation

Currently, NoAlloq supports the following methods without any allocation involved: 

 - `.Select(Func<TIn, TOut>)` but not yet `.Select(Func<TIn, int, TOut>)`.
 - `.Where(Func<T, bool>)` but not yet `.Where(Func<T, int, bool>)`.
 - `.Take()` and `.Skip()` as `.Slice(skip, take)` for consistency with `Span<T>.Slice()`.
 - `.First()`, `.First(Predicate<T>)`, `.FirstOrDefault()` and `.FirstOrDefault(Predicate<T>)`.
 - `.Last()`, `.Last(Predicate<T>)`, `.LastOrDefault()` and `.LastOrDefault(Predicate<T>)`.
 - `.Count()` and `.Count(Predicate<T>)`, as well as `.CountTrue()` as a shortcut for `.Count(b => b)`.
 - `.Any()` and `.Any(Predicate<T>)`, as well as `.AnyTrue()` as a shortcut for `.Any(b => b)`.
 - `.All(Predicate<T>)`, as well as `.AllTrue()` as a shortcut for `.All(b => b)`.
 - `.Aggregate()` 
 - `.Single()`, `.Single(Predicate<T>)`, `.SingleOrDefault()` and `SingleOrDefault(Predicate<T>)`.
 - `.Sum()` and `.Sum(Func<TIn, TOut>)` for `int`, `float` and `double`.
 - `SpanEnumerable.Range(first, count)` equivalent to `Enumerable.Range`.
 - `SpanEnumerable.Repeat(value, count)` equivalent to `Enumerable.Repeat`.

Other methods which are obviously possible without allocation (such as `ElementAt`) will be added in the near future.

### With external allocation

When an operation requires a memory allocation to store its output (or internal temporary values), NoAlloc will expect that memory as an argument. It can come from a `stackalloc` span (if the size is known to be small enough), from an `ArrayPool`, or simply from an array. 

#### CopyInto

`Span<T> SpanEnumerable.CopyInto(Span<T> span)` expects a span large enough to fit all data in the enumerable (and will throw if there is not enough room).   
 
```csharp
ReadOnlySpan<string> strings = ...;

Span<int> lengths = stackalloc int[strings.Length];
strings.Select(s => s.Length).CopyInto(lengths);
```

#### TakeInto

`Span<T> SpanEnumerable.TakeInto(Span<T> span)` works like `CopyInto`, but stops at the end of the provided span instead of throwing an exception. 

```csharp
var primes = stackalloc int[10];
primes = SpanEnumerable.Range(0, 1000).Where(IsPrime).TakeInto(primes);
```

#### ReverseInto

`Span<T> SpanEnumerable.ReverseInto(Span<T> span)` works like `CopyInto`, but places the results in the span in reverse order. This is equivalent to calling `CopyInto` followed by `Span<T>.Reverse()`.

```csharp
var reversed = stackalloc int[10];
reversed = SpanEnumerable.Range(0, 10).ReverseInto(reversed);
```

If you can afford to modify the original span, you can also use `.ReverseInPlace()`: 

```csharp
var numbers = stackalloc int[] { 5, 4, 3, 2, 1, 0 };
foreach (var n in numbers.ReverseInPlace())
  // visits 0, 1, 2, 3, 4, 5
```

#### OrderBy

To sort a sequence, it is necessary to store it in its entirety. Because of this, all `.OrderBy()` methods take a span where the sequence will be copied and stored. The standard LINQ methods thus become:

 - `.OrderBy(Span<T> backing, Func<T, TK> keySelector)` 
 - `.OrderBy(Span<T> backing, Func<T, TK> keySelector, IComparer<TK> comparer)` 
 - `.OrderByDescending(Span<T> backing, Func<T, TK> keySelector)`
 - `.OrderByDescending(Span<T> backing, Func<T, TK> keySelector, IComparer<TK> comparer)`

The backing span must be large enough to contain the entire sequence, but may be larger than the sequence (in which case NoAlloq will only use the necessary prefix). 

For convenience, NoAlloq also provides variants for the case where the key selector is `x => x`:

 - `.OrderByValue(Span<T> backing)`
 - `.OrderByValue(Span<T> backing, IComparer<T> comparer)`
 - `.OrderByValueDescending(Span<T> backing)`
 - `.OrderByValueDescending(Span<T> backing, IComparer<T> comparer)`

The above methods return an `OrderingPlan<..>` instance, which supports all usual NoAlloq methods in addition to: 

 - `.ThenBy(Func<T, TK> keySelector)`
 - `.ThenBy(Func<T, TK> keySelector, IComparer<TK> comparer)`
 - `.ThenByDescending(Func<T, TK> keySelector)`
 - `.ThenByDescending(Func<T, TK> keySelector, IComparer<TK> comparer)`

#### In-place operations

If you only use methods which consume the input linearly (that is, computing the value for position `p` does not require cells at positions `i < p` ), you may choose to use the input span as the output, effectively performing an in-place transformation.

```csharp
Span<string> strings = ...;
strings = strings.Select(s => s.ToUpper()).Where(s => s.Length < 10).CopyInto(strings);
```

The methods that consume the input linearly are: 

 - `.Select()`
 - `.Where()` 
 - `.OrderBy()` and its variants: every time a value is consumed from an `.OrderBy()`, it frees the lowest used index in its backing. 

This property of `.OrderBy()` means it's possible to do the following without any allocation: 

```csharp
string[] names = ...;
names.OrderBy(names, n => n.Length)
     .ThenBy(n => n)
     .Select(n => n.ToUpper())
     .CopyInto(names);
```

### With internal allocation

NoAlloq provides the usual `.ToArray()`, `.ToDictionary()`, `.ToList()` and `.ToHashSet()`, which inherently must allocate memory (since the returned object must be allocated).

However, in order to give the caller control over those allocations, NoAlloq provides alternative `.CopyInto()` methods which expect their destination to be provided.

```csharp
ReadOnlySpan<string> strings = ...;

// With ToList
List<string> list = strings.ToList();

// Equivalent: 
List<string> list2 = new List<string>();
strings.CopyInto(list2);

// With ToHashSet
HashSet<string> hash = strings.ToHashSet();

// Equivalent:
HashSet<string> hash = new HashSet<string>();
strings.CopyInto(hash);

// With ToDictionary
Dictionary<string, int> dict = strings.ToDictionary(s => s, s => s.Length);

// Equivalent:
Dictionary<string, int> dict2 = new Dictionary<string, int>();
strings.CopyInto(dict2, s => s, s => s.Length)
```

#### ToSpanEnumerable

The `ToSpanEnumerable` extension method converts a normal `IEnumerable<T>` into a span enumerable compatible with NoAlloq:

```csharp
List<int> values = ...;
Span<int> copy = stackalloc int[values.Count];
copy = names.ToSpanEnumerable().CopyInto(copy);
```

This extension method will invoke `IEnumerable<T>.GetEnumerator()` which will perform at least one allocation (the returned `IEnumerator<T>`).

## Returning or passing as arguments

Since any type capable of reading from a `Span<T>` must be a `ref struct`, the `SpanEnumerable` defined by NoAlloq cannot be stored as a member in a class or non-`ref` struct and cannot be used within the same function as `await` or `yield return`. 

Moreover, since `ref struct` types cannot implement interfaces, there is no easy way to hide the complexity of the types used by NoAlloq. For example, by writing: 

```csharp
Span<User> span = ...;
var alive = span.OrderBy(span, x => x.Name).Where(x => x.IsAlive);
```

The type of the `alive` variable is: 

```csharp
SpanEnumerable<User, User, 
    SecondaryWhereDelegateProducer<User, User, 
        OrderingProducer<User, string, DelegateExtractor<User, string>, Comparer<string>>>> 
```

As such, creating a function that returns such a value, or takes such a value as argument, is rather tedious, even by using generics and constraints. To alleviate this, NoAlloq provides the `SpanEnumerable<T>` type which hides the above details from the type _at the cost of a memory allocation_. To convert the result of an arbitrary NoAlloq enumerable to a `SpanEnumerable<T>`, call its `.Box()` method:

```csharp
Span<int> numbers = stackalloc int[] {...};

var smallest10oddNumbers = numbers
	.Where(n => n % 2 != 0)
	.OrderByValueDescending(numbers)
	.Slice(0, 10);

SpanEnumerable<int> boxed = smallest10oddNumbers.Box();
```

The `SpanEnumerable<T>` is a `ref struct` that supports the same NoAlloq operations as its unboxed source.

If your enumerable was constructed from a `Span<T>`, `.Box()` will only be allowed if `T` is an unmanaged type (managed types may contain references, and so cannot be hidden from the compiler).

```csharp
Span<User> users = ...; // 'User' is a class

users.Where(s => s.IsActive)
     .Box() // not allowed, original T is 'User'
```

## Performance

See the `NoAlloq.Tests/Benchmarks` folder for benchmarks. On the few benchmarks so far, NoAlloq runs faster than LINQ (but still, significantly slower than writing C# code by hand). A few examples:

Doubling the values in an array.

```csharp
Span<int> values = stackalloc int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
values = values
    .Select(v => v * 2)
    .CopyInto(values);
```
|       Method |       Mean |     Error |    StdDev | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |-----------:|----------:|----------:|------:|-----:|-------:|------:|------:|----------:|
|       Manual |   6.116 ns | 0.0895 ns | 0.0747 ns |  0.05 |    1 |      - |     - |     - |         - |
| WithNoAlloq |  49.304 ns | 0.4097 ns | 0.3632 ns |  0.37 |    2 |      - |     - |     - |         - |
|     WithLinq | 133.443 ns | 2.5039 ns | 2.8835 ns |  1.00 |    3 | 0.0317 |     - |     - |     200 B |

Adding the values from an array, doubled.

```csharp
Span<int> values = stackalloc int[] {
     1,  2,  3,  4,  5,  6,  7,  8,  9, 10,
    11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
    21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
};

var sum = values.Sum(x => x * 2);
```

|       Method |      Mean |    Error |   StdDev | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |----------:|---------:|---------:|------:|-----:|-------:|------:|------:|----------:|
|       Manual |  20.21 ns | 0.221 ns | 0.207 ns |  0.12 |    1 |      - |     - |     - |         - |
| WithNoAlloq | 144.25 ns | 0.925 ns | 0.865 ns |  0.88 |    2 |      - |     - |     - |         - |
|     WithLinq | 164.77 ns | 2.133 ns | 1.995 ns |  1.00 |    3 | 0.0279 |     - |     - |     176 B |

Sorting an array of integers, then picking the first 5: 

```csharp
Span<int> values = stackalloc int[] {
        91, 38, 29, 38, 81, 55, 17, 10, 40, 33,
        19, 61, 85, 25, 41, 31, 28, 12, 93, 67
};

var first5 = values
    .OrderByValue(values)
    .Slice(0, 5)
    .CopyInto(values);
```

|      Method |     Mean |    Error |   StdDev | Ratio | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |---------:|---------:|---------:|------:|-----:|-------:|------:|------:|----------:|
| WithNoAlloq | 367.3 ns |  5.25 ns |  4.91 ns |  0.41 |    1 |      - |     - |     - |         - |
|    WithLinq | 900.2 ns | 17.62 ns | 18.86 ns |  1.00 |    2 | 0.1459 |     - |     - |     920 B |

## Future Work

 - Implement the missing no-allocation functions.
 - Functions with memory backing: `.Distinct()`, `.GroupBy()` and `.ToLookUp()`.
 - More benchmarks.