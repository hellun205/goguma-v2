using System;

namespace goguma_v2.Engine.Item.Other;

[Serializable]
public abstract class OtherItem : Item
{
  public override string Type => ItemType.OtherItem;

  public OtherItem(string code) : base(code)
  {
    
  }


}