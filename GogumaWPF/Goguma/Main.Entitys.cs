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
          "item:potion", "item:potion2"
        },
        DialogWhenTrade = new string[]
        {
          "안녕하신가!!"
        },
        DialogWhenAfterPurchase = new string[]
        {
          "thx~"
        },
        DialogWhenAfterSell = new string[]
        {
          "~~"
        }
      }
    });
  }
}