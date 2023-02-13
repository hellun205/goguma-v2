namespace Goguma.Engine.Item;

public interface ITradable : IManageable
{
  public delegate void _OnPurchase(object sender, uint price);
  
  public delegate void _OnSell(object sender, uint price);
  
  public uint PriceOfPurchase { get; }
  
  public uint PriceOfSell { get; }
  
  public event _OnPurchase OnPurchase;
  
  public event _OnSell OnSell;

  public void Purchase();
  
  public void Sell();
}