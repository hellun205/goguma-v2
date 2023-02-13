namespace Goguma.Engine.Entity;

public class Entity : IGameObject
{
  public string Type => GameObjectManager.Types.Entity;

  public string Code { get; set; }
  
  public string Name { get; set; }
  
  public string Descriptions { get; set; }
  
  public ushort Level { get; set; }

  public Entity(string code)
  {
    this.Init(code);
  }
}