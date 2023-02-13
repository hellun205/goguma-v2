using System.Windows.Media;

namespace Goguma.Engine.Map;

public interface ICanvasItem
{
  public Pair<byte> Position { get; }
  
  public char Icon { get; }
  
  public string CanvasDescriptions { get; }
  
  public Brush Color { get; }
}