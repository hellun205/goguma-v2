namespace goguma_v2.Engine.Item;

public interface IPurchasable
{
  public delegate void _OnPurchase(object sender, uint price);
  
  public uint PriceOfPurchase { get; set; }
  
  public event _OnPurchase OnPurchase;

  public void Purchase(Player.Player player);
}