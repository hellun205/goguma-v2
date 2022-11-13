namespace GogumaConsole.Engine.Map;

public interface IPositionable
{
  public Pair<byte> Position { get; }
  
  public ICanvas Canvas { get; set; }

  public void Enter(ICanvas canvas);
}