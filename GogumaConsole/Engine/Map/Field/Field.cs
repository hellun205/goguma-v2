namespace GogumaConsole.Engine.Map.Field;

public abstract class Field : IManageable, ICanvas, ICanvasItem
{
  public string Type => Manager.Types.Field;
  
  public string Code { get; init; }
  
  public string Name { get; set; }
  
  public string Descriptions { get; set; }

  public Pair<byte> CanvasSize { get; set; }

  public Pair<byte> Position { get; set; }
  
  public char Icon { get; init; }

  public IEnumerable<ICanvasItem> CanvasChild { get; set; }
  
  public Pair<byte> StartPosition { get; }
  
  public IEnumerable<Pair<byte>> MoveablePosition { get; }

  protected Field(string worldCode, string code, char icon)
  {
    Code = $"{Type}:{worldCode}.{code}";
    Icon = icon;
  }

}