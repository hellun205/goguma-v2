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
      
      Main.Player.Inventory.GainItem(new Item("장비", "장비 1"));
      Main.Player.Inventory.GainItem(new Item("장비", "장비 2"));
      Main.Player.Inventory.GainItem(new Item("장비", "장비 3"));
      Main.Player.Inventory.GainItem(new Item("소비", "소비 1"));
      Main.Player.Inventory.GainItem(new Item("소비", "소비 2"));
      Main.Player.Inventory.GainItem(new Item("기타", "기타 1"));
      Main.Player.Inventory.GainItem(new Item("기타", "기타 2"));
      Main.Player.Inventory.GainItem(new Item("기타", "기타 3"));

      Main.Player.Inventory.Open(() => { Print($"selected item is {Main.Player.Inventory.SelectedItem}"); });
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}