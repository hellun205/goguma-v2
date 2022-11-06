using System;

namespace goguma_v2.Engine.Item.Equipable;

public struct BuffStats
{
  private Byte _avoidanceRate;
  private ushort _receiveExpGrowthRate;
  
  
  public int MaxHp { get; set; }
  
  // to do
  public Byte AvoidanceRate
  {
    get => _avoidanceRate;
    set => Util.Limit(0, 100, value);
  }

  public float ReceiveExpGrowthRate
  {
    get => _receiveExpGrowthRate;
    set => Util.LimitF(0f, 10f, Math.Round(value, 1));
  }
  
}