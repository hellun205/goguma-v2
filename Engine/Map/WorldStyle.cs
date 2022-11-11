namespace GogumaV2.Engine.Map;

public struct WorldStyle
{
  public char CornerTopLeft { get; }

  public char CornerTopRight { get; }

  public char CornerBottomLeft { get; }

  public char CornerBottomRight { get; }

  public char LineTop { get; }

  public char LineBottom { get; }

  public char LineLeft { get; }

  public char LineRight { get; }

  public char Empty { get; }

  public WorldStyle(string format = "┌─┐/│ │/└─┘")
  {
    string[] line = format.Split("/");

    CornerTopLeft = line[0].ToCharArray()[0];
    LineTop = line[0].ToCharArray()[1];
    CornerTopRight = line[0].ToCharArray()[2];

    LineLeft = line[1].ToCharArray()[0];
    Empty = line[1].ToCharArray()[1];
    LineRight = line[1].ToCharArray()[2];

    CornerBottomLeft = line[2].ToCharArray()[0];
    LineBottom = line[2].ToCharArray()[1];
    CornerBottomRight = line[2].ToCharArray()[2];
  }
}