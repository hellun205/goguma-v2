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
      
      Main.Player.Inventory.Items["장비"].AddRange(new []
      {
        new Item("test 1"),
        new Item("test 2"),
      });
      
      Main.Player.Inventory.Items["소비"].AddRange(new []
      {
        new Item("test 11241414"),
        new Item("test 2125"),
      });
      
      Main.Player.Inventory.Items["기타"].AddRange(new []
      {
        new Item("test 116216ㅂㅈㄴㅁㄹㅇ"),
        new Item("test 2151ㅁㄴ"),
      });

      Main.Player.Inventory.Open(() =>
      {
        Print($"selected item is {Main.Player.Inventory.SelectedItem.Value}");
      });
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}
