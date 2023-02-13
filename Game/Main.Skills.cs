using Goguma.Engine.Skill;
using Goguma.Engine;

namespace Goguma.Game;

public static partial class Main
{
  public static Skill GetSkill(this string code)
  {
    var get = Manager.Get(code);
    if (get is Skill skill)
    {
      return skill;
    }
    else
    {
      Engine.Manager.ThrowGetError("entity");
      return null;
    }
  }
  
  private static void InitSkillManager()
  {
    Manager.AddRange(new []
    {
      new BasicAttack(),
      
      // new ...
    });
  }
}