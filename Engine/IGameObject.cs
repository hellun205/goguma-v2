namespace Goguma.Engine;

public interface IGameObject
{
  public string Name { get; }
  
  public string Type { get; }
  
  public string Code { get; set; }
  
}