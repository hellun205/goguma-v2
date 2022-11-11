using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using GogumaV2.Engine.Map.Field;

namespace GogumaV2.Engine.Map;

public static class MapExtensions
{
  public static void PrintCanvas(this Screen screen, ICanvas canvas, string textF = "")
  {
    string[] texts = textF.Split("\n");

    void prints(string txt)
    {
      print(txt, new Pair<Brush>(screen.FGColor, screen.BGColor));
    }

    void print(string txt, Pair<Brush> clr)
    {
      // screen.PrintWithFont(txt, , clr);
      screen.Print(txt, clr);
    }
    
    screen.Clear();
    StringBuilder sb;

    sb = new StringBuilder()
      .Append("┌ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┐');
    prints(sb.ToString());
    prints("\n");

    for (byte y = 0; y <= canvas.CanvasSize.Y; y++)
    {
      prints("│ ");
      for (byte x = 0; x <= canvas.CanvasSize.X; x++)
      {
        var item = canvas.CanvasChild.FirstOrDefault(item => item.Position == new Pair<byte>(x, y));

        if (canvas.MovingObject != null && canvas.MovingObject.Position == new Pair<byte>(x, y))
        {
          print("● ", new Pair<Brush>(Brushes.Firebrick, screen.BGColor));
        }
        else
        {
          if (item == null)
            print("■ ", new Pair<Brush>(new SolidColorBrush(Color.FromArgb(1, 255,255,255)), screen.BGColor));
          else
          {
            var clr = new Pair<Brush>(Brushes.DarkGreen, screen.BGColor);
            if (item is IRequirable reqItem)
            {
              clr.X = (reqItem.Check ? clr.X : Brushes.DarkRed);
            }
            print($"{item.Icon} ", clr);
          }
        }
      }

      prints("│  ");
      if (texts.Length > y)
        prints(texts[y]);
      prints("\n");
    }

    sb = new StringBuilder()
      .Append("└ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┘')
      .Append('\n');
    prints(sb.ToString());
  }
}