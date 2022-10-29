using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
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
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}
