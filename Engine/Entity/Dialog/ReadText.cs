using System;

namespace Goguma.Engine.Entity.Dialog;

[Obsolete("TO DO", true)]
public sealed class ReadText : IDialog
{
  public string Text { get; }
  
  public Speaker Speaker { get; }
  
  public ReadText(Speaker speaker, string text)
  {
    Speaker = speaker;
    Text = text;
  }
}