using System.Drawing;
using System.Text;
using GogumaConsole.Console;
using GogumaConsole.Engine.Map.Field;
using static GogumaConsole.Console.ConsoleUtil;

namespace GogumaConsole.Engine.Map;

public static class MapExtensions
{
  private const char NothingIcon = '■';

  private static char GetIcon(this Direction direction)
  {
    return direction switch
    {
      Direction.UP => '▲',
      Direction.DOWN => '▼',
      Direction.LEFT => '◀',
      Direction.RIGHT => '▶',
    };
  }

  public static void Enter(this IPositionable movingObj, ICanvas canvas)
  {
    movingObj.Canvas = canvas;
    movingObj.Position = canvas.StartPosition;
    movingObj.Direction = canvas.StartDirection;
  }

  public static void PrintCanvas(IPositionable movingObj, string textF = "")
  {
    string[] texts = textF.Split("\n");
    var canvas = movingObj.Canvas;

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
        var position = new Pair<byte>(x, y);
        var item = canvas.CanvasChild.FirstOrDefault(item => item.Position == position);
        var moveable = canvas.MoveablePosition.Contains(position);

        if (movingObj != null && movingObj.Position == new Pair<byte>(x, y))
        {
          Print($"{movingObj.Direction.GetIcon()} ",
            new Pair<ConsoleColor>(Color.Firebrick.ToConsoleColor(), ConsoleUtil.DefaultColor.Y));
        }
        else if (moveable)
          Print("• ",
            new Pair<ConsoleColor>(Color.DimGray.ToConsoleColor(), ConsoleUtil.DefaultColor.Y));
        else
        {
          if (item == null)
            Print("  ");
          else
          {
            ConsoleColor clr = Color.DarkGreen.ToConsoleColor();
            if (item is IRequirable reqItem)
            {
              clr = (reqItem.Check ? clr : Color.DarkRed.ToConsoleColor());
            }

            Print($"{item.Icon} ", new(clr, DefaultColor.Y));
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

  public static void OpenCanvas(IPositionable movingObj)
  {
    var canvas = movingObj.Canvas;
    var text = "";

    bool CheckMoveable(Pair<byte> position)
    {
      return canvas.MoveablePosition.Contains(position) && position.X >= 0 && position.Y >= 0 &&
             position.X <= canvas.CanvasSize.X && position.Y <= canvas.CanvasSize.Y;
    }

    while (true)
    {
      PrintCanvas(movingObj, text);

      var key = ReadKey();

      Pair<byte> position = movingObj.Position;

      if (key == KeySet.Enter)
      {
      }
      else if (key == KeySet.Up)
      {
        position.Y -= 1;
        movingObj.Direction = Direction.UP;
      }
      else if (key == KeySet.Down)
      {
        position.Y += 1;
        movingObj.Direction = Direction.DOWN;
      }
      else if (key == KeySet.Left)
      {
        position.X -= 1;
        movingObj.Direction = Direction.LEFT;
      }
      else if (key == KeySet.Right)
      {
        position.X += 1;
        movingObj.Direction = Direction.RIGHT;
      }

      if (CheckMoveable(position)) movingObj.Position = position;
    }
  }
}