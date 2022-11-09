using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using GogumaV2.Engine.Item.Consumable;
using GogumaV2.Engine.Item.Equippable;

namespace GogumaV2.Engine.Item;

public abstract partial class Item
{
  public const string Empty = "global:empty";
  
  public static Item Get(string code)
  {
    var item = Items.FirstOrDefault(x => x.Code == code);
    if (code == Empty)
      throw new Exception($"{code} is empty item");
    if (item == null)
      throw new Exception($"invalid item code: {code}");
    return item;
  }

  public static string[] GetCodes() => Items.Select(x => x.Code).ToArray();

  private readonly static HashSet<Item> Items = new HashSet<Item>()
  {
    new EquipmentItem("test:hat", ItemType.EquipmentType.Hat)
    {
      Name = "테스트 모자",
      Description = "테스트 용으로 만들어졌다",
    },
    
    new EquipmentItem("test:t_shirt", ItemType.EquipmentType.Top)
    {
      Name = "티셔츠",
      Description = "아주 평범한 티셔츠이다",
    },
    
    new Potion("test:potion")
    {
      Name = "그냥 포션",
      Description = "마셔봤자 의미가 없을 것 같다",
      Buff = new BuffStats()
      {
        Hp = 1
      }
    }
    
    
  };
}