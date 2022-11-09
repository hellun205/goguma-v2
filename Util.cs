using System;

namespace GogumaV2;

public static class Util
{
  public static decimal Limit(decimal min, decimal max, decimal value)
  {
    return Math.Max(min, Math.Min(max, value));
  }
  
  public static float LimitF(float min, float max, float value)
  {
    return Math.Max(min, Math.Min(max, value));
  }
  
  public static double LimitF(double min, double max, double value)
  {
    return Math.Max(min, Math.Min(max, value));
  }
}