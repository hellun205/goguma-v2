using System.Windows.Media;

namespace GogumaWPF.Engine.Map;

public interface ICanvasItem
{
  public Pair<byte> Position { get; }
  
  public char Icon { get; }
  
  public string CanvasDescriptions { get; }
  
  public Brush Color { get; }
}