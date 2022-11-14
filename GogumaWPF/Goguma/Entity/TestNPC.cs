using System.Collections.Generic;
using System.Windows.Media;
using GogumaWPF.Engine.Entity;
using GogumaWPF.Engine.Entity.Dialog;

namespace GogumaWPF.Goguma.Entity;

public class TestNPC : Engine.Entity.Entity, INeutrality
{
  public TestNPC(string code) : base(code)
  {
  }

  public Pair<byte> Position { get; set; }
  
  public char Icon { get; set; }
  
  public string CanvasDescriptions { get; set; }
  
  public Brush Color { get; set; }
  
  public IEnumerable<IDialog> MeetDialogs { get; set; }
  
}