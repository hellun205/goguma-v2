namespace GogumaWPF.Engine.Map;

public interface IPositionable
{
  public Pair<byte> Position { get; set; }

  public ICanvas Canvas { get; set; }
  
  public Direction Direction { get; set; }
}