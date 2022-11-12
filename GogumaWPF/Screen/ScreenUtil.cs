using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using GogumaWPF.Engine.Map;

namespace GogumaWPF;

public class ScreenUtil
{
  public Screen.Screen MainScreen { get; init; }

  public Pair<Brush> ColorOnSelect;

  public Pair<Brush> ColorOnNoSelect;

  private bool isSelecting = false;

  public ScreenUtil(Screen.Screen screen)
  {
    MainScreen = screen;
    ColorOnSelect = new(MainScreen.BGColor, MainScreen.FGColor);
    ColorOnNoSelect = new(MainScreen.FGColor, MainScreen.BGColor);
  }

  public void ReadText(Action<string> callBack) => MainScreen.ReadText(callBack);

  public void ReadKey(Action<Key> callBack) => MainScreen.ReadKey(callBack);

  public void ReadKey(Key keyToPress, Action<Key> callBack) => MainScreen.ReadKey(keyToPress, callBack);

  public void Clear() => MainScreen.Clear();

  public void Print(string text) => MainScreen.Print(text);

  public void Print(string text, Pair<Brush> color) => MainScreen.Print(text, color);

  public void PrintF(string formattedText) => MainScreen.PrintF(formattedText);

  public void Select(string title, Dictionary<string, Action> queue) => Select(title, queue, string.Empty, null);

  public void Select(string title, Dictionary<string, Action> queue, string cancelText, Action? cancelCallBack)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    Select(title, dict, cancelText, value =>
    {
      if (cancellable && string.IsNullOrEmpty(value))
        cancelCallBack();
      else
        queue[value]();
    });
  }

  public void Select(string title, Dictionary<string, string> queue, Action<string> callBack) =>
    Select(title, queue, null, callBack);

  public void Select(string title, Dictionary<string, string> queue, string cancelText, Action<string> callBack)
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

          Print($" [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ", color);
          Print("\n");
        }

        ReadKey(key =>
        {
          switch (key)
          {
            case Key.Enter:
              isSelecting = false;
              callBack((((cancellable && selectingIndex == maxIndex) ? null : queue[options[selectingIndex]]) ??
                        string.Empty));
              break;
            case Key.Up:
            {
              if (selectingIndex == 0)
                selectingIndex = maxIndex;
              else
                selectingIndex -= 1;
              While();
              break;
            }
            case Key.Down:
            {
              if (selectingIndex == maxIndex)
                selectingIndex = 0;
              else
                selectingIndex += 1;
              While();
              break;
            }
            default:
              While();
              break;
          }
        });
      }

      While();
    }
    else throw new Exception("이미 선택중 입니다.");
  }

  public void Select2d(string title, Dictionary<string, List<string>> queue, string cancelText,
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
          Print($" [ {(cancellable && i == rows.Count ? cancelText : rows[i])} ] ", color);
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

        ReadKey(key =>
        {
          switch (key)
          {
            case Key.Enter:
              isSelecting = false;
              callBack((!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X) ? selectingIndexs : null));
              break;
            case Key.Left:
            {
              if (selectingIndexs.X == 0)
                selectingIndexs.X = maxIndexs.X;
              else
                selectingIndexs.X -= 1;
              selectingIndexs.Y = 0;
              if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
                maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
              While();
              break;
            }
            case Key.Right:
            {
              if (selectingIndexs.X == maxIndexs.X)
                selectingIndexs.X = 0;
              else
                selectingIndexs.X += 1;
              selectingIndexs.Y = 0;
              if (!cancellable || (cancellable && selectingIndexs.X != maxIndexs.X))
                maxIndexs.Y = queue[rows[selectingIndexs.X]].Count - 1;
              While();
              break;
            }
            case Key.Up:
            {
              if (selectingIndexs.Y == 0)
                selectingIndexs.Y = maxIndexs.Y;
              else
                selectingIndexs.Y -= 1;
              While();
              break;
            }
            case Key.Down:
            {
              if (selectingIndexs.Y == maxIndexs.Y)
                selectingIndexs.Y = 0;
              else
                selectingIndexs.Y += 1;
              While();
              break;
            }
            default:
              While();
              break;
          }
        });
      }

      While();
    }
    else throw new Exception("이미 선택중 입니다.");
  }

  public void Pause(string text, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      Print($"\n{text}\n");
    ReadKey(callBack);
  }

  public void Pause(Action<Key> callBack) => Pause("계속하려면 아무 키나 누르십시오...", callBack);

  public void Pause(string text, Key press, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      Print($"\n{text}\n");
    ReadKey(press, callBack);
  }

  public void Pause(Key press, Action<Key> callBack) => Pause($"계속하려면 {press}키를 누르십시오...", press, callBack);

  public void PrintCanvas(ICanvas canvas, string textF = "") => MainScreen.PrintCanvas(canvas, textF);
}