using GogumaConsole.Console;
using GogumaConsole.Engine;
using GogumaConsole.Engine.Map;
using GogumaConsole.Engine.Player;
using static GogumaConsole.Console.ConsoleUtil;

namespace GogumaConsole.Goguma;

public static partial class Main
{
  public static Player? player = null;

  public static string EmptyCode => Manager<IManageable>.Empty;

  public static void Initialize()
  {
    InitItemManager();
    InitSkillManager();
    InitFieldManager();
    InitWorldManager();
  }

  public static void Start()
  {
    // Player.Load(() =>
    // {
    //   
    // });
    player = new Player("test");

    var world = "world:test_world".GetWorld();
    player.Position = new Pair<byte>(5, 5);
    player.Enter(world);
    MapExtensions.OpenCanvas(player);
  }
}