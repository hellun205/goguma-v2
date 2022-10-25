using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace goguma
{
  public static class ConsoleUtil
  {
    public static Screen MainScreen { get; set; }
    public static string Text { get; private set; } = "";
    public static Key Key { get; private set; }
    public static string Selection { get; private set; } = "";
    public static Pair<int> Selection2d { get; private set; } = new(0, 0);
    public static Pair<Brush> ColorOnSelect => new(MainScreen.BGColor, MainScreen.FGColor);
    public static Pair<Brush> ColorOnNoSelect => new(MainScreen.FGColor, MainScreen.BGColor);

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
              color.X = (Brush)new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
            }
            if (tagSplit[0].Contains("bg='"))
            {
              color.Y = (Brush)new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
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

    public static void Select(string title, Dictionary<string, Action> queue)
    {
      List<string> options = queue.Keys.ToList();
      List<Action> actions = queue.Values.ToList();
      Dictionary<string, string> dict = new Dictionary<string, string>();
      foreach(string option in options)
      {
        dict.Add(option, option);
      }

      Select(title, dict, () =>
      {
        queue[Selection]();
      });
    }

    public static void Select(string title, Dictionary<string, string> queue, Action callBack)
    {
      if (!isSelecting)
      {
        isSelecting = true;
        int selectingIndex = 0;
        int maxIndex = queue.Count - 2;
        List<string> options = queue.Keys.ToList();
        List<string> actions = queue.Values.ToList();

        void Refresh()
        {
          Clear();
          PrintF(title);
          Print("\n");

          for (int i = 0; i < queue.Count - 1; i++)
          {
            Pair<Brush> color = ColorOnNoSelect;
            if (i == selectingIndex)
            {
              color = ColorOnSelect;
            }

            Print($" [ {options[i]} ] ", color);
            Print("\n");
          }
        }

        void While()
        {
          Refresh();
          ReadKey(() =>
          {
            if (Key == Key.Enter)
            {
              Selection = actions[selectingIndex];
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

    public static void Select2d(string title, Dictionary<string, List<string>> queue, Action callBack)
    {

    }
  }
}
