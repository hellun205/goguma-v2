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
    public static Brush SelectFGColor => MainScreen.BGColor;
    public static Brush SelectBGColor => MainScreen.FGColor;
    public static Brush SelectNormalFGColor => MainScreen.FGColor;
    public static Brush SelectNormalBGColor => MainScreen.BGColor;

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

    public static void Print(string text, Brush fgColor, Brush bgColor) => MainScreen.Print(text, fgColor, bgColor);

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
            Brush fgColor = MainScreen.FGColor;
            Brush bgColor = MainScreen.BGColor;

            if (tagSplit[0].Contains("fg='"))
            {
              fgColor = (Brush)new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
            }
            if (tagSplit[0].Contains("bg='"))
            {
              bgColor = (Brush)new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
            }
            Print(tagSplit[1], fgColor, bgColor);
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
      if (!isSelecting)
      {
        isSelecting = true;
        int selectingIndex = 0;
        int maxIndex = queue.Count - 2;
        List<string> options = queue.Keys.ToList();
        List<Action> actions = queue.Values.ToList();

        void Refresh()
        {
          Clear();
          PrintF(title);
          Print("\n");

          for (int i = 0; i < queue.Count - 1; i++)
          {
            Brush fg = SelectNormalFGColor;
            Brush bg = SelectNormalBGColor;
            if (i == selectingIndex)
            {
              fg = SelectFGColor;
              bg = SelectBGColor;
            }

            Print($" [ {options[i]} ] ", fg, bg);
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
              actions[selectingIndex]();
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
  }
}
