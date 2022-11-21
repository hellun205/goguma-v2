using System.Collections.Generic;
using System.Windows.Media;
using GogumaWPF.Engine.Entity;
using GogumaWPF.Engine.Entity.Dialog;

namespace GogumaWPF.Goguma.Entity;

public class TestNPC2 : Engine.Entity.Entity, IEnemy, IDroppable
{
  public TestNPC2(string code) : base(code)
  {
    
  }
  
  public EntityStatus Status { get; }
  
  public IEnumerable<DropItem> DropItems { get; }
}