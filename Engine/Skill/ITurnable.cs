namespace GogumaV2.Engine.Skill;

public interface ITurnable
{
  public delegate void _OnTurn(object sender);
  
  public byte TurnCount { get; set; }

  public event _OnTurn OnTurn;

  public void Turn();
}