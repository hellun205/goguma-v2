using System;

namespace Goguma.Engine.Entity.Dialogue;

[Obsolete("not implement")]
public sealed class ReadText : IDialogue
{
  public string Text { get; }
  
  public Speakers Speakers { get; }
  
  public ushort Line { get; set; } = 1;
  
  public ReadText(Speakers speakers, string text)
  {
    Speakers = speakers;
    Text = text;
  }
}