namespace Goguma.Engine.Entity;

public interface IEnemy : IManageable
{
  public EntityStatus Status { get; }
}