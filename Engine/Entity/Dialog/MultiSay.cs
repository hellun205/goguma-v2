using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Goguma.Engine.Entity.Dialog;

public sealed class MultiSay : IDialog
{
  public string Text => (PreviousText != null && Texts.ContainsKey(PreviousText) ? Texts[PreviousText] : DefaultText);

  public Speaker Speaker { get; }

  [NotNull] public string DefaultText { get; set; }

  [NotNull] public Dictionary<string, string> Texts { get; set; }

  public string? PreviousText { get; set; }

  public MultiSay(Speaker speaker)
  {
    Speaker = speaker;
  }
}