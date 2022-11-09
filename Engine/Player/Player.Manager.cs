using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using static GogumaV2.Main;

namespace GogumaV2.Engine.Player;

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

  public static void Load(Action callBack)
  {
    void load(Player? player)
    {
      Main.player = player;
      callBack();
    } 
    void While() 
    {
      consoleUtil.Clear();
      consoleUtil.Select("캐릭터를 선택하세요.", new Dictionary<string, string>()
      {
        {"새로 만들기", "new"},
        {"세이브 불러오기", "load"},
        {"뒤로 가기", "cancel"}
      }, selection =>
      {
        switch (selection)
        {
          case "new":
            consoleUtil.Print("캐릭터의 이름을 정해주세요\n");
            consoleUtil.ReadText(playerName =>
            {
              load(new Player(playerName));
              Save(Main.player);
            });
            break;
          
          case "load":
            DirectoryInfo dInfo = new DirectoryInfo($"{SavePath}");
            FileInfo[] fInfos = dInfo.GetFiles("*.json");
            Dictionary<string, string> datas = new Dictionary<string, string>();
            foreach (var file in fInfos)
            {
              PlayerData pData = PlayerData.Load(file.FullName);
              datas.Add(pData.ToString(), pData.Name);
            }
            consoleUtil.Select("불러올 캐릭터를 선택하세요.", datas, "취소", playerName =>
            {
               if (!string.IsNullOrEmpty(playerName)) load(Load(playerName));
               else While();
            });
            break;
          
          case "cancel":
            load(null);
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