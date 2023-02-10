namespace GogumaWPF.Engine;

public interface IManageable
{
  public string Name { get; }
  
  public string Type { get; }
  
  public string Code { get; set; }
  
}