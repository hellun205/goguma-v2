using Goguma.Engine.Skill;
using Goguma.Engine;

namespace Goguma.Game;

public static partial class Main
{
  public static Skill GetSkill(this string code)
  {
    var get = GameObjectManager.Get(code);
    if (get is Skill skill)
    {
      return skill;
    }
    else
    {
      Engine.GameObjectManager.ThrowGetError("entity");
      return null;
    }
  }
  
  private static void InitSkillManager()
  {
    GameObjectManager.AddRange(new []
    {
      new BasicAttack(),
      
      // new ...
    });
  }
}