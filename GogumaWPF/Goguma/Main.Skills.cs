using GogumaWPF.Engine;
using GogumaWPF.Engine.Skill;

namespace GogumaWPF.Goguma;

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