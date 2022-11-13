using GogumaConsole.Engine;
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
    world.Enter(player);
    void While()
    {
      Clear();
      PrintCanvas(world, $"[ {world.Name} ]\n{world.Descriptions}");
      var key = ReadKey();
      switch (key)
      {
        case ConsoleKey.LeftArrow:
          player.Position = new((byte)(player.Position.X - 1), player.Position.Y);
          break;
        case ConsoleKey.RightArrow:
          player.Position = new((byte)(player.Position.X + 1), player.Position.Y);
          break;
        case ConsoleKey.UpArrow:
          player.Position = new(player.Position.X,(byte)(player.Position.Y - 1));
          break;
        case ConsoleKey.DownArrow:
          player.Position = new(player.Position.X,(byte)(player.Position.Y + 1));
          break;
      }
    }
    While();
  }
}