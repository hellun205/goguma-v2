using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace GogumaV2
{
  public static class ConsoleUtil
  {
    public static Screen MainScreen { get; set; }
    public static string Text { get; private set; } = "";
    public static Key Key { get; private set; }
    public static string Selection { get; private set; } = "";
    public static Pair<int>? Selection2d { get; private set; } = new(0, 0);
    public static Pair<Brush> ColorOnSelect => new(MainScreen.BGColor, MainScreen.FGColor);
    public static Pair<Brush> ColorOnNoSelect => new(MainScreen.FGColor, MainScreen.BGColor);
    public static string SelectCancelText = "취소";

    private static bool isSelecting = false;

    public static void ReadText(Action callBack)
    {
      MainScreen.ReadText(() =>
      {
        Text = MainScreen.TextOfRead;
        callBack();
      });
    }

    public static void ReadKey(Action callBack)
    {
      MainScreen.ReadKey(() =>
      {
        Key = MainScreen.KeyOfRead;
        callBack();
      });
    }

    public static void ReadKey(Key keyToPress, Action callBack)
    {
      MainScreen.ReadKey(keyToPress, () =>
      {
        Key = MainScreen.KeyOfRead;
        callBack();
      });
    }

    public static void Clear() => MainScreen.Clear();

    public static void Print(string text) => MainScreen.Print(text);

    public static void Print(string text, Pair<Brush> color) => MainScreen.Print(text, color);

    public static void PrintF(string formattedText)
    {
      // new BrushConverter().ConvertFromString("Red");

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
            Pair<Brush> color = new(MainScreen.FGColor, MainScreen.BGColor);

            if (tagSplit[0].Contains("fg='"))
            {
              color.X = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
            }

            if (tagSplit[0].Contains("bg='"))
            {
              color.Y = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
            }

            Print(tagSplit[1], color);
          }
          catch
          {
            Print(tagSplit[1]);
          }
        }
      }
      else
        Print(formattedText);
    }

    public static void Select(string title, Dictionary<string, Action> queue, bool cancellable, Action cancelCallBack)
    {
      List<string> options = queue.Keys.ToList();
      List<Action> actions = queue.Values.ToList();
      Dictionary<string, string> dict = new Dictionary<string, string>();
      foreach (string option in options)
      {
        dict.Add(option, option);
      }

      Select(title, dict, cancellable, () =>
      {
        if (cancellable && string.IsNullOrEmpty(Selection))
          cancelCallBack();
        else
          queue[Selection]();
      });
    }

    public static void Select(string title, Dictionary<string, string> queue, bool cancellable, Action callBack)
    {
      if (!isSelecting)
      {
        isSelecting = true;
        int selectingIndex = 0;
        int maxIndex = queue.Count - (cancellable ? 0 : 1);
        List<string> options = queue.Keys.ToList();

        void While()
        {
          Clear();
          PrintF(title);
          Print("\n");

          for (int i = 0; i < queue.Count + (cancellable ? 1 : 0); i++)
          {
            Pair<Brush> color = ColorOnNoSelect;
            if (i == selectingIndex)
            {
              color = ColorOnSelect;
            }

            Print($" [ {(cancellable && i == maxIndex ? SelectCancelText : options[i])} ] ", color);
            Print("\n");
          }

          ReadKey(() =>
          {
            if (Key == Key.Enter)
            {
              if (cancellable && selectingIndex == maxIndex)
                Selection = null;
              else
                Selection = queue[options[selectingIndex]];

              isSelecting = false;
              callBack();
            }
            else if (Key == Key.Up)
            {
              if (selectingIndex == 0)
                selectingIndex = maxIndex;
              else
                selectingIndex -= 1;
              While();
            }
            else if (Key == Key.Down)
            {
              if (selectingIndex == maxIndex)
                selectingIndex = 0;
              else
                selectingIndex += 1;
              While();
            }
            else While();
          });
        }

        While();
      }
      else throw new Exception("이미 선택중 입니다.");
    }

    public static void Select2d(string title, Dictionary<string, List<string>> queue, bool cancellable, Action callBack)
    {
      if (!isSelecting)
      {
        isSelecting = true;
        List<string> rows = queue.Keys.ToList();
        Pair<int> selectingIndexs = new();
        Pair<int> maxIndexs = new(rows.Count - (cancellable ? 0 : 1), 0);

        void While()
        {
          Clear();
          PrintF($"{title}\n\n");

          for (int i = 0; i < rows.Count + (cancellable ? 1 : 0); i++)
          {
            Pair<Brush> color = ColorOnNoSelect;
            if (i == selectingIndexs.X)
            {
              color = ColorOnSelect;
            }

            Print("  ");
            Print($" [ {(cancellable && i == rows.Count ? SelectCancelText : rows[i])} ] ", color);
            Print("  ");
          }

          Print("\n");
          if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
            for (int i = 0; i < queue[rows[selectingIndexs.X]].Count; i++)
            {
              Pair<Brush> color = ColorOnNoSelect;
              if (i == selectingIndexs.Y)
              {
                color = ColorOnSelect;
              }

              Print("    ");
              Print($" [ {queue[rows[selectingIndexs.X]][i]} ] ", color);
              Print("\n");
            }

          ReadKey(() =>
          {
            if (Key == Key.Enter)
            {
              if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
                Selection2d = selectingIndexs;
              else
                Selection2d = null;
              isSelecting = false;
              callBack();
            }
            else if (Key == Key.Left)
            {
              if (selectingIndexs.X == 0)
                selectingIndexs.X = maxIndexs.X;
              else
                selectingIndexs.X -= 1;
              selectingIndexs.Y = 0;
              if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
                maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
              While();
            }
            else if (Key == Key.Right)
            {
              if (selectingIndexs.X == maxIndexs.X)
                selectingIndexs.X = 0;
              else
                selectingIndexs.X += 1;
              selectingIndexs.Y = 0;
              if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
                maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
              While();
            }
            else if (Key == Key.Up)
            {
              if (selectingIndexs.Y == 0)
                selectingIndexs.Y = maxIndexs.Y;
              else
                selectingIndexs.Y -= 1;
              While();
            }
            else if (Key == Key.Down)
            {
              if (selectingIndexs.Y == maxIndexs.Y)
                selectingIndexs.Y = 0;
              else
                selectingIndexs.Y += 1;
              While();
            }
            else While();
          });
        }

        While();
      }
      else throw new Exception("이미 선택중 입니다.");
    }

    public static void Pause(string text, Action callBack)
    {
      if (!string.IsNullOrEmpty(text))
        Print($"\n{text}\n");
      ReadKey(callBack);
    }

    public static void Pause(Action callBack) => Pause("계속하려면 아무 키나 누르십시오...", callBack);

    public static void Pause(string text, Key press, Action callBack)
    {
      if (!string.IsNullOrEmpty(text))
        Print($"\n{text}\n");
      ReadKey(press, callBack);
    }

    public static void Pause(Key press, Action callBack) => Pause($"계속하려면 {press}키를 누르십시오...", press, callBack);

  }
}