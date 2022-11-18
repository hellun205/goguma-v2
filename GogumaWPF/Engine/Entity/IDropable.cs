using System.Collections.Generic;

namespace GogumaWPF.Engine.Entity;

public interface IDropable : IEnemy
{
  public IEnumerable<DropItem> DropItems { get; }
}