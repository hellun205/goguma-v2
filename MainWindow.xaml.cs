using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using goguma_v2.Engine.Item;
using goguma_v2.Engine.Player;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      MainScreen = Screen;
      // Player.Load(() =>
      // {
      //   
      // });

      Main.Player = new Player("test");

      Main.Player.Inventory.GainItem("test:hat", 300);
      Main.Player.Inventory.GainItem("test:hat", 200);
      Main.Player.Inventory.LoseItem("test:hat", 10);
      Main.Player.Inventory.Open(() =>
      {
        var item = Item.Get(Main.Player.Inventory.Items[Main.Player.Inventory.SelectedItem.Value.X][Main.Player.Inventory.SelectedItem.Value.Y].Item);
        Print($"selected item is {item}\n{item.Name}\n{item.Description}");
      });
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}