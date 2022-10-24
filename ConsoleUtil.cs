using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace goguma
{
  public static class ConsoleUtil
  {
    public static Screen MainScreen { get; set; }
    public static string Text { get; private set; } = "";
    public static Key Key { get; private set; }

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
              fgColor = (SolidColorBrush)new BrushConverter().ConvertFromString(tagSplit[0].Split("fg='")[1].Split("'")[0]);
            }
            if (tagSplit[0].Contains("bg='"))
            {
              bgColor = (SolidColorBrush)new BrushConverter().ConvertFromString(tagSplit[0].Split("bg='")[1].Split("'")[0]);
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
  }
}
