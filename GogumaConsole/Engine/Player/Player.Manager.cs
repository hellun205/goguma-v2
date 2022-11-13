using System.Runtime.Serialization.Formatters.Binary;
using static GogumaConsole.Console.ConsoleUtil;

namespace GogumaConsole.Engine.Player;

public sealed partial class Player
{
  private static string SavePath => "./Save";
  
  private static void Save(Player player)
  {
    if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
      
    Stream ws = new FileStream($"{SavePath}/{player.Name}.pdt", FileMode.Create);
    BinaryFormatter serializer = new BinaryFormatter();
      
    serializer.Serialize(ws, player);
    ws.Close();
      
    PlayerData.Save($"{SavePath}/{player.Name}.json", player.GetData());
  }

  private static Player Load(string name)
  {
    Stream rs = new FileStream($"{SavePath}/{name}.pdt", FileMode.Open);
    BinaryFormatter deserializer = new BinaryFormatter();

    Player obj = (Player)deserializer.Deserialize(rs);
    rs.Close();
    return obj;
  }
  
  [Obsolete]
  private static PlayerData LoadData(string name) => PlayerData.Load($"{SavePath}/{name}.json");

  public static Player? Load()
  {
    while (true)
    {
      Clear();
      var selection = Select("캐릭터를 선택하세요.", new Dictionary<string, string>()
      {
        {"새로 만들기", "new"},
        {"세이브 불러오기", "load"},
        {"뒤로 가기", "cancel"}
      });

      string playerName;
      Player player;
      switch (selection)
      {
        case "new":
          Print("캐릭터의 이름을 정해주세요\n");
          playerName = ReadText();
          player = new Player(playerName);
          Save(player);
          return player;
          
        case "load":
          DirectoryInfo dInfo = new DirectoryInfo($"{SavePath}");
          FileInfo[] fInfos = dInfo.GetFiles("*.json");
          Dictionary<string, string> datas = fInfos.Select(file => PlayerData.Load(file.FullName)).ToDictionary(pData => pData.ToString(), pData => pData.Name);
          
          playerName = Select("불러올 캐릭터를 선택하세요.", datas, "취소");
          if (!string.IsNullOrEmpty(playerName)) return Load(playerName);
          break;
          
        case "cancel":
          return null;
      }
    }
  }
}