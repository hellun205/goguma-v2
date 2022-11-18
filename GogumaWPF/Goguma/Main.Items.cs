using GogumaWPF.Engine;
using GogumaWPF.Engine.Item;
using GogumaWPF.Engine.Item.Consumable;

namespace GogumaWPF.Goguma;

public static partial class Main
{
  public static Item GetItem(this string code)
  {
    var get = Manager.Get(code);
    if (get is Item item)
    {
      return item;
    }
    else
    {
      Engine.Manager.ThrowGetError("item");
      return null;
    }
  }

  private static void InitItemManager()
  {
    Manager.AddRange(new []
    {
      new Potion("potion")
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