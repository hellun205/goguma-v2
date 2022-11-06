using System;

namespace goguma_v2.Engine.Player;

public struct Stats
{
  public int MaxHp { get; set; }
  public int Hp { get; set; }
  public int Exp { get; set; }

  // TO DO
  public Byte AvoidanceRate { get; set; } // 회피율
}