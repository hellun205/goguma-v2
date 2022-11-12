using GogumaConsole.Goguma;

namespace GogumaConsole.Engine.Map;

public sealed class World : IManageable, ICanvas
{
  public string Type => Manager.Types.World;
  
  public string Name { get; set; }
  
  public string Code { get; init; }
  
  public string Descriptions { get; set; }

  public Pair<byte> CanvasSize { get; set; }

  public IEnumerable<ICanvasItem> CanvasChild => Fields.Select(fieldCode => fieldCode.GetField());
  
  public IPositionable? MovingObject { get; private set; }
  
  public IEnumerable<string> Fields { get; set; }

  public World(string code)
  {
    Code = $"{Type}:{code}";
  }

  public void Enter(IPositionable player)
  {
    MovingObject = player;
  }

  public void Leave()
  {
    MovingObject = null;
  }
}