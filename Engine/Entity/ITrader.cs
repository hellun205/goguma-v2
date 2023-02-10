using System.Collections.Generic;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Item;

namespace GogumaWPF.Engine.Entity;

public interface ITrader : INeutrality
{
  public IEnumerable<string> TradingItems { get; }
  
  public IEnumerable<string> DialogWhenTrade { get; }
  
  public IEnumerable<string> DialogWhenAfterPurchase { get; }
  
  public IEnumerable<string> DialogWhenAfterSell { get; }
  
  public IEnumerable<string> DialogWhenLackOfGold { get; }
}