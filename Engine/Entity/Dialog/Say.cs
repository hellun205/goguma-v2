namespace GogumaWPF.Engine.Entity.Dialog;

public sealed class Say : IDialog
{
  public string Text { get; }
  
  public Speaker Speaker { get; }
  
  public Say(Speaker speaker, string text)
  {
    Speaker = speaker;
    Text = text;
  }
}