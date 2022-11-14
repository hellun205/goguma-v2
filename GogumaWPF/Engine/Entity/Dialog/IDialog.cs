using System.Diagnostics.CodeAnalysis;

namespace GogumaWPF.Engine.Entity.Dialog;

public interface IDialog
{
  public string Text { get; }
  
  public Speaker Speaker { get; }
  
}