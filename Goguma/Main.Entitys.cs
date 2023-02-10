using System;
using System.Collections.Generic;
using System.Windows.Media;
using GogumaWPF.Engine;
using GogumaWPF.Engine.Entity;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Skill;
using GogumaWPF.Goguma.Entity;

namespace GogumaWPF.Goguma;

public static partial class Main
{
  public static Engine.Entity.Entity GetEntity(this string code)
  {
    var get = Manager.Get(code);
    if (get is Engine.Entity.Entity entity)
    {
      return entity;
    }
    else
    {
      Engine.Manager.ThrowGetError("entity");
      return null;
    }
  }

  private static void InitEntityManager()
  {
    Manager.AddRange(new[]
    {
      new TestNPC("test")
      {
        Name = "상인",
        Color = Brushes.Yellow,
        Descriptions = "testing dialogs",
        Position = new(3, 3),
        Level = 1,
        CanvasDescriptions = "testing dialogs npc",
        Icon = '●',
        MeetDialogs = new IDialog[]
        {
          new Say(Speaker.ENTITY, "hello?"),
          new Say(Speaker.ENTITY, "my name is TEST!"),
          new Say(Speaker.PLAYER, "hello, TEST NPC."),
          new Select(Speaker.ENTITY, "can you select?")
          {
            Options = new[]
            {
              "yes",
              "no",
              "hmm.."
            }
          },
          new MultiSay(Speaker.ENTITY)
          {
            DefaultText = "...okay.",
            Texts = new Dictionary<string, string>()
            {
              {"yes", "oh yes!"},
              {"no", "..."}
            }
          },
        },
        TradingItems = new string[]
        {
          "item:potion", "item:potion2", "item:potion3", "item:potion4"
        },
        DialogWhenTrade = new string[]
        {
          "안녕하신가!!",
          "어서옵셔!!",
          "맥주!"
        },
        DialogWhenAfterPurchase = new string[]
        {
          "아이고 다음에 또 와주세요 ^^",
          "하하 감삼다!!",
          "더 사달라구 친구!!",
          "헤헤 돈벌었당"
        },
        DialogWhenAfterSell = new string[]
        {
          "나에게 아주 꼭 필요한 물건이었어!",
          "좋은 물건이구만 형씨!",
          "좋았어 이걸 이제 되..팔.."
        },
        DialogWhenLackOfGold = new string[]
        {
          "이봐 친구 돈이 부족하다네",
          "어이 돈이 부족한걸?",
          "친구 돈을 가져와"
        }
      }
    });
  }
}