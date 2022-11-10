using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GogumaV2.Engine.Item;
using GogumaV2.Engine.Player;
using static GogumaV2.Goguma.Main;

namespace GogumaV2
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
          consoleUtil.Print(
            $"\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}",
            new Pair<Brush>(Brushes.DarkRed, consoleUtil.MainScreen.FGColor));
          consoleUtil.Pause(key => { Application.Current.Shutdown(); });
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