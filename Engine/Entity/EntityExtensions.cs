using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Goguma.Engine.Item;
using Goguma.Game;
using Goguma.Screen;

namespace Goguma.Engine.Entity;

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

  public static void OpenTrader(this ITrader trader, Action callBack)
  {
    var screen = Screen.Screen.MainScreen;
    var player = Main.Player;
    if (screen == null) throw new Exception("스크린이 존재하지 않습니다.");
    if (player == null) throw new Exception("플레이어가 존재하지 않습니다.");

    screen.OpenSubScreen("상점", new Size(500, 0), tradeWindow =>
    {
      const int listMaxCount = 10;
      bool isSellMode = false;
      int maxCount = 0;
      int currentIndex = 0;
      int scroll = 0;

      tradeWindow.SetProperSize(500, 5 + listMaxCount);

      void While()
      {
        var tradingItems = trader.TradingItems.ToArray();
        var playerItems = new List<ItemBundle>();

        foreach (var type in player.Inventory.Items.Keys)
        {
          var items = player.Inventory.Items[type];
          if (items.Count > 0) playerItems.AddRange(items);
        }

        tradeWindow.Clear();
        tradeWindow.PrintF(
          $"&{{!}}[ &{{%#FF909098%}}{trader.TraderType}&{{!}} ] &{{%{trader.Color}%}}{trader.Name}");
        tradeWindow.Println(2);
        tradeWindow.Print(" [  구매  ] ", (!isSellMode ? tradeWindow.Colors.ToReversal() : tradeWindow.Colors));
        tradeWindow.Print(" ");
        tradeWindow.Print(" [  판매  ] ", (isSellMode ? tradeWindow.Colors.ToReversal() : tradeWindow.Colors));
        tradeWindow.Println();

        if (isSellMode)
        {
          maxCount = playerItems.Count;

          for (int i = 0; i < listMaxCount; i++)
          {
            tradeWindow.Println();
            if (maxCount <= i + scroll) continue;

            var item = (ITradable) playerItems[i + scroll].Item.GetGameObject();

            tradeWindow.Print($" - {playerItems[i + scroll]} [ {item.PriceOfSell.GetMoneyString()} Ｇ ] ",
              (i == currentIndex ? tradeWindow.Colors.ToReversal() : tradeWindow.Colors));
          }
        }
        else
        {
          maxCount = tradingItems.Length;

          for (int i = 0; i < listMaxCount; i++)
          {
            tradeWindow.Println();
            if (maxCount <= i + scroll) continue;

            var item = (ITradable) tradingItems[i + scroll].GetGameObject();

            tradeWindow.Print($" - {((Item.Item)item).GetDisplay()} [ {item.PriceOfPurchase.GetMoneyString()} Ｇ ] ",
              (i == currentIndex ? tradeWindow.Colors.ToReversal() : tradeWindow.Colors));
          }
        }

        tradeWindow.Println(2);
        tradeWindow.PrintF(
          $"&{{!}}| 목록: {(maxCount == 0 ? "0" : currentIndex + scroll + 1)} / {maxCount} | 현재 &{{%#FF908718%}}{player.Inventory.Gold.GetMoneyString()} Ｇ&{{!}} 보유 중 |");

        void While2()
        {
          tradeWindow.ReadKey(key =>
          {
            if (key == tradeWindow.KeySet.Enter)
            {
              string str = (isSellMode ? "판매" : "구매");

              void AskCount(Action<int?> callBack_)
              {
                var maxCnt = (isSellMode ? playerItems[currentIndex + scroll].Count : byte.MaxValue);
                tradeWindow.SelectInt($"몇 개를 {str}하시겠습니까?", 1, maxCnt,
                  count => callBack_.Invoke(count), 1, str);
              }

              void SellOrPurchase(uint count)
              {
                var item = (ITradable) (isSellMode
                  ? playerItems[currentIndex + scroll].Item.GetGameObject()
                  : tradingItems[currentIndex + scroll].GetGameObject());
                var price = ((isSellMode ? item.PriceOfSell : item.PriceOfPurchase) * count).GetMoneyString();

                tradeWindow.Ask(
                  $"다음 아이템을 {str}하시겠습니까?\n\" {item.Name}{(count == 1 ? "" : $" {count}개")} \" ({price} Ｇ)", result =>
                  {
                    void ShowMsg()
                    {
                      tradeWindow.ShowMessage(
                        $"다음 아이템을 {str}하였습니다.\n\" {item.Name}{(count == 1 ? "" : $" {count}개")} \" ({price} Ｇ)",
                        () =>
                        {
                          if (currentIndex + scroll == maxCount - 1 && scroll > 0) scroll--;
                          else if (currentIndex == maxCount - 1) currentIndex--;
                          While();
                        }, str, 2);
                    }

                    if (result != null && result.Value)
                    {
                      if (isSellMode)
                      {
                        player.Inventory.LoseItem(item.Code, count);
                        player.Inventory.GainGold(item.PriceOfSell * count);
                        ShowMsg();
                      }
                      else
                      {
                        if (player.Inventory.CheckGold(item.PriceOfPurchase * count))
                        {
                          player.Inventory.GainItem(item.Code, count);
                          player.Inventory.LoseGold(item.PriceOfPurchase * count);
                          ShowMsg();
                        }
                        else
                          tradeWindow.ShowMessage($"골드가 부족합니다.", () => While(), str);
                      }
                    }
                    else While();
                  }, str, 2);
              }

              if (isSellMode)
              {
                if (playerItems[currentIndex + scroll].Count == 1)
                {
                  SellOrPurchase(1);
                }
                else
                {
                  AskCount(count =>
                  {
                    if (count != null && count != 0)
                      SellOrPurchase((uint) count);
                    else While();
                  });
                }
              }
              else
              {
                AskCount(count =>
                {
                  if (count != null && count != 0)
                    SellOrPurchase((uint) count);
                  else While();
                });
              }
            }
            else if (key == tradeWindow.KeySet.Exit)
            {
              tradeWindow.Ask("상점을 종료하시겠습니까?", res =>
              {
                if (res.Value) tradeWindow.ExitSub();
                else While2();
              });
            }
            else if (key == tradeWindow.KeySet.Left || key == tradeWindow.KeySet.Right)
            {
              isSellMode = !isSellMode;
              currentIndex = 0;
              scroll = 0;
              While();
            }
            else if (key == tradeWindow.KeySet.Up)
            {
              if (currentIndex == 0 && scroll == 0)
              {
                if (maxCount >= 10)
                {
                  currentIndex = 9;
                  scroll = maxCount - 10;
                }
                else
                {
                  currentIndex = maxCount - 1;
                  scroll = 0;
                }
              }
              else
              {
                if (currentIndex == 0 && scroll > 0)
                {
                  scroll -= 1;
                }
                else if (currentIndex > 0)
                {
                  currentIndex -= 1;
                }
              }

              While();
            }
            else if (key == tradeWindow.KeySet.Down)
            {
              if ((currentIndex == 9 && scroll == maxCount - 10) || (currentIndex + scroll + 1 == maxCount))
              {
                currentIndex = 0;
                scroll = 0;
              }
              else
              {
                if (currentIndex == 9 && scroll < maxCount - 10)
                {
                  scroll += 1;
                }
                else if (currentIndex < 10 && maxCount > currentIndex + scroll + 1)
                {
                  currentIndex += 1;
                }
              }

              While();
            }
            else While2();
          });
        }

        While2();
      }

      While();
    });
  }
}