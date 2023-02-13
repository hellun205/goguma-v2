using System.Collections.Generic;
using System.Windows.Media;
using Goguma.Engine.Entity;
using Goguma.Engine.Entity.Dialog;

namespace Goguma.Game.Entity;

public class TestNPC2 : Engine.Entity.Entity, IEnemy, IDroppable
{
  public TestNPC2(string code) : base(code)
  {
    
  }
  
  public EntityStatus Status { get; }
  
  public IEnumerable<DropItem> DropItems { get; }
}