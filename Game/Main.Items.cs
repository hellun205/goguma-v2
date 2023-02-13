using Goguma.Engine.Item;
using Goguma.Engine.Item.Consumable;
using Goguma.Engine;

namespace Goguma.Game;

public static partial class Main
{
  public static Item GetItem(this string code)
  {
    var get = GameObjectManager.Get(code);
    if (get is Item item)
    {
      return item;
    }
    else
    {
      Engine.GameObjectManager.ThrowGetError("item");
      return null;
    }
  }

  private static void InitItemManager()
  {
    GameObjectManager.AddRange(new []
    {
      new Potion("potion")
      {
        Name = "포션",
        Description = "효능이 별로 없을 것 같은 포션이다.",
        Buff = new BuffStats()
        {
          Hp = 5
        },
        PriceOfPurchase = 50,
        PriceOfSell = 15,
      },
      
      new Potion("potion2")
      {
        Name = "강력한 포션",
        Description = "체력을 무려 5000이나???",
        Buff = new BuffStats()
        {
          Hp = 5000
        },
        PriceOfPurchase = 5000,
        PriceOfSell = 2500,
      },
      new Potion("potion3")
      {
        Name = "헤응 포션",
        Description = "헤으응",
        Buff = new BuffStats()
        {
          Hp = 500000
        },
        PriceOfPurchase = 5000,
        PriceOfSell = 2500,
      },    
      new Potion("potion4")
      {
        Name = "흐음 포션",
        Description = "흐음? 이라는 말이 나올 정도로 효과가 없다.",
        Buff = new BuffStats()
        {
          Hp = 1
        },
        PriceOfPurchase = 5000,
        PriceOfSell = 2500,
      },
      
      // new ...
    });
  }
}