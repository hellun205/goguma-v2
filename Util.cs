using System;

namespace GogumaV2;

public static class Util
{
  public static int Limit(this int value, int min, int max) => (int)Math.Max(min, Math.Min(max, value));
  public static uint Limit(this uint value, uint min, uint max) => (uint)Math.Max(min, Math.Min(max, value));
  public static short Limit(this short value, short min, short max) => (short)Math.Max(min, Math.Min(max, value));
  public static ushort Limit(this ushort value, ushort min, ushort max) => (ushort)Math.Max(min, Math.Min(max, value));
  public static byte Limit(this byte value, byte min, byte max) => (byte)Math.Max(min, Math.Min(max, value));
  public static sbyte Limit(this sbyte value, sbyte min, sbyte max) => (sbyte)Math.Max(min, Math.Min(max, value));
  public static decimal Limit(this decimal value, decimal min, decimal max) => Math.Max(min, Math.Min(max, value));
  public static long Limit(this long value, long min, long max) => Math.Max(min, Math.Min(max, value));
  public static ulong Limit(this ulong value, ulong min, ulong max) => Math.Max(min, Math.Min(max, value));
  public static double Limit(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));
  public static float Limit(this float value, float min, float max) => Math.Max(min, Math.Min(max, value));
}