using System;

namespace Goguma.Engine.Entity.Dialogue;

[Obsolete("not implement")]
public sealed class ReadInt : IDialogue
{
  public string Text { get; }
  
  public Speakers Speakers { get; }

  public ushort Line { get; set; } = 1;

  public ReadInt(Speakers speakers, string text)
  {
    Speakers = speakers;
    Text = text;
  }
}