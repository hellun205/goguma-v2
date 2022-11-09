using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using GogumaV2.Engine.Item;
using GogumaV2.Engine.Player;
using static GogumaV2.ConsoleUtil;

namespace GogumaV2
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

      void Play()
      {
        MainScreen = Screen;
        // Player.Load(() =>
        // {
        //   
        // });
        Main.Player = new Player("test");

        Main.Player.Inventory.GainItem("test:hat", 300);
        Main.Player.Inventory.GainItem("test:hat", 200);
        Main.Player.Inventory.GainItem("test:t_shirt", 2);
        Main.Player.Equipment.EquipItem("test:t_shirt");
        // Main.Player.Equipment.UnEquipItem(ItemType.EquipmentType.Top);
        Main.Player.Inventory.Open(selecttedItem =>
        {
          var item = Item.Get(
            Main.Player.Inventory.Items[selecttedItem.Value.X][
              selecttedItem.Value.Y].Item);
          Main.Player.Equipment.Open(selecttedEquipment => { Print($"you select : {selecttedEquipment}"); });
        });
      }

      if (PrintErrorOnScreen)
      {
        try
        {
          Play();
        }
        catch (Exception ex)
        {
          PrintF(
            $"<fg='{Brushes.DarkRed}' bg='{MainScreen.FGColor}'>\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}");
          Pause(() => { Application.Current.Shutdown(); });
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