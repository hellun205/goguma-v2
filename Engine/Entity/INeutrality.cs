using System.Collections.Generic;
using Goguma.Engine.Entity.Dialog;
using Goguma.Engine.Map;

namespace Goguma.Engine.Entity;

public interface INeutrality : ICanvasItem, IGameObject // NPC
{
  // public string Name { get; }
  
  public string Descriptions { get; }
  
  public IEnumerable<IDialog> MeetDialogs { get; }
  
}