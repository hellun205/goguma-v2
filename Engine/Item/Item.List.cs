using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
    new Equipable.Hat("test:hat")
    {
      Name = "테스트 모자",
      Description = "테스트 용으로 만들어졌다",
    },
    
    new Equipable.Top("test:t_shirt")
    {
      Name = "티셔츠",
      Description = "아주 평범한 티셔츠이다",
    },
    
    
  };
}