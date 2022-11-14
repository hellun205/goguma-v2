using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GogumaWPF.Engine.Entity.Dialog;

public sealed class Select : IDialog
{
  public string Text { get; }
  
  public Speaker Speaker { get; }

  [NotNull]
  public IEnumerable<string> Options { get; set; }

  public Select(Speaker speaker, string text)
  {
    Speaker = speaker;
    Text = text;
  }
}