using System.Collections.Generic;
using GogumaV2.Engine;
using GogumaV2.Engine.Map;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static World GetWorld(this string code)
  {
    return WorldManager.Get(code);
  }

  public static Manager<World> WorldManager = new Manager<World>();

  private static void InitWorldManager()
  {
    WorldManager.AddRange(new[]
    {
      new World("test_world")
      {
        Name = "테스트 월드",
        Descriptions = "캔버스 테스트를 위해 만들어짐.",
        Fields = new[]
        {
          "field:test_world.test_field"
        },
        CanvasSize = new Pair<byte>(30, 17),
      }
    });
  }
}