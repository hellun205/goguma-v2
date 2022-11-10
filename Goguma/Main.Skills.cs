using GogumaV2.Engine;
using GogumaV2.Engine.Skill;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static Skill GetSkill(this string code)
  {
    return SkillManager.Get(code);
  }

  public static Manager<Skill> SkillManager = new Manager<Skill>();
  
  private static void InitSkillManager()
  {
    SkillManager.AddRange(new []
    {
      new BasicAttack(),
      
      // new ...
    });
  }
}