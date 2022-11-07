using System;
using System.Collections.Generic;

namespace goguma_v2.Engine.Player;

[Serializable]
public sealed class Equipment
{
  /// <summary>
  /// Key: Type of Equipment, Value: Code of Item 
  /// </summary>
  public Dictionary<string, string> Items { get; private set; }

  public Equipment(string[] typeOfEquipment)
  {
    Items = new Dictionary<string, string>();

    foreach (string type in typeOfEquipment)
      Items.Add(type, Item.Item.Empty);
  }

  public void Open()
  {
    throw new NotImplementedException();
  }

  public void EquipItem(string itemCode)
  {
    throw new NotImplementedException();
  }

  public void UnEquipItem(string typeOfEquipment)
  {
    throw new NotImplementedException();
  }
}