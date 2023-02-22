using System.Collections.Generic;

namespace Goguma.Engine.Entity;

public interface ITrader : INeutrality
{
  public string TraderType { get; }
  
  public IEnumerable<string> TradingItems { get; }
  
  public IEnumerable<string>? DialogWhenTrade { get; }
  
  public IEnumerable<string>? DialogWhenAfterPurchase { get; }
  
  public IEnumerable<string>? DialogWhenAfterSell { get; }
  
  public IEnumerable<string>? DialogWhenLackOfGold { get; }
}