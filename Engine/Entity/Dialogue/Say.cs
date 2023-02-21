namespace Goguma.Engine.Entity.Dialogue;

public sealed class Say : IDialogue
{
  public string Text { get; }
  
  public Speakers Speakers { get; }
  
  public ushort Line { get; set; } = 1;

  public Say(Speakers speakers, string text)
  {
    Speakers = speakers;
    Text = text;
  }
}