using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Goguma.Engine;
using Goguma.Engine.Entity;
using Goguma.Engine.Entity.Dialogue;
using Goguma.Engine.Player;
using Goguma.Screen;

namespace Goguma.Game;

public static partial class Main
{
  // public static MainWindow Window = (MainWindow) Application.Current.MainWindow;
  public static Screen.Screen? Screen;
  public static Player? Player;
  public static Logger.Logger Logger = new Logger.Logger();
  public static bool CanOpenMenu { get; set; } = true;
  public static GameObjectManager<IGameObject> GameObjectManager { get; set; } = new GameObjectManager<IGameObject>();

  public static string EmptyCode => GameObjectManager<IGameObject>.Empty;

  public static IGameObject GetGameObject(this string code) => GameObjectManager.Get(code);

  public static IGameObject? GetGameObjectOrDefault(this string code, IGameObject? defaultValue = null)
  {
    try
    {
      return GameObjectManager.Get(code);
    }
    catch
    {
      return defaultValue;
    }
  }

  static Main()
  {
    InitItemManager();
    InitSkillManager();
    InitFieldManager();
    InitWorldManager();
    InitEntityManager();
  }

  public static void Start()
  {
    // Player.Load(() =>
    // {
    //   
    // });
    Player = new Player("test");

    // Menu
    Goguma.Screen.Screen.IgnoreKeyPressEvent = false;
    Goguma.Screen.Screen.OnKeyPress += (sender, e) =>
    {
      if (e.Key == Key.C && CanOpenMenu)
      {
        Goguma.Screen.Screen.IgnoreKeyPressEvent = true;
        sender.OpenSubScreen("메뉴", new Size(150, 100), menu =>
        {
          menu.AutoSetTextAlign = true;
          menu.TextAlignment = TextAlignment.Center;

          void While()
          {
            menu.Clear();
            menu.SelectV(new Dictionary<string, Action>()
            {
              {
                "Resume", () =>
                {
                  menu.ExitSub();
                  Goguma.Screen.Screen.IgnoreKeyPressEvent = false;
                }
              },
              {
                "Game Exit", () =>
                {
                  menu.OpenSubScreen("question", new Size(200, 100), question =>
                  {
                    question.ScrollToEnd = false;
                    question.TextAlignment = TextAlignment.Center;

                    question.Print("진짜로 종료하시겠습니까?\n");
                    question.SelectH(new Dictionary<string, Action>()
                    {
                      {
                        "아니요", () =>
                        {
                          question.ExitSub();
                          While();
                        }
                      },
                      {"예", () => menu.ExitSub("game-exit")}
                    });
                  });
                }
              }
            });
          }

          While();
        }, result =>
        {
          switch (result)
          {
            case "game-exit":
              Application.Current.Shutdown();
              break;
          }
        });
      }
    };

    INeutrality npc = (INeutrality)"entity:test".GetGameObject();
    
    Screen.ReadKey(Key.Enter, key =>
    {
      npc.MeetDialogs.ShowDialogue(npc, () =>
      {
      
      });
    });
  }
}