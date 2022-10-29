using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using static goguma_v2.ConsoleUtil;

namespace goguma_v2.Engine.Player;

public partial class Player
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

  private static PlayerData LoadData(string name) => PlayerData.Load($"{SavePath}/{name}.json");

  public static Player? Load()
  {
    Player While()
    {
      Clear();
      Select("캐릭터를 선택하세요.", new Dictionary<string, string>()
      {
        {"새로 만들기", "new"},
        {"세이브 불러오기", "load"},
        {"게임 종료", "cancel"}
      }, () =>
      {
        switch (Selection)
        {
          case "new":
            Print("캐릭터의 이름을 정해주세요\n");
            ReadText(() =>
            {
              // load(new Player(Text));
            });
            break;
          
          case "load":
            DirectoryInfo dInfo = new DirectoryInfo($"{SavePath}");
            FileInfo[] fInfos = dInfo.GetFiles("*.json");
            Dictionary<string, string> datas = new Dictionary<string, string>();
            foreach (var file in fInfos)
            {
              PlayerData pData = LoadData(file.Name);
              datas.Add($"{pData.Name} ( Lv. {pData.Level} / {pData.Class} )", file.Name);
            }
            Select("불러올 캐릭터를 선택하세요.", datas, () =>
            {
               // load(Load(Selection));
            });
            break;
          
          case "cancel":
            Application.Current.Shutdown();
            break;
        }
      });
      return While();
    }
    return While();
  }
}