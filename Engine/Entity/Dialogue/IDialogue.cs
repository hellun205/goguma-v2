
namespace Goguma.Engine.Entity.Dialogue;

public interface IDialogue
{
  public string Text { get; }
  
  public Speakers Speakers { get; }
  
  public ushort Line { get; }
  
}