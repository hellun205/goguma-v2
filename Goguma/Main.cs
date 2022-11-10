using System.Windows;
using GogumaV2.Engine.Player;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static ConsoleUtil consoleUtil;
  public static Player? player = null;

  public static void Initialize(Screen screen)
  {
    consoleUtil = new ConsoleUtil(screen);
    InitItemManager();
    InitSkillManager();
  }
}