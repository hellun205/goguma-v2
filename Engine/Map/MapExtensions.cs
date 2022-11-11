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

    screen.Clear();
    StringBuilder sb;

    sb = new StringBuilder()
      .Append('┌')
      .Append(canvas.CanvasTitle.GetSep(canvas.CanvasSize.X + 1, '─'))
      .Append('┐')
      .Append('\n');
    screen.Print(sb.ToString());

    for (byte y = 0; y <= canvas.CanvasSize.Y; y++)
    {
      screen.Print("│");
      for (byte x = 0; x <= canvas.CanvasSize.X; x++)
      {
        var item = canvas.CanvasChild.FirstOrDefault(item => item.Position == new Pair<byte>(x, y));

        if (canvas.MovingObject != null && canvas.MovingObject.Position == new Pair<byte>(x, y))
        {
          screen.Print("●", new Pair<Brush>(Brushes.Firebrick, screen.BGColor));
        }
        else
        {
          if (item == null)
            screen.Print(" ");
          else
          {
            var clr = new Pair<Brush>(screen.FGColor, screen.BGColor);
            if (item is IRequirable reqItem)
            {
              clr.X = (reqItem.Check ? Brushes.DarkRed : clr.X);
            }
            screen.Print($"{item.Icon}", clr);
          }
        }
      }

      screen.Print("│  ");
      if (texts.Length > y)
        screen.PrintF(texts[y]);
      screen.Print("\n");
    }

    sb = new StringBuilder()
      .Append('└')
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, '─'))
      .Append('┘')
      .Append('\n');
    screen.Print(sb.ToString());
  }
}