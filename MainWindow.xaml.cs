using System.Collections.Generic;
using System.Windows;
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

      //Select($"<bg='{Brushes.Black}'> 오늘 기분이 어떠신가요? ", new Dictionary<string, Action>() {
      //  { "좋아용", () => { Print("\n그래용"); } },
      //  { "시러용", () => { Print("\n시러용?"); } },
      //  { "심심해용", () => { Print("\n놀아줄까용?"); } },
      //  { "흐음", () => { Print("\n??"); } },
      //  { "몰라용", () => { Print("\n잘생각해봐용~"); } },
      //});

      void While()
      {
        Select2d("테스트", new Dictionary<string, List<string>>()
      {
        { "장비", new() { "e1", "e2", "e3", "e4" } },
        { "소비", new() { "c1", "c2", "c3", "c4", "c5" } },
        { "기타", new() { "o1", "o2" } }
      }, true, () =>
      {
        Clear();
        Print($"너는 {Selection2d.X}와 {Selection2d.Y}를 선택했군 !");
        ReadKey(() =>
        {
          While();
        });
      });
      }

      While();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}
