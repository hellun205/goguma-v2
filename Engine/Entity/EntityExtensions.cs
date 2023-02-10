using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Item;
using GogumaWPF.Goguma;

namespace GogumaWPF.Engine.Entity;

public static class EntityExtensions
{
  public static string[] GetDrops(this IEnumerable<DropItem> dropItems)
  {
    HashSet<string> resList = new();

    foreach (var dropItem in dropItems)
    {
      if (dropItem.Chance > 0)
      {
        var rand = new Random();
        if (dropItem.Chance >= rand.Next(1, 101))
          resList.Add(dropItem.ItemCode);
      }
    }

    return resList.ToArray();
  }

  public static void OpenTrader(this Screen.Screen screen, ITrader trader, Player.Player player, Action callBack)
  {
    const string purchaseTitle = "구매";
    const string sellTitle = "판매";
    Pair<int> maxIndexes = new Pair<int>(45, 17);
    var emptyLen = maxIndexes.X - (purchaseTitle.Length + sellTitle.Length + 6);
    var sb = new StringBuilder();
    Pair<Brush> clrOnSelect = new Pair<Brush>(screen.BGColor, screen.FGColor);
    Pair<Brush> clrOnDefault = new Pair<Brush>(screen.FGColor, screen.BGColor);
    Pair<int> selectedIndexes = new Pair<int>();
    Pair<int> maxSelectIndexes = new Pair<int>(1, 0);
    int scroll = 0;
    var randomFText = trader.DialogWhenTrade.GetRandom();
    int textLen = maxIndexes.X - (trader.Name.Length + 1);
    var defaultText =
      $"<fg='{trader.Color}'>{trader.Name.ToSymbol()}<reset>{":".ToSymbol()}{randomFText.FillEmpty(textLen).ToSymbol()}";
    string npcText = defaultText;
    var purchasableItems = trader.TradingItems.ToArray();
    ItemBundle[] sellableItems;

    screen.CanTask = true;

    void While()
    {
      sellableItems = player.Inventory.Items.Keys.SelectMany(type => player.Inventory.Items[type]).ToArray();
      screen.Clear();
      sb.Clear().Append('┌').Append('─'.While(maxIndexes.X)).Append('┐');
      screen.Print(sb.ToString());
      screen.Println();
      sb.Clear().Append("<reset>│").Append($"{npcText}").Append("│\n");
      screen.PrintF(sb.ToString());
      sb.Clear().Append('├').Append('─'.While(purchaseTitle.Length + 2)).Append('┬')
        .Append('─'.While(sellTitle.Length + 2)).Append('┬').Append('─'.While(emptyLen)).Append("┤\n");
      screen.Print(sb.ToString());
      sb.Clear().Append('│');
      screen.Print(sb.ToString());
      screen.Print($" {purchaseTitle} ".ToSymbol(), (selectedIndexes.X == 0 ? clrOnSelect : clrOnDefault));
      sb.Clear().Append('│');
      screen.Print(sb.ToString());
      screen.Print($" {sellTitle} ".ToSymbol(), (selectedIndexes.X == 1 ? clrOnSelect : clrOnDefault));
      sb.Clear().Append('│');
      if (selectedIndexes.X == 0)
        sb.Append(
          $"{(purchasableItems.Length == 0 ? 0 : selectedIndexes.Y + scroll + 1)}/{purchasableItems.Length} | {player.Inventory.Gold}G 소지 중"
            .FillEmpty(emptyLen).ToSymbol()).Append("│\n");
      else if (selectedIndexes.X == 1)
        sb.Append(
          $"{(sellableItems.Length == 0 ? 0 : selectedIndexes.Y + scroll + 1)}/{sellableItems.Length} | {player.Inventory.Gold}G 소지 중"
            .FillEmpty(emptyLen).ToSymbol()).Append("│\n");

      screen.Print(sb.ToString());
      sb.Clear().Append('├').Append('─'.While(purchaseTitle.Length + 2)).Append('┴')
        .Append('─'.While(sellTitle.Length + 2))
        .Append('┴').Append('─'.While(emptyLen)).Append("┤\n");
      screen.Print(sb.ToString());
      screen.Print("│");
      screen.Print("아이템 이름".FillEmpty(maxIndexes.X - 12).ToSymbol());
      screen.Print("가격".FillEmpty(12).ToSymbol());
      screen.Print("│\n");

      for (var i = 0; i < maxIndexes.Y; i++)
      {
        screen.Print("│");
        if (selectedIndexes.X == 0)
        {
          if (purchasableItems.Length > i)
          {
            var item = purchasableItems[i + scroll].GetItem();
            screen.Print($"[{item.CheckType()}]{item.Name}".FillEmpty(maxIndexes.X - 12).ToSymbol(),
              (selectedIndexes.Y == i ? clrOnSelect : clrOnDefault));
            screen.Print($"{item.PriceOfPurchase} G".FillEmpty(12).ToSymbol(),
              (selectedIndexes.Y == i ? clrOnSelect : clrOnDefault));
          }
          else
            screen.Print("".FillEmpty(maxIndexes.X).ToSymbol());
        }
        else if (selectedIndexes.X == 1)
        {
          if (sellableItems.Length > i)
          {
            var item = sellableItems[i + scroll].Item.GetItem();
            var count = sellableItems[i + scroll].Count;
            screen.Print($"[{item.CheckType()}]{item.Name}{(count > 1 ? $" {count} 개" : "")}"
                .FillEmpty(maxIndexes.X - 12).ToSymbol(),
              (selectedIndexes.Y == i ? clrOnSelect : clrOnDefault));
            screen.Print($"{item.PriceOfSell}G".FillEmpty(12).ToSymbol(),
              (selectedIndexes.Y == i ? clrOnSelect : clrOnDefault));
          }
          else
            screen.Print("".FillEmpty(maxIndexes.X).ToSymbol());
        }

        screen.Print("│\n");
      }

      sb.Clear().Append('└').Append('─'.While(maxIndexes.X)).Append('┘');
      screen.Print(sb.ToString());

      if (selectedIndexes.X == 0)
        maxSelectIndexes.Y = purchasableItems.Length;
      else if (selectedIndexes.X == 1)
        maxSelectIndexes.Y = sellableItems.Length;
      npcText = defaultText;

      screen.CanTask = true;
      screen.ReadKey(key =>
      {
        if (key == screen.KeySet.Left)
        {
          selectedIndexes.X = (selectedIndexes.X - 1 < 0 ? maxSelectIndexes.X : selectedIndexes.X - 1);
          selectedIndexes.Y = 0;
          scroll = 0;
        }
        else if (key == screen.KeySet.Right)
        {
          selectedIndexes.X = (selectedIndexes.X + 1 > maxSelectIndexes.X ? 0 : selectedIndexes.X + 1);
          selectedIndexes.Y = 0;
          scroll = 0;
        }
        else if (key == screen.KeySet.Up)
        {
          if (selectedIndexes.Y == 0 && scroll > 0)
            scroll -= 1;
          else if (selectedIndexes.Y > 0)
            selectedIndexes.Y -= 1;
        }
        else if (key == screen.KeySet.Down)
        {
          if (selectedIndexes.Y == maxIndexes.Y - 1 && selectedIndexes.Y + scroll < maxSelectIndexes.Y - 1)
            scroll += 1;
          else if (selectedIndexes.Y + scroll + 1 < maxSelectIndexes.Y)
            selectedIndexes.Y += 1;
        }
        else if (key == screen.KeySet.Enter)
        {
          if (selectedIndexes.Y >= 0 && selectedIndexes.Y + scroll < maxSelectIndexes.Y)
            if (selectedIndexes.X == 0)
            {
              var item = purchasableItems[selectedIndexes.Y + scroll].GetItem();
              if (player.Inventory.Gold >= item.PriceOfPurchase)
              {
                player.Inventory.Gold -= item.PriceOfPurchase;
                player.Inventory.GainItem(purchasableItems[selectedIndexes.Y + scroll]);
                npcText =
                  $"<fg='{trader.Color}'>{trader.Name.ToSymbol()}<reset>{":".ToSymbol()}{trader.DialogWhenAfterPurchase.GetRandom().FillEmpty(textLen).ToSymbol()}";
              }
              else
              {
                npcText =
                  $"<fg='{trader.Color}'>{trader.Name.ToSymbol()}<reset>{":".ToSymbol()}{trader.DialogWhenLackOfGold.GetRandom().FillEmpty(textLen).ToSymbol()}";
              }
            }
            else if (selectedIndexes.X == 1)
            {
              var item = sellableItems[selectedIndexes.Y + scroll].Item.GetItem();
              player.Inventory.Gold += item.PriceOfSell;
              player.Inventory.LoseItem(sellableItems[selectedIndexes.Y + scroll].Item);
              npcText =
                $"<fg='{trader.Color}'>{trader.Name.ToSymbol()}<reset>{":".ToSymbol()}{trader.DialogWhenAfterSell.GetRandom().FillEmpty(textLen).ToSymbol()}";
            }
        }
        else if (key == screen.KeySet.Exit)
        {
          callBack();
          return;
        }

        While();
      });
    }

    While();
  }
}