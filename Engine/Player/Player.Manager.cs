using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Goguma.Game;
using Goguma.Screen;

namespace Goguma.Engine.Player;

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

    Player obj = (Player) deserializer.Deserialize(rs);
    rs.Close();
    return obj;
  }

  [Obsolete]
  private static PlayerData LoadData(string name) => PlayerData.Load($"{SavePath}/{name}.json");

  public static void Load(Action<Player?> callBack)
  {
    void While()
    {
      Main.Screen.Clear();
      Main.Screen.Print("캐릭터를 선택하세요");
      Main.Screen.SelectV(new Dictionary<string, object>()
      {
        {"새로 만들기", "new"},
        {"세이브 불러오기", "load"},
        {"뒤로 가기", "cancel"}
      }, selection =>
      {
        switch (selection)
        {
          case "new":
            Main.Screen.Clear();
            Main.Screen.Print("캐릭터의 이름을 정해주세요\n");
            Main.Screen.ReadText(playerName =>
            {
              callBack(new Player(playerName));
              ;
              Save(Main.Player);
            });
            break;

          case "load":
            DirectoryInfo dInfo = new DirectoryInfo($"{SavePath}");
            FileInfo[] fInfos = dInfo.GetFiles("*.json");
            Dictionary<string, object> datas = fInfos.Select(file => PlayerData.Load(file.FullName))
              .ToDictionary(pData => pData.ToString(), pData => (object)pData.Name);

            Main.Screen.Clear();
            Main.Screen.Print("불러올 캐릭터를 선택하세요.");
            Main.Screen.SelectV(datas, "취소", playerName => callBack(Load(playerName.ToString())), () => While());
            break;

          case "cancel":
            callBack(null);
            break;
          default:
            While();
            break;
        }
      });
    }

    While();
  }
}