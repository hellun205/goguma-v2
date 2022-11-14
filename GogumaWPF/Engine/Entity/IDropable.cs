using System.Collections.Generic;

namespace GogumaWPF.Engine.Entity;

public interface IDropable
{
  public IEnumerable<DropItem> DropItems { get; }
}