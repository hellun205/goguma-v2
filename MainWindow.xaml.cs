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
    private const bool PrintErrorOnScreen = true;
    private const bool ShowLogger = true;

    public MainWindow()
    {
      InitializeComponent();
      Main.Screen = Screen;
      Goguma.Screen.Screen.ParentGrid = mainGrid;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (ShowLogger)
      {
        Main.Logger.OpenLogger();
        this.Topmost = true;
        this.Topmost = false;
      }

      Screen.Focus();

      if (PrintErrorOnScreen)
      {
        try
        {
          Start();
        }
        catch (Exception ex)
        {
          if (ShowLogger)
          {
            Main.Logger.Error($"{ex.Message}\n{ex.StackTrace ?? ""}");
          }
          else
          {
            Main.Screen.Print(
              $"\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}",
              new Pair<Brush>(Brushes.DarkRed, Main.Screen.FGColor));
            Main.Screen.Pause(key => { Application.Current.Shutdown(); });
          }
        }
      }
      else
        Start();
    }

    private void OnClosed(object? sender, EventArgs e)
    {
      Application.Current.Shutdown(0);
    }

    private void MainWindow_OnLocationChanged(object? sender, EventArgs e)
    {
      Main.Logger.loggerUI.Top = Top;
      Main.Logger.loggerUI.Left = Left + Width;
    }
  }
}