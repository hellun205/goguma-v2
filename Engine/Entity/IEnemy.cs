namespace GogumaWPF.Engine.Entity;

public interface IEnemy : IManageable
{
  public EntityStatus Status { get; }
}