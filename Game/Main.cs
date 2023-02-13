using System.Windows;
using System.Windows.Input;
using Goguma.Engine;
using Goguma.Engine.Entity;
using Goguma.Engine.Player;
using Goguma.Engine.Entity.Dialog;
using Goguma.Engine.Map;
using Goguma.Screen.Writing;

namespace Goguma.Game;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static Screen.Screen screen;
  public static Player? player = null;
  public static GameObjectManager<IGameObject> GameObjectManager { get; set; } = new GameObjectManager<IGameObject>();

  public static string EmptyCode => GameObjectManager<IGameObject>.Empty;

  public static IGameObject GetManageable(this string code) => GameObjectManager.Get(code);

  public static IGameObject? GetManageableOrDefault(this string code, IGameObject? defaultValue = null)
  {
    try
    {
      return GameObjectManager.Get(code);
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

    // var entity = "entity:test".GetEntity();

    // screen.ShowDialogs(((INeutrality) entity).MeetDialogs, entity, player, () => { });
    // screen.ReadWritingEng(16, str => { MessageBox.Show(str); });
    // player.Inventory.GainItem("item:potion2", 5);
    // player.Inventory.GainItem("item:potion", 56);
    // player.Inventory.GainItem("item:potion3", 15);
    // screen.OpenTrader((ITrader) entity, player, () => { });

    // Screen.Screen.OnKeyPress += (sender, e) =>
    // {
    //   Screen.Screen.MainScreen.Print($"IsSubScreen = {sender.IsSubScreen}, Key = {e.Key}\n");
    // };
    //
    // screen.ReadKey(Key.A,key =>
    // {
    //   screen.OpenSubScreen("test sub screen", new Size(400,350), screen =>
    //   {
    //     screen.Print("Hello World!");
    //     screen.Focus();
    //     screen.ReadKey(Key.Enter, key =>
    //     {
    //       screen.ExitSub("cex");
    //     });
    //   }, result =>
    //   {
    //     screen.Print("Oh! get result:");
    //     screen.Print(result.ToString());
    //   });
    // });
    
  }
}