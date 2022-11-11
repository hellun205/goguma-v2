using GogumaV2.Engine;
using GogumaV2.Engine.Map.Field;
using GogumaV2.Goguma.Field;

namespace GogumaV2.Goguma;

public static partial class Main
{
  public static Engine.Map.Field.Field GetField(this string code)
  {
    return FieldManager.Get(code);
  }

  public static Manager<Engine.Map.Field.Field> FieldManager = new Manager<Engine.Map.Field.Field>();

  private static void InitFieldManager()
  {
    FieldManager.AddRange(new[]
    {
      new TestField("test_world", "test_field", '☆')
      {
        Name = "테스트 필드",
        CanvasTitle = "TEST FIELD",
        Descriptions = "테스트용",
        Position = new Pair<byte>(4, 4),
        CanvasSize = new Pair<byte>(10, 10)
      }
    });
  }
}