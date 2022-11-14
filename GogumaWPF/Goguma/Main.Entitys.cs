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
    return EntityManager.Get(code);
  }

  public static Manager<Engine.Entity.Entity> EntityManager = new Manager<Engine.Entity.Entity>();
  
  private static void InitEntityManager()
  {
    EntityManager.AddRange(new []
    {
      new TestNPC("test")
      {
        Name = "TEST",
        Color = Brushes.Yellow,
        Descriptions = "testing dialogs",
        Position = new(3,3),
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
            Options = new []
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
      }
    });
  }
}