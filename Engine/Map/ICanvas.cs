using System.Collections.Generic;

namespace Goguma.Engine.Map;

public interface ICanvas
{
  public Pair<byte> CanvasSize { get; }
  
  public IEnumerable<ICanvasItem> CanvasChild { get; }

  public Pair<byte> StartPosition { get; }
  
  public IEnumerable<Pair<byte>> MoveablePosition { get; }
  
  public Direction StartDirection { get; }
}