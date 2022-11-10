using System.Windows;
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
  }

  public static void Start()
  {
    // Player.Load(() =>
    // {
    //   
    // });
    player = new Player("test");
    consoleUtil.Print($"basic attack skill's name: {"skill:basic_attack".GetSkill().Name}");
  }
}