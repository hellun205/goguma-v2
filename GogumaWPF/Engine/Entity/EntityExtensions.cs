using System;
using System.Collections.Generic;
using GogumaWPF.Engine.Entity.Dialog;

namespace GogumaWPF.Engine.Entity;

public static class EntityExtensions
{
  public static string[] GetDrops(this IEnumerable<DropItem> dropItems)
  {
    List<string> resList = new();
    
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
}