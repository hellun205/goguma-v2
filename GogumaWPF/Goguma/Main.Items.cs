using GogumaWPF.Engine;
using GogumaWPF.Engine.Item;
using GogumaWPF.Engine.Item.Consumable;

namespace GogumaWPF.Goguma;

public static partial class Main
{
  public static Item GetItem(this string code)
  {
    return ItemManager.Get(code);
  }

  public static Manager<Item> ItemManager = new Manager<Item>();

  private static void InitItemManager()
  {
    ItemManager.AddRange(new []
    {
      new Potion("goguma:potion")
      {
        Name = "포션",
        Description = "효능이 별로 없을 것 같은 포션이다.",
        Buff = new BuffStats()
        {
          Hp = 5
        }
      },
      
      // new ...
    });
  }
}