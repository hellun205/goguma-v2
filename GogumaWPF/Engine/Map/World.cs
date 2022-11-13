using System.Collections.Generic;
using System.Linq;
using GogumaWPF.Goguma;

namespace GogumaWPF.Engine.Map;

public sealed class World : IManageable, ICanvas
{
  public string Type => Manager.Types.World;
  
  public string Name { get; set; }
  
  public string Code { get; init; }
  
  public string Descriptions { get; set; }

  public Pair<byte> CanvasSize { get; set; }

  public IEnumerable<ICanvasItem> CanvasChild => Fields.Select(fieldCode => fieldCode.GetField());

  public Pair<byte> StartPosition { get; set; }
  
  public IEnumerable<Pair<byte>> MoveablePosition { get; set; }
  
  public Direction StartDirection { get; set; }

  public IEnumerable<string> Fields { get; set; }

  public World(string code)
  {
    Code = $"{Type}:{code}";
  }
  
}