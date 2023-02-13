using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Goguma;

public static class ScreenUtil
{
  public static void PrintF(this Screen.Screen screen, string formattedText)
  {
    var ftxts = screen.GetFTexts(formattedText);

    foreach (var ftxt in ftxts)
      screen.Print(ftxt.Y, ftxt.X);
  }

  public static Pair<Pair<Brush>, string>[] GetFTexts(this Screen.Screen screen, string formattedText, bool isSymbol = false)
  {
    var defaultRes = new[]
      {new Pair<Pair<Brush>, string>(new(screen.FGColor, screen.BGColor), formattedText)};
    var result = new List<Pair<Pair<Brush>, string>>();

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

          if (!tagSplit[0].Contains("reset"))
          {
            if (tagSplit[0].Contains("fg='"))
            {
              color.X = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
            }

            if (tagSplit[0].Contains("bg='"))
            {
              color.Y = (Brush) new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
            }
          }

          result.Add(new Pair<Pair<Brush>, string>(color, (isSymbol ? tagSplit[1].ToSymbol() : tagSplit[1])));
        }
        catch
        {
          return defaultRes;
        }
      }

      return result.ToArray();
    }
    else
      return defaultRes;
  }
  
  public static string GetStringOfFText(this Screen.Screen screen, string fStr)
  {
    var pairs = screen.GetFTexts(fStr);
    var sb = new StringBuilder();

    foreach (var pair in pairs)
      sb.Append(pair.Y);

    return sb.ToString();
  }

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, Action> queue) =>
    Select(screen, title, queue, string.Empty, null);

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, Action> queue,
    string cancelText, Action? cancelCallBack)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    Select(screen, dict, cancelText, value =>
    {
      if (cancellable && string.IsNullOrEmpty(value))
        cancelCallBack?.Invoke();
      else
        queue[value]();
    });
  }

  public static void Select(this Screen.Screen screen, Dictionary<string, string> queue,
    Action<string> callBack) =>
    Select(screen, queue, null, callBack);

  public static void Select(this Screen.Screen screen, Dictionary<string, string> queue,
    string cancelText, Action<string> callBack)
  {
    if (screen.CanTask)
    {
      screen.CanTask = false;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      int selectingIndex = 0;
      int maxIndex = queue.Count - (cancellable ? 0 : 1);
      string[] options = queue.Keys.ToArray();

      screen.SaveRTF();

      void While()
      {
        screen.LoadRTF();
        screen.Println();

        for (int i = 0; i < queue.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndex)
          {
            color = new(screen.BGColor, screen.FGColor);
          }

          screen.Print($" [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ", color);
          screen.Println();
        }

        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            screen.CanTask = true;
            callBack((((cancellable && selectingIndex == maxIndex) ? null : queue[options[selectingIndex]]) ??
                      string.Empty));
          }
          else if (key == screen.KeySet.Up)
          {
            if (selectingIndex == 0)
              selectingIndex = maxIndex;
            else
              selectingIndex -= 1;
            While();
          }
          else if (key == screen.KeySet.Down)
          {
            if (selectingIndex == maxIndex)
              selectingIndex = 0;
            else
              selectingIndex += 1;
            While();
          }
          else
          {
            While();
          }
        });
      }

      While();
    }
    else
      screen.ThrowWhenCantTask();
  }

  public static void Select2d(this Screen.Screen screen, Dictionary<string, List<string>> queue,
    string cancelText, Action<Pair<int>?> callBack)
  {
    if (screen.CanTask)
    {
      screen.CanTask = false;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      List<string> rows = queue.Keys.ToList();
      Pair<int> selectingIndexes = new();
      Pair<int> maxIndexes = new(rows.Count - (cancellable ? 0 : 1), 0);

      screen.SaveRTF();

      void While()
      {
        screen.LoadRTF();
        screen.Println();

        for (int i = 0; i < rows.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndexes.X)
          {
            color = new(screen.BGColor, screen.FGColor);
          }

          screen.Print("  ");
          screen.Print($" [ {(cancellable && i == rows.Count ? cancelText : rows[i])} ] ", color);
          screen.Print("  ");
        }

        screen.Print("\n");
        if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
          for (int i = 0; i < queue[rows[selectingIndexes.X]].Count; i++)
          {
            Pair<Brush> color = new(screen.FGColor, screen.BGColor);
            if (i == selectingIndexes.Y)
            {
              color = new(screen.BGColor, screen.FGColor);
            }

            screen.Print("    ");
            screen.Print($" [ {queue[rows[selectingIndexes.X]][i]} ] ", color);
            screen.Println();
          }

        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            screen.CanTask = true;
            callBack((!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X) ? selectingIndexes : null));
          }
          else if (key == screen.KeySet.Left)
          {
            if (selectingIndexes.X == 0)
              selectingIndexes.X = maxIndexes.X;
            else
              selectingIndexes.X -= 1;
            selectingIndexes.Y = 0;
            if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
              maxIndexes.Y = queue[rows[selectingIndexes.X]].Count - 1;
            While();
          }
          else if (key == screen.KeySet.Right)
          {
            if (selectingIndexes.X == maxIndexes.X)
              selectingIndexes.X = 0;
            else
              selectingIndexes.X += 1;
            selectingIndexes.Y = 0;
            if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
              maxIndexes.Y = queue[rows[selectingIndexes.X]].Count - 1;
            While();
          }
          else if (key == screen.KeySet.Up)
          {
            if (selectingIndexes.Y == 0)
              selectingIndexes.Y = maxIndexes.Y;
            else
              selectingIndexes.Y -= 1;
            While();
          }
          else if (key == screen.KeySet.Down)
          {
            if (selectingIndexes.Y == maxIndexes.Y)
              selectingIndexes.Y = 0;
            else
              selectingIndexes.Y += 1;
            While();
          }
          else
          {
            While();
          }
        });
      }

      While();
    }
    else
      screen.ThrowWhenCantTask();
  }

  public static void Pause(this Screen.Screen screen, string text, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(callBack);
  }

  public static void Pause(this Screen.Screen screen, Action<Key> callBack) =>
    screen.Pause("계속하려면 아무 키나 누르십시오...", callBack);

  public static void Pause(this Screen.Screen screen, string text, Key press, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(press, callBack);
  }

  public static void Pause(this Screen.Screen screen, Key press, Action<Key> callBack) =>
    screen.Pause($"계속하려면 {press}키를 누르십시오...", press, callBack);

  public static void Println(this Screen.Screen screen, uint count = 1)
  {
    if (count == 0) return;
    var sb = new StringBuilder();
    for (var i = 0; i < count; i++)
      sb.Append('\n');

    screen.Print(sb.ToString());
  }
}