using System.Collections.Generic;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Map;

namespace GogumaWPF.Engine.Entity;

public interface INeutrality : ICanvasItem // NPC
{
  public IEnumerable<IDialog> MeetDialogs { get; }
  
}