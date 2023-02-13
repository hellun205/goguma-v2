using System.Collections.Generic;
using Goguma.Engine.Entity.Dialog;
using Goguma.Engine.Item;

namespace Goguma.Engine.Entity;

public interface ITrader : INeutrality
{
  public IEnumerable<string> TradingItems { get; }
  
  public IEnumerable<string> DialogWhenTrade { get; }
  
  public IEnumerable<string> DialogWhenAfterPurchase { get; }
  
  public IEnumerable<string> DialogWhenAfterSell { get; }
  
  public IEnumerable<string> DialogWhenLackOfGold { get; }
}