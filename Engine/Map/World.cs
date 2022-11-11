using System.Collections.Generic;
using System.Linq;
using GogumaV2.Goguma;

namespace GogumaV2.Engine.Map;

public sealed class World : IManageable, ICanvas
{
  public string Type => Manager.Types.World;
  
  public string Name { get; set; }
  
  public string Code { get; init; }

  public string CanvasTitle { get; set; }
  
  public Pair<byte> CanvasSize { get; set; }

  public IEnumerable<ICanvasItem> CanvasChild => Fields.Values.Select(fieldCode => fieldCode.GetField());
  
  public IPositionable? MovingObject { get; private set; }

  /// <summary>
  /// Key: Location of Field, Value: Field Code
  /// </summary>
  public Dictionary<Pair<byte>, string> Fields { get; set; }

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