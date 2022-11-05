namespace goguma_v2.Engine.Item;

public class Item
{
  public string Name { get; set; }
  public string Type { get; private set; }

  public Item(string type, string name)
  {
    Type = type;
    Name = name;
  }

  public override string ToString() => $"[ {Type} ] {Name}";
}