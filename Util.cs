using System;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace GogumaV2;

public static class Util
{
  public static int Limit(this int value, int min, int max) => (int) Math.Max(min, Math.Min(max, value));
  public static uint Limit(this uint value, uint min, uint max) => (uint) Math.Max(min, Math.Min(max, value));
  public static short Limit(this short value, short min, short max) => (short) Math.Max(min, Math.Min(max, value));
  public static ushort Limit(this ushort value, ushort min, ushort max) => (ushort) Math.Max(min, Math.Min(max, value));
  public static byte Limit(this byte value, byte min, byte max) => (byte) Math.Max(min, Math.Min(max, value));
  public static sbyte Limit(this sbyte value, sbyte min, sbyte max) => (sbyte) Math.Max(min, Math.Min(max, value));
  public static decimal Limit(this decimal value, decimal min, decimal max) => Math.Max(min, Math.Min(max, value));
  public static long Limit(this long value, long min, long max) => Math.Max(min, Math.Min(max, value));
  public static ulong Limit(this ulong value, ulong min, ulong max) => Math.Max(min, Math.Min(max, value));
  public static double Limit(this double value, double min, double max) => Math.Max(min, Math.Min(max, value));
  public static float Limit(this float value, float min, float max) => Math.Max(min, Math.Min(max, value));

  public static string GetSep(this string txt, int length, string sep)
  {
    var sb = new StringBuilder();

    if (txt == "")
    {
      for (var i = 0; i < length; i++) sb.Append(sep);
    }
    else if (txt.Length % 2 == 0)
    {
      var l = (length - txt.Length) / 2 - 1;
      for (var i = 0; i < l; i++) sb.Append(sep);
      sb.Append($" {txt} ");
      for (var i = 0; i < l + 1; i++) sb.Append(sep);
    }
    else
    {
      var l = (length - txt.Length - 1) / 2 - 1;
      for (var i = 0; i < l + 1; i++) sb.Append(sep);
      sb.Append($" {txt} ");
      for (var i = 0; i < l + 1; i++) sb.Append(sep);
    }

    return sb.ToString();
  }

  public static void PrintF(this Screen screen, string formattedText)
  {
    //<fg='' bg=''>
    if (formattedText.Contains('<'))
    {
      string[] split = formattedText.Split('<');

      for (int i = 1; i < split.Length; i++)
      {
        string text = split[i];
        string[] tagSplit = text.Split('>');
        try
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);

          if (tagSplit[0].Contains("fg='"))
          {
            color.X = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
          }

          if (tagSplit[0].Contains("bg='"))
          {
            color.Y = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
          }

          screen.Print(tagSplit[1], color);
        }
        catch
        {
          screen.Print(tagSplit[1]);
        }
      }
    }
    else
      screen.Print(formattedText);
  }

  public static void PrintWithFont(this Screen screen, string text, FontFamily fontFamily, Pair<Brush> color)
  {
    TextRange tr = new TextRange(screen.RTBMain.Document.ContentEnd, screen.RTBMain.Document.ContentEnd);
    tr.Text = text;
    tr.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
    tr.ApplyPropertyValue(TextElement.ForegroundProperty, color.X);
    tr.ApplyPropertyValue(TextElement.BackgroundProperty, color.Y);
  }
}