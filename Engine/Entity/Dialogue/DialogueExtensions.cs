using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Goguma.Game;
using Goguma.Screen;

namespace Goguma.Engine.Entity.Dialogue;

public static class DialogueExtensions
{
  public static readonly Brush PLAYER_COLOR = Brushes.DarkGreen;
  public static readonly Brush UNKNOWN_COLOR = Brushes.Goldenrod;
  public static int DialogSpeed { get; set; } = 100;

  private static async Task ShowDialogText(this IDialogue dialogue, Screen.Screen screen, Action? callBack = null)
  {
    bool isSkip = false;
    var ftxts = screen.GetFTexts(dialogue.Text);

    screen.ReadKey(screen.KeySet.Enter, key => { isSkip = true; });

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

    if (!isSkip) screen.ExitRead();
    callBack?.Invoke();
  }

  private static void StartDialogue(this IEnumerable<IDialogue> dialogueItems, Player.Player player,
    INeutrality opponent, Screen.Screen dialogueWindow, Action<string> callBack)
  {
    IDialogue[] dialogues = dialogueItems.ToArray();
    int currentIndex = 0;

    void While()
    {
      dialogueWindow.SetProperSize(300, 3 + dialogues[currentIndex].Line);
      string speakerPrefix = dialogues[currentIndex].Speakers switch
      {
        Speakers.ENTITY => $"&{{%{opponent.Color}%}}{opponent.Name}",
        Speakers.PLAYER => $"&{{%{PLAYER_COLOR}%}}{player.Name} (you)",
        Speakers.UNKNOWN => $"&{{%{UNKNOWN_COLOR}%}}???",
        _ => $"not implement"
      };
      dialogueWindow.ScrollToEnd = false;
      dialogueWindow.TextAlignment = TextAlignment.Center;

      dialogueWindow.Clear();
      dialogueWindow.PrintF(speakerPrefix);
      dialogueWindow.Println(2);

      ShowDialogText(dialogues[currentIndex], dialogueWindow, () =>
      {
        dialogueWindow.Println();

        switch (dialogues[currentIndex])
        {
          case Say:
            if (currentIndex == 0)
            {
              dialogueWindow.SelectH(new Dictionary<string, Action>()
              {
                {"대화 종료", () => callBack.Invoke("exit")},
                {
                  "다음", () =>
                  {
                    currentIndex++;
                    While();
                  }
                }
              }, 0, 1);
            }
            else if (currentIndex == dialogues.Length - 1)
            {
              dialogueWindow.SelectH(new Dictionary<string, Action>()
              {
                {"대화 종료", () => callBack.Invoke("exit")},
                {
                  "이전", () =>
                  {
                    currentIndex--;
                    While();
                  }
                },
                {
                  "확인", () => callBack.Invoke("end")
                }
              }, 0, 2);
            }
            else
            {
              dialogueWindow.SelectH(new Dictionary<string, Action>()
              {
                {"대화 종료", () => callBack.Invoke("exit")},
                {
                  "이전", () =>
                  {
                    currentIndex--;
                    While();
                  }
                },
                {
                  "다음", () =>
                  {
                    currentIndex++;
                    While();
                  }
                }
              }, 0, 2);
            }

            break;

          case Select selectDialogue:
            dialogueWindow.SetProperSize(300, 3 + dialogues[currentIndex].Line + selectDialogue.Options.Count());
            dialogueWindow.SelectV(selectDialogue.Options.ToDictionary(pair => pair.Key, pair => (object) pair.Key),
              selection =>
              {
                selectDialogue.Options[selection.ToString()]
                  .StartDialogue(player, opponent, dialogueWindow, result =>
                  {
                    switch (result)
                    {
                      case "end":
                      case "exit":
                        if (currentIndex == dialogues.Length - 1)
                        {
                          dialogueWindow.ExitSub("end-dialogue");
                        }
                        else
                        {
                          currentIndex++;
                          While();
                        }

                        break;
                    }
                  });
              });
            break;

          case ReadInt:
          case ReadText:
            throw new NotImplementedException();
            break;
        }
      });
    }

    While();
  }

  public static void ShowDialogue(this IEnumerable<IDialogue> dialogueItems, INeutrality opponent, Action callBack)
  {
    Screen.Screen? screen = Main.Screen;
    Player.Player? player = Main.Player;
    if (screen == null) throw new Exception("메인 스크린이 존재하지 않습니다.");
    if (player == null) throw new Exception("플레이어가 존재하지 않습니다.");

    screen.OpenSubScreen("dialogue", new Size(300, ScreenUtils.GetProperHeight(4)),
      dialogueWindow =>
      {
        dialogueItems.StartDialogue(player, opponent, dialogueWindow, result => dialogueWindow.ExitSub(result));
      },
      result =>
      {
        switch (result)
        {
          case "end":
          case "exit":
            callBack.Invoke();
            break;
        }
      });
  }
}