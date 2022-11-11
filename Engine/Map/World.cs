using System.Collections.Generic;

namespace GogumaV2.Engine.Map;

public class World
{
  public Pair<byte> Size { get; init; }
  
  public WorldStyle Style { get; set; }
  
  public HashSet<Field.Field> Fields { get; set; }
  
}