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

      Main.Player.Inventory.GainItem(Item.Get("test:t_shirt"));

      Main.Player.Inventory.Open(() =>
      {
        Print($"selected item is {Main.Player.Inventory.SelectedItem}");
      });
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}