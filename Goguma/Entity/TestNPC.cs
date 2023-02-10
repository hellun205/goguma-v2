using System.Collections.Generic;
using System.Windows.Media;
using GogumaWPF.Engine.Entity;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Item;

namespace GogumaWPF.Goguma.Entity;

public class TestNPC : Engine.Entity.Entity, ITrader, INeutrality
{
  public TestNPC(string code) : base(code)
  {
  }

  public Pair<byte> Position { get; set; }
  
  public char Icon { get; set; }
  
  public string CanvasDescriptions { get; set; }
  
  public Brush Color { get; set; }
  
  public IEnumerable<IDialog> MeetDialogs { get; set; }

  public IEnumerable<string> TradingItems { get; set; }
  public IEnumerable<string> DialogWhenTrade { get; set; }
  public IEnumerable<string> DialogWhenAfterPurchase { get; set; }
  public IEnumerable<string> DialogWhenAfterSell { get; set; }
  
  public IEnumerable<string> DialogWhenLackOfGold { get; set; }
}