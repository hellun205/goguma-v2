using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using static goguma.ConsoleUtil;

namespace goguma
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

      Select($"<bg='{Brushes.Black}'> 오늘 기분이 어떠신가요? ", new Dictionary<string, Action>() {
        { "좋아용", () => { Print("\n그래용"); } },
        { "시러용", () => { Print("\n시러용?"); } },
        { "심심해용", () => { Print("\n놀아줄까용?"); } },
        { "흐음", () => { Print("\n??"); } },
        { "몰라용", () => { Print("\n잘생각해봐용~"); } },
      });
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}
