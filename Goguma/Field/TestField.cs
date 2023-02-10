using GogumaWPF.Engine.Map.Field;

namespace GogumaWPF.Goguma.Field;

public class TestField : Engine.Map.Field.Field, IRequirable
{
  public TestField(string worldCode, string code, char icon) : base(worldCode, code, icon)
  {
    
  }

  public Requirement Requirement { get; }

  public bool Check => true;
}