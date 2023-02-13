using Goguma.Engine.Map.Field;

namespace Goguma.Game.Field;

public class TestField : Engine.Map.Field.Field, IRequirable
{
  public TestField(string worldCode, string code, char icon) : base(worldCode, code, icon)
  {
    
  }

  public Requirement Requirement { get; }

  public bool Check => true;
}