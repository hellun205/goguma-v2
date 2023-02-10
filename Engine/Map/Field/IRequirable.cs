namespace GogumaWPF.Engine.Map.Field;

public interface IRequirable
{
  public Requirement Requirement { get; }
  
  public bool Check { get; }
}