using System.Collections.Generic;
using GogumaWPF.Engine.Entity.Dialog;
using GogumaWPF.Engine.Map;

namespace GogumaWPF.Engine.Entity;

public interface INeutrality : ICanvasItem, IManageable // NPC
{
  public string Name { get; }
  
  public string Descriptions { get; }
  
  public IEnumerable<IDialog> MeetDialogs { get; }
  
}