﻿using System;
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
using Goguma.Game;
using static Goguma.Game.Main;

namespace Goguma
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const bool PrintErrorOnScreen = false;

    public MainWindow()
    {
      InitializeComponent();
      Main.screen = Screen;
      
      void Play()
      {
        Screen.Focus();
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
  }
}