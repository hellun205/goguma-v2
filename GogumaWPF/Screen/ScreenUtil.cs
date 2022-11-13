using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GogumaWPF.Engine.Map;
using GogumaWPF.Screen;

namespace GogumaWPF;

public static class ScreenUtil
{
  private static bool isSelecting = false;

  public static void PrintF(this Screen.Screen screen, string formattedText)
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

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, Action> queue) => Select(screen, title, queue, string.Empty, null);

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, Action> queue, string cancelText, Action? cancelCallBack)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    Select(screen, title, dict, cancelText, value =>
    {
      if (cancellable && string.IsNullOrEmpty(value))
        cancelCallBack?.Invoke();
      else
        queue[value]();
    });
  }

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, string> queue, Action<string> callBack) =>
    Select(screen, title, queue, null, callBack);

  public static void Select(this Screen.Screen screen, string title, Dictionary<string, string> queue, string cancelText, Action<string> callBack)
  {
    if (!isSelecting)
    {
      isSelecting = true;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      int selectingIndex = 0;
      int maxIndex = queue.Count - (cancellable ? 0 : 1);
      string[] options = queue.Keys.ToArray();

      void While()
      {
        screen.Clear();
        screen.PrintF(title);
        screen.Println();

        for (int i = 0; i < queue.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndex)
          {
            color = new(screen.BGColor,screen.FGColor);
          }

          screen.Print($" [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ", color);
          screen.Println();
        }

        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            isSelecting = false;
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
    else throw new Exception("이미 선택중 입니다.");
  }

  public static void Select2d(this Screen.Screen screen, string title, Dictionary<string, List<string>> queue, string cancelText,
    Action<Pair<int>?> callBack)
  {
    if (!isSelecting)
    {
      isSelecting = true;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      List<string> rows = queue.Keys.ToList();
      Pair<int> selectingIndexs = new();
      Pair<int> maxIndexs = new(rows.Count - (cancellable ? 0 : 1), 0);

      void While()
      {
        screen.Clear();
        screen.PrintF($"{title}\n\n");

        for (int i = 0; i < rows.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndexs.X)
          {
            color = new(screen.BGColor, screen.FGColor);
          }

          screen.Print("  ");
          screen.Print($" [ {(cancellable && i == rows.Count ? cancelText : rows[i])} ] ", color);
          screen.Print("  ");
        }

        screen.Print("\n");
        if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
          for (int i = 0; i < queue[rows[selectingIndexs.X]].Count; i++)
          {
            Pair<Brush> color = new(screen.FGColor, screen.BGColor);
            if (i == selectingIndexs.Y)
            {
              color = new(screen.BGColor, screen.FGColor);
            }

            screen.Print("    ");
            screen.Print($" [ {queue[rows[selectingIndexs.X]][i]} ] ", color);
            screen.Println();
          }

        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            isSelecting = false;
            callBack((!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X) ? selectingIndexs : null));
          }
          else if (key == screen.KeySet.Left)
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
          else if (key == screen.KeySet.Right)
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
          else if (key == screen.KeySet.Up)
          {
            if (selectingIndexs.Y == 0)
              selectingIndexs.Y = maxIndexs.Y;
            else
              selectingIndexs.Y -= 1;
            While();
          }
          else if (key == screen.KeySet.Down)
          {
            if (selectingIndexs.Y == maxIndexs.Y)
              selectingIndexs.Y = 0;
            else
              selectingIndexs.Y += 1;
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
    else throw new Exception("이미 선택중 입니다.");
  }

  public static void Pause(this Screen.Screen screen, string text, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(callBack);
  }

  public static void Pause(this Screen.Screen screen, Action<Key> callBack) => screen.Pause("계속하려면 아무 키나 누르십시오...", callBack);

  public static void Pause(this Screen.Screen screen, string text, Key press, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(press, callBack);
  }

  public static void Pause(this Screen.Screen screen, Key press, Action<Key> callBack) => screen.Pause($"계속하려면 {press}키를 누르십시오...", press, callBack);

  public static void Println(this Screen.Screen screen) => screen.Print("\n");
}