using System.Collections.Generic;
using System.Windows.Media;
using Goguma.Engine.Entity;
using Goguma.Engine.Entity.Dialogue;
using Goguma.Engine.Item;

namespace Goguma.Game.Entity;

public class TestNPC : Engine.Entity.Entity, ITrader, INeutrality
{
  public TestNPC(string code) : base(code)
  {
  }

  public Pair<byte> Position { get; set; }
  
  public char Icon { get; set; }
  
  public string CanvasDescriptions { get; set; }
  
  public Brush Color { get; set; }
  
  public IEnumerable<IDialogue> MeetDialogs { get; set; }

  public string TraderType { get; set; }
  public IEnumerable<string> TradingItems { get; set; }
  public IEnumerable<string> DialogWhenTrade { get; set; }
  public IEnumerable<string> DialogWhenAfterPurchase { get; set; }
  public IEnumerable<string> DialogWhenAfterSell { get; set; }
  
  public IEnumerable<string> DialogWhenLackOfGold { get; set; }
}