﻿using System;
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
    private const bool ShowLogger = false;

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
          Goguma.Screen.Screen.IgnoreKeyPressEvent = true;
          if (ShowLogger)
          {
            Main.Logger.Error($"{ex.Message}\n{ex.StackTrace ?? ""}");
          }
          else
          {
            Goguma.Screen.Screen.MainScreen.OpenSubScreen("ERROR!", new Size(650, 400), errorWindow =>
            {
              errorWindow.Print(
                $"\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}",
                new Pair<Brush>(Brushes.DarkRed, Main.Screen.FGColor));
              
              errorWindow.Pause(key => { Application.Current.Shutdown(); });
            });
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