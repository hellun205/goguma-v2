using GogumaV2.Engine;
using GogumaV2.Engine.Map.Field;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static Field GetField(this string code)
  {
    return FieldManager.Get(code);
  }

  public static Manager<Field> FieldManager = new Manager<Field>();
  
  private static void InitFieldManager()
  {
    // FieldManager.AddRange(new []
    // {
    //
    // });
  }
}