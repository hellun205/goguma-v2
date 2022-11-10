using System;
using System.Collections.Generic;
using System.Linq;

namespace GogumaV2.Engine.Skill;

[Serializable]
public sealed class SkillManager
{
  public HashSet<Skill> Skills { get; private set; } = new HashSet<Skill>();

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

  public void Add(Skill skill)
  {
    if (Skills.FirstOrDefault(x => x.Code == skill.Code) == null)
      Skills.Add(skill);
    else
      throw new Exception($"skill code already exists: {skill.Code}");
  }

  public void AddRange(IEnumerable<Skill> skills)
  {
    foreach (var skill in skills)
      Add(skill);
  }
}