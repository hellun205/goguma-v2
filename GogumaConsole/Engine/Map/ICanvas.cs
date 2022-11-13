namespace GogumaConsole.Engine.Map;

public interface ICanvas
{
  public Pair<byte> CanvasSize { get; }
  
  public IEnumerable<ICanvasItem> CanvasChild { get; }

  public Pair<byte> StartPosition { get; }
  
  public IEnumerable<Pair<byte>> MoveablePosition { get; }
}