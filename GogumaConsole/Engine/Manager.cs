namespace GogumaConsole.Engine;

public sealed class Manager<T> where T : IManageable
{
  public HashSet<T> Items { get; private set; } = new HashSet<T>();

  public string[] GetCodes => Items.Select(x => x.Code).ToArray();
  
  public const string Empty = "global:empty";
  
  public T Get(string code)
  {
    var skill = Items.FirstOrDefault(x => x.Code == code);
    if (code == Empty)
      throw new Exception($"{code} is empty");
    if (skill == null)
      throw new Exception($"invalid {typeof(T).Name} code: {code}");
    return skill;
  }

  public void Add(T item)
  {
    if (Items.FirstOrDefault(x => x.Code == item.Code) == null)
      Items.Add(item);
    else
      throw new Exception($"{typeof(T).Name} code already exists: {item.Code}");
  }

  public void AddRange(IEnumerable<T> items)
  {
    foreach (var item in items)
      Add(item);
  }
}

public static class Manager
{
  public const string Empty = Manager<IManageable>.Empty;
  
  public static class Types
  {
    public const string Item = "item";

    public const string Skill = "skill";

    public const string Field = "field";

    public const string World = "world";
  }
}