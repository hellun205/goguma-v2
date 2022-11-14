using System.Drawing;
using System.Text;
using GogumaConsole.Console;
using GogumaConsole.Engine.Map.Field;
using static GogumaConsole.Console.ConsoleUtil;

namespace GogumaConsole.Engine.Map;

public static class MapExtensions
{
  private static ICanvas lastCanvas;
  private static Direction lastDir;
  private static Pair<byte> lastPos;

  public static char GetIcon(this Direction direction)
  {
    return direction switch
    {
      Direction.UP => '▲',
      Direction.DOWN => '▼',
      Direction.LEFT => '◀',
      Direction.RIGHT => '▶',
    };
  }

  public static Direction GetOpposite(this Direction direction)
  {
    return direction switch
    {
      Direction.UP => Direction.DOWN,
      Direction.DOWN => Direction.UP,
      Direction.LEFT => Direction.RIGHT,
      Direction.RIGHT => Direction.LEFT,
    };
  }

  public static void Enter(this IPositionable movingObj, ICanvas canvas)
  {
    lastCanvas = canvas;
    lastDir = movingObj.Direction;
    lastPos = movingObj.Position;
    movingObj.Canvas = canvas;
    movingObj.Position = canvas.StartPosition;
    movingObj.Direction = canvas.StartDirection;
  }

  public static void Leave(this IPositionable movingObj)
  {
    movingObj.Canvas = lastCanvas;
    movingObj.Direction = lastDir.GetOpposite();
    movingObj.Position = lastPos;
  }

  public static void PrintCanvas(IPositionable movingObj, string textF = "") => PrintCanvas(movingObj, null, textF);

  public static void PrintCanvas(IPositionable movingObj, Pair<byte>? selectPosition, string textF = "")
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
        else if (moveable) // if moveable position
          Print("• ",
            new Pair<ConsoleColor>(Color.DimGray.ToConsoleColor(), ConsoleUtil.DefaultColor.Y));
        else
        {
          if (item == null) // if nothing item
            Print("  ");
          else // if has item
          {
            Pair<ConsoleColor> clr = new(item.Color, DefaultColor.Y);
            if (selectPosition != null && selectPosition == item.Position &&
                item is IRequirable reqItem) // check requirable
              clr.X = (reqItem.Check ? Color.DarkGreen.ToConsoleColor() : Color.DarkRed.ToConsoleColor());

            Print($"{item.Icon} ", clr);
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

  public static ICanvasItem OpenCanvas(IPositionable movingObj)
  {
    ICanvas canvas = movingObj.Canvas;
    string text = "";
    Pair<byte> tempPos = movingObj.Position;

    bool CheckMoveable(Pair<byte> position)
    {
      return canvas.MoveablePosition.Contains(position) &&
             canvas.CanvasChild.FirstOrDefault(x => x.Position == position) == null &&
             position.X >= 0 && position.Y >= 0 &&
             position.X <= canvas.CanvasSize.X && position.Y <= canvas.CanvasSize.Y;
    }

    ICanvasItem TryGetItem(Pair<byte> position) => canvas.CanvasChild.FirstOrDefault(x => x.Position == position);

    while (true)
    {
      PrintCanvas(movingObj, tempPos, text);

      var key = ReadKey();

      Pair<byte> position = movingObj.Position;

      if (key == KeySet.Enter)
      {
        var res = TryGetItem(tempPos);
        if (res != null)
          return res;
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

      var item = TryGetItem(position);
      text = (item != null ? item.CanvasDescriptions : string.Empty);
      tempPos = position;
    }
  }
}