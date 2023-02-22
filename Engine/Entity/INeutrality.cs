using System.Collections.Generic;
using Goguma.Engine.Entity.Dialogue;
using Goguma.Engine.Map;

namespace Goguma.Engine.Entity;

public interface INeutrality : ICanvasItem, IGameObject // NPC
{
  // public string Name { get; }
  
  public string Descriptions { get; }
  
  public IEnumerable<IDialogue> MeetDialogs { get; }
  
}