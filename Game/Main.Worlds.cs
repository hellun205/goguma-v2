using System.Collections.Generic;
using Goguma.Engine.Map;
using Goguma.Engine;

namespace Goguma.Game;

public static partial class Main
{
  public static World GetWorld(this string code)
  {
    var get = GameObjectManager.Get(code);
    if (get is World world)
    {
      return world;
    }
    else
    {
      Engine.GameObjectManager.ThrowGetError("entity");
      return null;
    }
  }

  private static void InitWorldManager()
  {
    GameObjectManager.AddRange(new[]
    {
      new World("test_world")
      {
        Name = "테스트 월드",
        Descriptions = "캔버스 테스트를 위해 만들어짐.",
        Fields = new[]
        {
          "field:test_world.test_field"
        },
        CanvasSize = new Pair<byte>(14, 10),
        MoveablePosition = new[]
        {
          new Pair<byte>(5, 5),
          new Pair<byte>(5, 4),
          new Pair<byte>(5, 3),
          new Pair<byte>(5, 2),
        },
        StartPosition = new(5, 2),
        StartDirection = Direction.DOWN,
      }
    });
  }
}