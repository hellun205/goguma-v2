using System.Windows.Media;
using Goguma.Engine.Map.Field;
using Goguma.Engine;
using Goguma.Game.Field;

namespace Goguma.Game;

public static partial class Main
{
  public static Engine.Map.Field.Field GetField(this string code)
  {
    var get = Manager.Get(code);
    if (get is Engine.Map.Field.Field field)
    {
      return field;
    }
    else
    {
      Engine.Manager.ThrowGetError("field");
      return null;
    }
  }

  private static void InitFieldManager()
  {
    Manager.AddRange(new[]
    {
      new TestField("test_world", "test_field", '☆')
      {
        Name = "테스트 필드",
        Descriptions = "테스트용",
        Position = new Pair<byte>(4, 4),
        CanvasSize = new Pair<byte>(10, 10),
        CanvasDescriptions = "canvas item descriptions test",
        Color = Brushes.Gold
      }
    });
  }
}