using GogumaConsole.Engine.Map.Field;

namespace GogumaConsole.Goguma.Field;

public class TestField : Engine.Map.Field.Field, IRequirable
{
  public TestField(string worldCode, string code, char icon) : base(worldCode, code, icon)
  {
    
  }

  public Requirement Requirement { get; }

  public bool Check => true;
}