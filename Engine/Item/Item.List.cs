using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using goguma_v2.Engine.Item.Consumable;
using goguma_v2.Engine.Item.Equippable;

namespace goguma_v2.Engine.Item;

public abstract partial class Item
{
  public static Item Get(string code)
  {
    var item = Items.FirstOrDefault(x => x.Code == code);
    if (item == null)
      MessageBox.Show($"invalid item code: {code}", "error");
    
    return item;
  }

  public static string[] GetCodes() => Items.Select(x => x.Code).ToArray();

  private static HashSet<Item> Items = new HashSet<Item>()
  {
    new EquipmentItem("test:hat", ItemType.EquipmentType.Hat)
    {
      Name = "테스트 모자",
      Description = "테스트 용으로 만들어졌다",
    },
    
    new EquipmentItem("test:hat", ItemType.EquipmentType.Hat)
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