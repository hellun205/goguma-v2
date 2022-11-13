using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GogumaWPF.Goguma;
using static GogumaWPF.Goguma.Main;

namespace GogumaWPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const bool PrintErrorOnScreen = true;

    public MainWindow()
    {
      InitializeComponent();
      Main.screen = Screen;

      void Play()
      {
        Initialize(Screen);
        Start();
      }

      if (PrintErrorOnScreen)
      {
        try
        {
          Play();
        }
        catch (Exception ex)
        {
          Main.screen.Print(
            $"\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}",
            new Pair<Brush>(Brushes.DarkRed, Main.screen.FGColor));
          Main.screen.Pause(key => { Application.Current.Shutdown(); });
        }
      }
      else
        Play();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      Screen.RTBMain.Focus();
    }
  }
}