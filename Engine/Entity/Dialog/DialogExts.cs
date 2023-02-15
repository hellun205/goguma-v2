using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Goguma.Game;
using Goguma.Screen;

namespace Goguma.Engine.Entity.Dialog;

public static class DialogExts
{
  public static int DialogSpeed { get; set; } = 300;

  private static async Task ShowDialogText(this IDialog dialog, Screen.Screen screen)
  {
    bool isSkip = false;
    var ftxts = screen.GetFTexts(dialog.Text);
    
    // screen.ReadKey(screen.KeySet.Enter, key => { isSkip = true; });

    foreach (var ftxt in ftxts)
    {
      var clr = ftxt.X;
      var strs = ftxt.Y.ToCharArray();

      foreach (var str in strs)
      {
        screen.Print(str.ToString(), clr);
        if (str != ' ' || !isSkip) await Task.Delay(DialogSpeed);
      }
    }

    // if (!isSkip) screen.ExitRead();
  }

  public static async void ShowDialogs(this IEnumerable<IDialog> dialog, INeutrality opponent, Action callBack)
  {
    Screen.Screen? screen = Main.Screen;
    Player.Player? player = Main.Player;
    if (screen == null) throw new Exception("메인 스크린이 존재하지 않습니다.");
    if (player == null) throw new Exception("플레이어가 존재하지 않습니다.");

    IDialog[] dialogs = dialog.ToArray();
    int currentIndex = 0;

    void While()
    {
      string speakerPrefix = dialogs[currentIndex].Speaker switch
      {
        Speaker.ENTITY => $"&{{%{opponent.Color}%}}{opponent.Name}",
        Speaker.PLAYER => $"&{{%{Brushes.DarkGreen}%}}{player.Name}",
        Speaker.UNKNOWN => $"&{{%{Brushes.Goldenrod}%}}???",
        _ => $"not implement"
      };

      screen.OpenSubScreen("dialog", new Size(300, 200), async dialogWindow =>
      {
        dialogWindow.ScrollToEnd = false;
        dialogWindow.TextAlignment = TextAlignment.Center;

        dialogWindow.PrintF(speakerPrefix);
        dialogWindow.Println(2);

        await ShowDialogText(dialogs[currentIndex], dialogWindow);
        dialogWindow.Println();
        
        if (currentIndex == 0)
        {
          dialogWindow.SelectH(new Dictionary<string, Action>()
          {
            {"대화 종료", () => dialogWindow.ExitSub("exit")},
            {"다음", () => dialogWindow.ExitSub("next")}
          }, 0, 1);
        }
        else
        {
          dialogWindow.SelectH(new Dictionary<string, Action>()
          {
            {"대화 종료", () => dialogWindow.ExitSub("exit")},
            {"이전", () => dialogWindow.ExitSub("previous")},
            {"다음", () => dialogWindow.ExitSub("next")}
          }, 0, 2);
        }
      }, result =>
      {
        switch (result)
        {
          case "next":
            currentIndex++;
            While();
            break;
          case "previous":
            currentIndex--;
            While();
            break;
          case "exit":
            callBack.Invoke();
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(result));
        }
      });
    }

    While();
  }
}