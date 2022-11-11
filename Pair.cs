using System;
using System.Collections.Generic;

namespace GogumaV2;

public struct Pair<T> : IEquatable<Pair<T>>
{
  public T X { get; set; }
  public T Y { get; set; }

  public Pair(T x, T y)
  {
    X = x;
    Y = y;
  }

  public override string ToString() => $"({X}, {Y})";

  public bool Equals(Pair<T> other)
  {
    return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);
  }

  public override bool Equals(object? obj)
  {
    return obj is Pair<T> other && Equals(other);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(X, Y);
  }

  public static bool operator ==(Pair<T> a, Pair<T> b) => a.Equals(b);
  public static bool operator !=(Pair<T> a, Pair<T> b) => !a.Equals(b);
}

public struct Pair<Tx, Ty> : IEquatable<Pair<Tx, Ty>>
{
  public Tx X { get; set; }
  public Ty Y { get; set; }

  public Pair(Tx x, Ty y)
  {
    X = x;
    Y = y;
  }

  public bool Equals(Pair<Tx, Ty> other)
  {
    return EqualityComparer<Tx>.Default.Equals(X, other.X) && EqualityComparer<Ty>.Default.Equals(Y, other.Y);
  }

  public override bool Equals(object? obj)
  {
    return obj is Pair<Tx, Ty> other && Equals(other);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(X, Y);
  }

  public override string ToString() => $"({X}, {Y})";

  public static bool operator ==(Pair<Tx, Ty> a, Pair<Tx, Ty> b) => a.Equals(b);
  public static bool operator !=(Pair<Tx, Ty> a, Pair<Tx, Ty> b) => !a.Equals(b);
}