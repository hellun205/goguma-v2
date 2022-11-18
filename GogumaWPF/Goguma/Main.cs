using System.Windows;
using System.Windows.Input;
using GogumaWPF.Engine;
using GogumaWPF.Engine.Entity;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Map;
using GogumaWPF.Engine.Player;
using GogumaWPF.Screen.Writing;

namespace GogumaWPF.Goguma;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static Screen.Screen screen;
  public static Player? player = null;
  public static Manager<IManageable> Manager { get; set; } = new Manager<IManageable>();

  public static string EmptyCode => Manager<IManageable>.Empty;

  public static IManageable GetManageable(this string code) => Manager.Get(code);

  public static IManageable? GetManageableOrDefault(this string code, IManageable? defaultValue = null)
  {
    try
    {
      return Manager.Get(code);
    }
    catch
    {
      return defaultValue;
    }
  }

  static Main()
  {
    InitItemManager();
    InitSkillManager();
    InitFieldManager();
    InitWorldManager();
    InitEntityManager();
  }

  public static void Start()
  {
    // Player.Load(() =>
    // {
    //   
    // });
    player = new Player("test");

    // var world = "world:test_world".GetWorld();
    // player.Position = new Pair<byte>(5, 5);
    // player.Enter(world);
    // screen.OpenCanvas(player, field =>
    // {
    //   var fld = field as Field.TestField;
    //   screen.Clear();
    //   screen.Print($"{fld.Icon} {fld.Name}\n{fld.Descriptions}");
    // });

    var entity = "entity:test".GetEntity();

    // screen.ShowDialogs(((INeutrality) entity).MeetDialogs, entity, player, () => { });
    screen.ReadWritingEng(16, str => { MessageBox.Show(str); });
  }
}