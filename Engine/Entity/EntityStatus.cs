namespace Goguma.Engine.Entity;

public struct EntityStatus
{
  public uint MaxHp { get; set; }
  
  public uint Hp { get; set; } 
  
  public uint Exp { get; set; } 
  
  public byte AvoidanceRate { get; set; }

  public short Armor { get; set; } 
  
  public short CritArmor { get; set; } 
  
  public byte CritPer { get; set; } 
  
  public byte CritDmg { get; set; } 
  
  public short ArmorPenetration { get; set; }
}