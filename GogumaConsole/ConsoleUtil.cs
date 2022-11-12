using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GogumaConsole;
using GogumaConsole.Engine.Map;
using GogumaConsole.Engine.Map.Field;

namespace GogumaConsole;

public static class ConsoleUtil
{
  public static readonly string ColorOnSelect = AnsiColor.BLACK_BG + AnsiColor.WHITE;

  public static readonly string ColorOnNoSelect = AnsiColor.WHITE_BG + AnsiColor.BLACK;

  public static string ReadText()
  {
    Print($"{AnsiColor.WHITE_BG}{AnsiColor.BLACK} ");
    var res = Console.ReadLine();
    Print($" {AnsiColor.RESET}");
    return res;
  }

  public static ConsoleKey ReadKey() => ReadKey(null);

  public static ConsoleKey ReadKey(ConsoleKey? keyToPress)
  {
    if (keyToPress == null)
    {
      return Console.ReadKey(true).Key;
    }
    else
    {
      ConsoleKeyInfo res;
      do
      {
        res = Console.ReadKey(true);
      } while (res.Key != keyToPress);

      return res.Key;
    }
  }

  public static void Clear() => Console.Clear();

  public static void Print(string text) => Console.Write($"{AnsiColor.RESET}{text}");

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

  public static string Select(string title, Dictionary<string, string> queue) => Select(title, queue, null);

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
        string color = ColorOnNoSelect;
        if (i == selectingIndex)
        {
          color = ColorOnSelect;
        }

        Print($"{color} [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ");
        Println();
      }

      var key = ReadKey();

      switch (key)
      {
        case ConsoleKey.Enter:
          return (((cancellable && selectingIndex == maxIndex) ? null : queue[options[selectingIndex]]) ??
                  string.Empty);
          break;

        case ConsoleKey.UpArrow:
          if (selectingIndex == 0)
            selectingIndex = maxIndex;
          else
            selectingIndex -= 1;
          break;

        case ConsoleKey.DownArrow:
          if (selectingIndex == maxIndex)
            selectingIndex = 0;
          else
            selectingIndex += 1;
          break;
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
        string color = ColorOnNoSelect;
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
          string color = ColorOnNoSelect;
          if (i == selectingIndexs.Y)
          {
            color = ColorOnSelect;
          }

          Print("    ");
          Print($"{color} [ {queue[rows[selectingIndexs.X]][i]} ] ");
          Println();
        }

      var key = ReadKey();

      switch (key)
      {
        case ConsoleKey.Enter:
          return (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X) ? selectingIndexs : null);
          break;

        case ConsoleKey.LeftArrow:
          if (selectingIndexs.X == 0)
            selectingIndexs.X = maxIndexs.X;
          else
            selectingIndexs.X -= 1;
          selectingIndexs.Y = 0;
          if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
            maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
          break;

        case ConsoleKey.RightArrow:
          if (selectingIndexs.X == maxIndexs.X)
            selectingIndexs.X = 0;
          else
            selectingIndexs.X += 1;
          selectingIndexs.Y = 0;
          if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
            maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
          break;

        case ConsoleKey.UpArrow:
          if (selectingIndexs.Y == 0)
            selectingIndexs.Y = maxIndexs.Y;
          else
            selectingIndexs.Y -= 1;
          break;

        case ConsoleKey.DownArrow:
        {
          if (selectingIndexs.Y == maxIndexs.Y)
            selectingIndexs.Y = 0;
          else
            selectingIndexs.Y += 1;
          break;
        }
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

  public static void PrintCanvas(ICanvas canvas, string textF = "")
  {
    const char PlayerIcon = '●';
    const char NothingIcon = '■';
    string NothingClr = AnsiColor.GetRGBFG(50, 50, 50);
    string PlayerClr = AnsiColor.GetRGBFG(178, 34, 34);

    string[] texts = textF.Split("\n");

    Clear();
    StringBuilder sb;

    sb = new StringBuilder()
      .Append("┌ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┐');
    Print(sb.ToString());
    Print("\n");

    for (byte y = 0; y <= canvas.CanvasSize.Y; y++)
    {
      Print("│ ");
      for (byte x = 0; x <= canvas.CanvasSize.X; x++)
      {
        var item = canvas.CanvasChild.FirstOrDefault(item => item.Position == new Pair<byte>(x, y));

        if (canvas.MovingObject != null && canvas.MovingObject.Position == new Pair<byte>(x, y))
        {
          Print($"{PlayerClr}{PlayerIcon} ");
        }
        else
        {
          if (item == null)
            Print($"{NothingClr}{NothingIcon} ");
          else
          {
            string clr = AnsiColor.GREEN;
            if (item is IRequirable reqItem)
            {
              clr = (reqItem.Check ? clr : AnsiColor.RED);
            }

            Print($"{clr}{item.Icon} ");
          }
        }
      }

      Print("│  ");
      if (texts.Length > y)
        Print(texts[y]);
      Println();
    }

    sb = new StringBuilder()
      .Append("└ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┘')
      .Append('\n');
    Print(sb.ToString());
  }
}