using System;
using System.Collections.Generic;
using System.Linq;

namespace GogumaV2.Engine.Skill;

[Serializable]
public class SkillManager
{
  public HashSet<Skill> Skills { get; set; } = new HashSet<Skill>();

  public string[] GetCodes => Skills.Select(x => x.Code).ToArray();
  
  public const string Empty = "skill:empty";
  
  public Skill Get(string code)
  {
    var skill = Skills.FirstOrDefault(x => x.Code == code);
    if (code == Empty)
      throw new Exception($"{code} is empty skill");
    if (skill == null)
      throw new Exception($"invalid skill code: {code}");
    return skill;
  }
}