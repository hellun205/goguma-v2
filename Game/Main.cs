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
      if (e.Key == Key.C) OpenMenu();
    };

  }

  private static void OpenMenu()
  {
    if (CanOpenMenu)
    {
      var screen = Goguma.Screen.Screen.MainScreen;
      Goguma.Screen.Screen.IgnoreKeyPressEvent = true;
      screen.OpenSubScreen<string>("메뉴", new Size(150, 0), menu =>
      {
        menu.SetProperSize(150, 1);
        menu.AutoSetTextAlign = true;
        menu.TextAlignment = TextAlignment.Center;
        menu.ScrollToEnd = false;
        int index = 0;
        
        void While()
        {
          menu.Clear();
          menu.SelectV(new Dictionary<string, Action<int>>()
          {
            {
              "Resume", (i) =>
              {
                index = i;
                menu.ExitSub("resume");
                Goguma.Screen.Screen.IgnoreKeyPressEvent = false;
              }
            },
            {
              "Game Exit", (i) =>
              {
                index = i;
                menu.Ask("진짜로 종료하시겠습니까?", result =>
                {
                  if (result == null || !result.Value)
                  {
                    While();
                  } 
                  else if (result.Value) menu.ExitSub("game-exit");
                    
                });
              }
            }
          }, startIndex: index);
        }

        While();
      }, result =>
      {
        switch (result)
        {
          case "resume":
            break;
          case "game-exit":
            Application.Current.Shutdown();
            break;
        }
      });
    }
  }
}