namespace goguma_v2.Engine.Item;

public interface IConsumable
{
  public delegate void _OnUse(object sender);
  
  public string ComsumptionType { get; }
  
  public BuffStats Buff { get; set; }

  public event _OnUse OnUse;

  public void Use();
}