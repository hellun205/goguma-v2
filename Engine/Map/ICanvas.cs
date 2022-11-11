using System.Collections.Generic;

namespace GogumaV2.Engine.Map;

public interface ICanvas
{
  /// <summary>
  /// Only english
  /// </summary>
  public string CanvasTitle { get; }
  
  public Pair<byte> CanvasSize { get; }
  
  public IEnumerable<ICanvasItem> CanvasChild { get; }

  public IPositionable? MovingObject { get; }
}