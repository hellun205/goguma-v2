using GogumaWPF.Engine;
using GogumaWPF.Engine.Skill;

namespace GogumaWPF.Goguma;

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