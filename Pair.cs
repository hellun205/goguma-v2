namespace goguma_v2
{
  public struct Pair<T>
  {
    public T X { get; set; }
    public T Y { get; set; }

    public Pair(T x, T y)
    {
      X = x;
      Y = y;
    }

    public override string ToString() => $"({X}, {Y})";
  }
}
