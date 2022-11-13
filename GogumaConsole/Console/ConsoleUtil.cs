using System.Drawing;
using System.Text;
using GogumaConsole.Engine.Map;
using GogumaConsole.Engine.Map.Field;


namespace GogumaConsole.Console;

public static class ConsoleUtil
{
  public static readonly Pair<ConsoleColor> DefaultColor =
    new(System.Console.ForegroundColor, System.Console.BackgroundColor);

  public static readonly Pair<ConsoleColor> ColorOnSelect =
    new Pair<ConsoleColor>(DefaultColor.Y, ConsoleClrFromRGB(125, 125, 125));

  public static readonly Pair<ConsoleColor> ColorOnNoSelect =
    new Pair<ConsoleColor>(ConsoleClrFromRGB(125, 125, 125), DefaultColor.Y);

  public static KeySet<ConsoleKey> KeySet { get; set; } = new KeySet<ConsoleKey>(ConsoleKey.UpArrow,
    ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.Enter);

  public static string ReadText()
  {
    SetColor(ColorOnSelect);
    Print(" ");
    var res = System.Console.ReadLine();
    Print(" ");
    SetColor(DefaultColor);
    return res;
  }

  public static ConsoleKey ReadKey() => ReadKey(null);

  public static ConsoleKey ReadKey(ConsoleKey? keyToPress)
  {
    if (keyToPress == null)
    {
      return System.Console.ReadKey(true).Key;
    }
    else
    {
      ConsoleKeyInfo res;
      do
      {
        res = System.Console.ReadKey(true);
      } while (res.Key != keyToPress);

      return res.Key;
    }
  }

  public static void Clear() => System.Console.Clear();

  public static void Print(string text) => System.Console.Write(text);

  public static void Print(string text, Pair<ConsoleColor> color)
  {
    SetColor(color);
    System.Console.Write(text);
    SetColor(DefaultColor);
  }

  public static void SetColor(Pair<ConsoleColor> color)
  {
    System.Console.ForegroundColor = color.X;
    System.Console.BackgroundColor = color.Y;
  }

  public static void Println() => Print("\n");

  public static void Select(string title, Dictionary<string, Action> queue) => Select(title, queue, string.Empty, null);

  public static void Select(string title, Dictionary<string, Action> queue, string cancelText, Action? cancelCallBack)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    var value = Select(title, dict, cancelText);

    if (cancellable && string.IsNullOrEmpty(value))
      cancelCallBack();
    else
      queue[value]();
  }

  public static string Select(string title, Dictionary<string, string> queue) => Select(title, queue, string.Empty);

  public static string Select(string title, Dictionary<string, string> queue, string cancelText)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    int selectingIndex = 0;
    int maxIndex = queue.Count - (cancellable ? 0 : 1);
    string[] options = queue.Keys.ToArray();

    while (true)
    {
      Clear();
      Print(title);
      Println();

      for (int i = 0; i < queue.Count + (cancellable ? 1 : 0); i++)
      {
        Pair<ConsoleColor> color = ColorOnNoSelect;
        if (i == selectingIndex)
        {
          color = ColorOnSelect;
        }

        Print($"{color} [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ");
        Println();
      }

      var key = ReadKey();

      if (key == KeySet.Enter)
      {
        return (((cancellable && selectingIndex == maxIndex) ? null : queue[options[selectingIndex]]) ??
                string.Empty);
      }
      else if (key == KeySet.Up)
      {
        if (selectingIndex == 0)
          selectingIndex = maxIndex;
        else
          selectingIndex -= 1;
      }
      else if (key == KeySet.Down)
      {
        if (selectingIndex == maxIndex)
          selectingIndex = 0;
        else
          selectingIndex += 1;
      }
    }
  }

  public static Pair<int>? Select2d(string title, Dictionary<string, List<string>> queue) =>
    Select2d(title, queue, null);

  public static Pair<int>? Select2d(string title, Dictionary<string, List<string>> queue, string cancelText)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    List<string> rows = queue.Keys.ToList();
    Pair<int> selectingIndexs = new();
    Pair<int> maxIndexs = new(rows.Count - (cancellable ? 0 : 1), 0);

    while (true)
    {
      Clear();
      Print($"{title}\n\n");

      for (int i = 0; i < rows.Count + (cancellable ? 1 : 0); i++)
      {
        Pair<ConsoleColor> color = ColorOnNoSelect;
        if (i == selectingIndexs.X)
        {
          color = ColorOnSelect;
        }

        Print("  ");
        Print($"{color} [ {(cancellable && i == rows.Count ? cancelText : rows[i])} ] ");
        Print("  ");
      }

      Println();
      if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
        for (int i = 0; i < queue[rows[selectingIndexs.X]].Count; i++)
        {
          Pair<ConsoleColor> color = ColorOnNoSelect;
          if (i == selectingIndexs.Y)
          {
            color = ColorOnSelect;
          }

          Print("    ");
          Print($"{color} [ {queue[rows[selectingIndexs.X]][i]} ] ");
          Println();
        }

      var key = ReadKey();

      if (key == KeySet.Enter)
      {
        return (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X) ? selectingIndexs : null);
      }
      else if (key == KeySet.Left)
      {
        if (selectingIndexs.X == 0)
          selectingIndexs.X = maxIndexs.X;
        else
          selectingIndexs.X -= 1;
        selectingIndexs.Y = 0;
        if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
          maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
      }
      else if (key == KeySet.Right)
      {
        if (selectingIndexs.X == maxIndexs.X)
          selectingIndexs.X = 0;
        else
          selectingIndexs.X += 1;
        selectingIndexs.Y = 0;
        if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
          maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
      }
      else if (key == KeySet.Up)
      {
        if (selectingIndexs.Y == 0)
          selectingIndexs.Y = maxIndexs.Y;
        else
          selectingIndexs.Y -= 1;
      }
      else if (key == KeySet.Down)
      {
        if (selectingIndexs.Y == maxIndexs.Y)
          selectingIndexs.Y = 0;
        else
          selectingIndexs.Y += 1;
      }
    }
  }

  public static ConsoleKey Pause(string text) => Pause(text, null);

  public static ConsoleKey Pause() => Pause("계속하려면 아무 키나 누르십시오...");

  public static ConsoleKey Pause(ConsoleKey press) => Pause($"계속하려면 {press}키를 누르십시오...", press);

  public static ConsoleKey Pause(string text, ConsoleKey? press)
  {
    if (!string.IsNullOrEmpty(text))
      Print($"\n{text}\n");
    return ReadKey(press);
  }

  public static ConsoleColor ConsoleClrFromRGB(byte r, byte g, byte b)
  {
    int index = (r > 128 | g > 128 | b > 128) ? 8 : 0; // Bright bit
    index |= (r > 64) ? 4 : 0; // Red bit
    index |= (g > 64) ? 2 : 0; // Green bit
    index |= (b > 64) ? 1 : 0; // Blue bit

    return (System.ConsoleColor) index;
  }

  public static ConsoleColor ToConsoleColor(this Color color) => ConsoleClrFromRGB(color.R, color.G, color.B);
}