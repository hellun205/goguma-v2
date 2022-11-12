using System.Collections.Generic;

namespace GogumaWPF.Engine.Map;

public interface ICanvas
{
  public Pair<byte> CanvasSize { get; }
  
  public IEnumerable<ICanvasItem> CanvasChild { get; }

  public IPositionable? MovingObject { get; }
}