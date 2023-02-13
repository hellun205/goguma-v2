using System.Collections.Generic;

namespace Goguma.Engine.Entity;

public interface IDroppable : IEnemy
{
  public IEnumerable<DropItem> DropItems { get; }
}