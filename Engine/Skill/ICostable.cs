namespace Goguma.Engine.Skill;

public interface ICostable
{
  public CostType CostType { get; set; }
  
  public uint Cost { get; set; }
  
}