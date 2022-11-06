using System;
using System.Windows.Media;

namespace goguma_v2.Engine.Item;

[Serializable]
public abstract class OtherItem : Item
{
  public override string Type => ItemType.OtherItem;

  public override void OnUse()
  {
    throw new System.NotImplementedException();
  }

  public OtherItem(string code) : base(code)
  {
    
  }


}