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
        consoleUtil = new ConsoleUtil(Screen);
        // Player.Load(() =>
        // {
        //   
        // });
        player = new Player("test");

        player.Inventory.GainItem("test:hat", 300);
        player.Inventory.GainItem("test:hat", 200);
        player.Inventory.GainItem("test:t_shirt", 2);
        player.Equipment.EquipItem("test:t_shirt");
        // player.Equipment.UnEquipItem(ItemType.EquipmentType.Top);
        player.Inventory.Open(selecttedItem =>
        {
          Item item = null;
          if (selecttedItem != null)
          {
            item = Item.Get(
              player.Inventory.Items[selecttedItem.Value.X][
                selecttedItem.Value.Y].Item);
          }

          player.Equipment.Open(selecttedEquipment =>
          {
            consoleUtil.Print($"you select : {selecttedEquipment}");
            consoleUtil.ReadText(value => { consoleUtil.Print($"Select: {value}"); });
          });
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
          consoleUtil.PrintF(
            $"<fg='{Brushes.DarkRed}' bg='{consoleUtil.MainScreen.FGColor}'>\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}");
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