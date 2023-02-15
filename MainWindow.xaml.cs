using System;
using System.Windows;
using System.Windows.Media;
using Goguma.Game;
using Goguma.Screen;
using static Goguma.Game.Main;

namespace Goguma
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private const bool PrintErrorOnScreen = false;

    public MainWindow()
    {
      InitializeComponent();
      Main.screen = Screen;
      Goguma.Screen.Screen.ParentGrid = mainGrid;
    }

    private void StartGame()
    {
      Start();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      Screen.Focus();

      if (PrintErrorOnScreen)
      {
        try
        {
          StartGame();
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
        StartGame();
    }
  }
}