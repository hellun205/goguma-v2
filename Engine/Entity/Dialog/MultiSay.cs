using System.Collections.Generic;

namespace Goguma.Engine.Entity.Dialog;

public sealed class MultiSay : IDialog
{
  public string Text => (PreviousText != null && Texts.ContainsKey(PreviousText) ? Texts[PreviousText] : DefaultText);

  public Speaker Speaker { get; }

  public string DefaultText { get; set; } = string.Empty;

  public Dictionary<string, string>? Texts { get; set; }

  public string? PreviousText { get; set; }

  public MultiSay(Speaker speaker)
  {
    Speaker = speaker;
  }
}