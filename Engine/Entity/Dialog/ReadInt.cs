using System;

namespace Goguma.Engine.Entity.Dialog;

[Obsolete("not implement", true)]
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