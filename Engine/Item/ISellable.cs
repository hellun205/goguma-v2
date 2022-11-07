namespace goguma_v2.Engine.Item;

public interface ISellable
{
  public delegate void _OnSell(object sender, uint price);
  
  public uint PriceOfSell { get; set; }

  public event _OnSell OnSell;
  
  public void Sell(Player.Player player);
}