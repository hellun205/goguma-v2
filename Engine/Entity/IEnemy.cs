namespace Goguma.Engine.Entity;

public interface IEnemy : IGameObject
{
  public EntityStatus Status { get; }
}