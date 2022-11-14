using System;

namespace GogumaWPF.Engine.Entity.Dialog;

[Obsolete("TO DO", true)]
public sealed class ReadInt : IDialog
{
  public string Text { get; }
  
  public Speaker Speaker { get; }
  
  public ReadInt(Speaker speaker, string text)
  {
    Speaker = speaker;
    Text = text;
  }
}