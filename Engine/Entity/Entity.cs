namespace Goguma.Engine.Entity;

public class Entity : IManageable
{
  public string Type => Manager.Types.Entity;

  public string Code { get; set; }
  
  public string Name { get; set; }
  
  public string Descriptions { get; set; }
  
  public ushort Level { get; set; }

  public Entity(string code)
  {
    this.Init(code);
  }
}