using System;
using System.Collections.Generic;

namespace Goguma.Engine.Entity.Dialogue;

public sealed class Select : IDialogue
{
  public string Text { get; }
  
  public Speakers Speakers { get; }

  public Dictionary<string, IEnumerable<IDialogue>?> Options { get; set; }

  public ushort Line { get; set; } = 1;

  public Select(Speakers speakers, string text, Dictionary<string, IEnumerable<IDialogue>?> options)
  {
    Speakers = speakers;
    Text = text;
    Options = options;
  }
}