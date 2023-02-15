using System;
using System.Collections.Generic;
using System.Linq;
using Goguma.Game;

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

  public static void OpenTrader(this ITrader trader, Screen.Screen screen, Action callBack)
  {
    
  }
}