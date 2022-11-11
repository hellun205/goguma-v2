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
    // WorldManager.AddRange(new []
    // {
    //
    // });
  }
}