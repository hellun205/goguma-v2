using System;

namespace goguma_v2.Engine.Player;

[Serializable]
public struct Stats
{
  public static uint GetNextLevelExp(ushort level) =>
    (uint) Math.Floor(0.04 * (level ^ 3) + 0.8 * (level ^ 2) + 2 * level);

  private uint _maxHp;
  private uint _hp;
  private uint _maxMoisture; // = max mp
  private uint _moisture; // = mp
  private ushort _lv;
  private uint _exp;
  private byte _avoidanceRate;
  private double _receiveExpGrowthRate;
  private short _armor;
  private short _critArmor;
  private byte _critPer;
  private byte _critDmg;
  private ushort _armorPenetration;

  public uint MaxHp // 최대 HP
  {
    get => _maxHp;
    set => _maxHp = (uint) Util.Limit(1, uint.MaxValue, value);
  }

  public uint Hp // HP
  {
    get => _hp;
    set => _hp = (uint) Util.Limit(0, MaxHp, value);
  }

  public uint MaxMoisture // 최대 MP
  {
    get => _maxMoisture;
    set => _maxMoisture = (uint) Util.Limit(1, uint.MaxValue, value);
  }

  public uint Moisture // MP
  {
    get => _moisture;
    set => _moisture = (uint) Util.Limit(0, MaxHp, value);
  }

  public ushort Level // 레벨
  {
    get => _lv;
    private set => _lv = (ushort) Util.Limit(1, ushort.MaxValue, value);
  }

  public uint Exp // 경험치
  {
    get => _exp;
    private set => _exp = (uint) Util.Limit(0, uint.MaxValue, value);
  }

  public byte AvoidanceRate // 회피율
  {
    get => _avoidanceRate;
    set => _avoidanceRate = (byte) Util.Limit(0, 100, value);
  }

  public double ReceiveExpGrowthRate // 경험치 획득량 증가
  {
    get => _receiveExpGrowthRate;
    set => _receiveExpGrowthRate = Util.LimitF(0d, 10d, Math.Round(value, 1));
  }

  public short Armor // 방어력 = (+) 1 / (1 + ARMOR * 0.01) / (-) 2 - (1 / (1 - ARMOR * 0.01))
  {
    get => _armor;
    set => _armor = (short) Util.Limit(-400, 400, value);
  }

  public short CritArmor // 치명타 방어력 = (+) 1 / (1 + ARMOR * 0.01) / (-) 2 - (1 / (1 - ARMOR * 0.01))
  {
    get => _critArmor;
    set => _critArmor = (short) Util.Limit(-400, 400, value);
  }
  
  public byte CritPer // 치명타 확률
  {
    get => _critPer;
    set => _critPer = (byte) Util.Limit(0, 100, value);
  }
  
  public byte CritDmg // 치명타 데미지 ( % ) = DMG * (1 + CRITDMG / 100)
  {
    get => _critDmg;
    set => _critDmg = (byte) Util.Limit(0, 100, value);
  }
  
  public ushort ArmorPenetration // 방어력 관통력 = ARMOR - PENETRATION
  {
    get => _armorPenetration;
    set => _armorPenetration = (ushort) Util.Limit(0, 400, value);
  }

  public void GainExp(uint amount)
  {
    uint leftAmount;
    if (Exp + amount >= GetNextLevelExp(Level))
    {
      leftAmount = Exp + amount - GetNextLevelExp(Level);
      Level += 1;
      GainExp(leftAmount);
    }
    else
    {
      Exp += amount;
    }
  }

  public Stats()
  {
    this = default(Stats);
    MaxHp = 10;
    Hp = MaxHp;
    MaxMoisture = 10;
    Moisture = MaxMoisture;
    AvoidanceRate = 0;
    ReceiveExpGrowthRate = 0;
    Armor = 0;
    CritArmor = 0;
    CritPer = 0;
    CritDmg = 0;
    Level = 1;
    ArmorPenetration = 0;

  }
}