using System.Windows;
using System.Windows.Input;
using GogumaWPF.Engine;
using GogumaWPF.Engine.Player;

namespace GogumaWPF.Goguma;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static ScreenUtil ScreenUtil;
  public static Player? player = null;

  public static string EmptyCode => Manager<IManageable>.Empty;

  public static void Initialize(Screen.Screen screen)
  {
    ScreenUtil = new ScreenUtil(screen);
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
      ScreenUtil.Clear();
      ScreenUtil.PrintCanvas(world, $"[ {world.Name} ]\n{world.Descriptions}");
      ScreenUtil.ReadKey(key =>
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