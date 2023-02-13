using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Goguma.Engine;
using Goguma.Engine.Entity;
using Goguma.Engine.Player;
using Goguma.Engine.Entity.Dialog;
using Goguma.Engine.Map;
using Goguma.Screen.Writing;

namespace Goguma.Game;

public static partial class Main
{
  public static MainWindow window = (MainWindow) Application.Current.MainWindow;
  public static Screen.Screen screen;
  public static Player? player = null;
  public static GameObjectManager<IGameObject> GameObjectManager { get; set; } = new GameObjectManager<IGameObject>();

  public static string EmptyCode => GameObjectManager<IGameObject>.Empty;

  public static IGameObject GetManageable(this string code) => GameObjectManager.Get(code);

  public static IGameObject? GetManageableOrDefault(this string code, IGameObject? defaultValue = null)
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
    player = new Player("test");

    // Menu
    Screen.Screen.IgnoreKeyPressEvent = false;
    Screen.Screen.OnKeyPress += (sender, e) =>
    {
      if (e.Key == Key.C && !screen.IsOpenedSubScreen)
      {
        Screen.Screen.IgnoreKeyPressEvent = true;
        screen.OpenSubScreen("메뉴", new Size(220, 300), screen =>
        {
          screen.RTBMain.HorizontalContentAlignment = HorizontalAlignment.Center;
          screen.RTBMain.VerticalContentAlignment = VerticalAlignment.Center;
          
          void While()
          {
            screen.Clear();
            screen.Select(new Dictionary<string, Action>()
            {
              {"Resume", () =>
              {
                screen.ExitSub();
                Screen.Screen.IgnoreKeyPressEvent = false;
              }},
              {"Game Exit", () =>
              {
                screen.OpenSubScreen("question", new Size(200,120), screen2 =>
                {
                  screen2.ScrollToEnd = false;
                  screen2.Print("진짜로 종료하시겠습니까?\n");
                  screen2.Select(new Dictionary<string, Action>()
                  {
                    {"아니요", () =>
                    {
                      screen2.ExitSub();
                      While();
                    }},
                    {"예", () =>
                    {
                      Application.Current.Shutdown();
                    }}
                  });
                }, result =>
                {
                    
                });
              }}
            });
          }
          While();
        }, result =>
        {
          
        });
      }
    };
  }
}