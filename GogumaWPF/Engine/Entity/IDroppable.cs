using System.Collections.Generic;

namespace GogumaWPF.Engine.Entity;

public interface IDroppable : IEnemy
{
  public IEnumerable<DropItem> DropItems { get; }
}