using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using GogumaWPF.Engine.Map.Field;

namespace GogumaWPF.Engine.Map;

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

  public static void PrintCanvas(this Screen.Screen screen, IPositionable movingObj, string textF = "")
  {
    string[] texts = textF.Split("\n");
    var canvas = movingObj.Canvas;

    screen.Clear();
    StringBuilder sb;

    sb = new StringBuilder()
      .Append("┌ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┐');
    screen.Print(sb.ToString());
    screen.Print("\n");

    for (byte y = 0; y <= canvas.CanvasSize.Y; y++)
    {
      screen.Print("│ ");
      for (byte x = 0; x <= canvas.CanvasSize.X; x++)
      {
        var position = new Pair<byte>(x, y);
        var item = canvas.CanvasChild.FirstOrDefault(item => item.Position == position);
        var moveable = canvas.MoveablePosition.Contains(position);

        if (movingObj != null && movingObj.Position == new Pair<byte>(x, y))
        {
          screen.Print($"{movingObj.Direction.GetIcon()} ", new Pair<Brush>(Brushes.Firebrick, screen.BGColor));
        }
        else if (moveable)
          screen.Print($"ㆍ ", new Pair<Brush>(Utils.GetARGB(50, 255, 255, 255), screen.BGColor));
        else
        {
          if (item == null)
            screen.Print($"{NothingIcon} ", new Pair<Brush>(Utils.GetARGB(1, 255, 255, 255), screen.BGColor));
          else
          {
            var clr = new Pair<Brush>(Brushes.DarkGreen, screen.BGColor);
            if (item is IRequirable reqItem)
            {
              clr.X = (reqItem.Check ? clr.X : Brushes.DarkRed);
            }

            screen.Print($"{item.Icon} ", clr);
          }
        }
      }

      screen.Print("│  ");
      if (texts.Length > y)
        screen.Print(texts[y]);
      screen.Print("\n");
    }

    sb = new StringBuilder()
      .Append("└ ")
      .Append(string.Empty.GetSep(canvas.CanvasSize.X + 1, "─ "))
      .Append('┘')
      .Append('\n');
    screen.Print(sb.ToString());
  }

  public static void OpenCanvas(this Screen.Screen screen, IPositionable movingObj, Action<ICanvasItem> callBack)
  {
    ICanvas canvas = movingObj.Canvas;
    string text = String.Empty;
    Pair<byte> tempPos = movingObj.Position;

    bool CheckMoveable(Pair<byte> position)
    {
      return canvas.MoveablePosition.Contains(position) && position.X >= 0 && position.Y >= 0 &&
             position.X <= canvas.CanvasSize.X && position.Y <= canvas.CanvasSize.Y;
    }

    ICanvasItem TryGetItem(Pair<byte> position) => canvas.CanvasChild.FirstOrDefault(x => x.Position == position);

    void While()
    {
      screen.PrintCanvas(movingObj, text);

      screen.ReadKey(key =>
      {
        Pair<byte> position = movingObj.Position;

        if (key == screen.KeySet.Enter)
        {
          var res = TryGetItem(tempPos);
          if (res != null)
          {
            callBack(res);
            return;
          }
        }
        else if (key == screen.KeySet.Up)
        {
          position.Y -= 1;
          movingObj.Direction = Direction.UP;
        }
        else if (key == screen.KeySet.Down)
        {
          position.Y += 1;
          movingObj.Direction = Direction.DOWN;
        }
        else if (key == screen.KeySet.Left)
        {
          position.X -= 1;
          movingObj.Direction = Direction.LEFT;
        }
        else if (key == screen.KeySet.Right)
        {
          position.X += 1;
          movingObj.Direction = Direction.RIGHT;
        }

        if (CheckMoveable(position)) movingObj.Position = position;

        var item = TryGetItem(position);
        text = (item != null ? item.CanvasDescriptions : string.Empty);
        tempPos = position;

        While();
      });
    }

    While();
  }
}