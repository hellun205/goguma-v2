using System.Windows;
using System.Windows.Input;
using GogumaV2.Engine;
using GogumaV2.Engine.Player;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static ConsoleUtil consoleUtil;
  public static Player? player = null;

  public static string EmptyCode => Manager<IManageable>.Empty;

  public static void Initialize(Screen screen)
  {
    consoleUtil = new ConsoleUtil(screen);
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
      consoleUtil.Clear();
      consoleUtil.PrintCanvas(world);
      consoleUtil.ReadKey(key =>
      {
        switch (key)
        {
          case Key.Left:
            player.Position = new((byte)(player.Position.X - 1), player.Position.Y);
            break;
          case Key.Right:
            player.Position = new((byte)(player.Position.X + 1), player.Position.Y);
            break;
          case Key.Up:
            player.Position = new(player.Position.X,(byte)(player.Position.Y - 1));
            break;
          case Key.Down:
            player.Position = new(player.Position.X,(byte)(player.Position.Y + 1));
            break;
        }
        While();
      });
    }
    While();
  }
}