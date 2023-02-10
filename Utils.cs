using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace GogumaWPF;

public static class Utils
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

  public static decimal Lim(decimal value, decimal min, decimal max) => Limit(value, min, max);

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

  public static Brush GetARGB(byte a, byte r, byte g, byte b) => new SolidColorBrush(Color.FromArgb(a, r, g, b));

  public static char ToSymbol(this char character)
  {
    return character switch
    {
      'a' => 'ａ', 'b' => 'ｂ', 'c' => 'ｃ', 'd' => 'ｄ', 'e' => 'ｅ', 'f' => 'ｆ', 'g' => 'ｇ', 'h' => 'ｈ', 'i' => 'ｉ',
      'j' => 'ｊ', 'k' => 'ｋ', 'l' => 'ｌ', 'm' => 'ｍ', 'n' => 'ｎ', 'o' => 'ｏ', 'p' => 'ｐ', 'q' => 'ｑ', 'r' => 'ｒ',
      's' => 'ｓ', 't' => 'ｔ', 'u' => 'ｕ', 'v' => 'ｖ', 'w' => 'ｗ', 'x' => 'ｘ', 'y' => 'ｙ', 'z' => 'ｚ',
      'A' => 'Ａ', 'B' => 'Ｂ', 'C' => 'Ｃ', 'D' => 'Ｄ', 'E' => 'Ｅ', 'F' => 'Ｆ', 'G' => 'Ｇ', 'H' => 'Ｈ', 'I' => 'Ｉ',
      'J' => 'Ｊ', 'K' => 'Ｋ', 'L' => 'Ｌ', 'M' => 'Ｍ', 'N' => 'Ｎ', 'O' => 'Ｏ', 'P' => 'Ｐ', 'Q' => 'Ｑ', 'R' => 'Ｒ',
      'S' => 'Ｓ', 'T' => 'Ｔ', 'U' => 'Ｕ', 'V' => 'Ｖ', 'W' => 'Ｗ', 'X' => 'Ｘ', 'Y' => 'Ｙ', 'Z' => 'Ｚ',
      '`' => '｀', '1' => '１', '2' => '２', '3' => '３', '4' => '４', '5' => '５', '6' => '６', '7' => '７', '8' => '８',
      '9' => '９', '0' => '０', '-' => '－', '=' => '＝', '_' => '＿', '+' => '＋', '[' => '［', '{' => '｛', ']' => '］',
      '}' => '｝', '\\' => '￦', '|' => '｜', ';' => '；', ':' => '：', '\'' => '＇', '"' => '＂', ',' => '，', '<' => '＜',
      '.' => '．', '>' => '＞', '/' => '／', '?' => '？', ' ' => '　',
      '~' => '～', '!' => '！', '@' => '＠', '#' => '＃', '$' => '＄', '%' => '％', '^' => '＾', '&' => '＆', '*' => '＊',
      '(' => '（', ')' => '）',
      _ => character
    };
  }

  public static char ToDefault(this char symbol)
  {
    return symbol switch
    {
      'ａ' => 'a', 'ｂ' => 'b', 'ｃ' => 'c', 'ｄ' => 'd', 'ｅ' => 'e', 'ｆ' => 'f', 'ｇ' => 'g', 'ｈ' => 'h', 'ｉ' => 'i',
      'ｊ' => 'j', 'ｋ' => 'k', 'ｌ' => 'l', 'ｍ' => 'm', 'ｎ' => 'n', 'ｏ' => 'o', 'ｐ' => 'p', 'ｑ' => 'q', 'ｒ' => 'r',
      'ｓ' => 's', 'ｔ' => 't', 'ｕ' => 'u', 'ｖ' => 'v', 'ｗ' => 'w', 'ｘ' => 'x', 'ｙ' => 'y', 'ｚ' => 'z',
      'Ａ' => 'A', 'Ｂ' => 'B', 'Ｃ' => 'C', 'Ｄ' => 'D', 'Ｅ' => 'E', 'Ｆ' => 'F', 'Ｇ' => 'G', 'Ｈ' => 'H', 'Ｉ' => 'I',
      'Ｊ' => 'J', 'Ｋ' => 'K', 'Ｌ' => 'L', 'Ｍ' => 'M', 'Ｎ' => 'N', 'Ｏ' => 'O', 'Ｐ' => 'P', 'Ｑ' => 'Q', 'Ｒ' => 'R',
      'Ｓ' => 'S', 'Ｔ' => 'T', 'Ｕ' => 'U', 'Ｖ' => 'V', 'Ｗ' => 'W', 'Ｘ' => 'X', 'Ｙ' => 'Y', 'Ｚ' => 'Z',
      '｀' => '`', '１' => '1', '２' => '2', '３' => '3', '４' => '4', '５' => '5', '６' => '6', '７' => '7', '８' => '8',
      '９' => '9', '０' => '0', '－' => '-', '＝' => '=', '＿' => '_', '＋' => '+', '［' => '[', '｛' => '{', '］' => ']',
      '｝' => '}', '￦' => '\\', '｜' => '|', '；' => ';', '：' => ':', '＇' => '\'', '＂' => '"', '，' => ',', '＜' => '<',
      '．' => '.', '＞' => '>', '／' => '/', '？' => '?', '　' => ' ',
      '～' => '~', '！' => '!', '＠' => '@', '＃' => '#', '＄' => '$', '％' => '%', '＾' => '^', '＆' => '&', '＊' => '*',
      '（' => '(', '）' => ')',
      _ => symbol
    };
  }

  public static string ToSymbol(this string str)
  {
    if (string.IsNullOrEmpty(str))
      return string.Empty;
    else if (str.Length == 1)
      return str.ToCharArray()[0].ToSymbol().ToString();
    else
    {
      var chars = str.ToCharArray();
      var sb = new StringBuilder();
      foreach (var chr in chars)
        sb.Append(chr.ToSymbol());

      return sb.ToString();
    }
  }

  public static string ToDefault(this string symbolStr)
  {
    if (string.IsNullOrEmpty(symbolStr))
      return string.Empty;
    else if (symbolStr.Length == 1)
      return symbolStr.ToCharArray()[0].ToDefault().ToString();
    else
    {
      var chars = symbolStr.ToCharArray();
      var sb = new StringBuilder();
      foreach (var chr in chars)
        sb.Append(chr.ToDefault());

      return sb.ToString();
    }
  }

  public static string FillEmpty(this string str, int len, char fillChr = ' ')
  {
    if (len <= str.Length)
    {
      str.Substring(0, str.Length - 1);
      str.Append('…');
    }

    var sb = new StringBuilder(str);
    for (var i = 0; i < len - str.Length; i++)
    {
      sb.Append(fillChr);
    }

    return sb.ToString();
  }

  public static T GetRandom<T>(this IEnumerable<T> enumerable)
  {
    var rand = new Random();
    return enumerable.ToArray()[rand.Next(0, enumerable.Count())];
  }

  public static string While(this string str, int index)
  {
    var sb = new StringBuilder();
    for (var i = 0; i < index; i++)
    {
      sb.Append(str);
    }

    return sb.ToString();
  }

  public static string While(this char str, int index)
  {
    var sb = new StringBuilder();
    for (var i = 0; i < index; i++)
    {
      sb.Append(str);
    }

    return sb.ToString();
  }

  public static string ToEvenLength(this string str)
  {
    if (str.Length % 2 == 0) return str;
    else return str + " ";
  }
}