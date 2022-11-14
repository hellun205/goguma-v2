using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GogumaWPF.Engine.Entity.Dialog;

public static class DialogExts
{
  public static int DialogSpeed { get; set; } = 50;

  private static async Task ShowDialogText(this Screen.Screen screen, string textF)
  {
    bool isSkip = false;
    var ftxts = screen.GetFTxts(textF);

    screen.ReadKey(screen.KeySet.Enter, key => { isSkip = true; });

    foreach (var ftxt in ftxts)
    {
      var clr = ftxt.X;
      var strs = ftxt.Y.ToCharArray();

      foreach (var str in strs)
      {
        screen.Print(str.ToString(), clr);
        if (str != ' ' && !isSkip) await Task.Delay(DialogSpeed);
      }
    }

    if (!isSkip) screen.ExitRead();
  }

  private static async void ShowDialog(this Screen.Screen screen, IDialog dialog, Entity entity, Player.Player player,
    bool hasNextDialog, Action<string?> callBack)
  {
    if (entity is INeutrality npc)
    {
      string prefixF = dialog.Speaker switch
      {
        Speaker.ENTITY => $"<fg='{npc.Color}'>{entity.Name}",
        Speaker.PLAYER => $"<fg='{Brushes.DarkGreen}'>{player.Name}",
        _ => $"<fg='{Utils.GetARGB(100, 255, 255, 255)}'>알 수 없음"
      };

      void Fin(bool hasHelpText = true, string? res = null)
      {
        if (hasHelpText)
        {
          screen.Println(2);
          screen.PrintF(
            $"<fg='{Brushes.DimGray}'>[ {screen.KeySet.Enter} ] {(hasNextDialog ? "다음" : "대화 종료")}");
          screen.ReadKey(screen.KeySet.Enter, key => { callBack(res); });
        }
        else
          callBack(res);
      }

      screen.PrintF($"{prefixF}<reset> : ");

      switch (dialog)
      {
        case MultiSay multiSay:
        case Say:
          await screen.ShowDialogText(dialog.Text);
          Fin();
          break;

        case Select select:
          await screen.ShowDialogText(dialog.Text);

          var tempRTF = screen.TempRTF;
          Dictionary<string, string> dict = select.Options.ToDictionary<string, string>(x => x);
          screen.Select(dict, result =>
          {
            screen.TempRTF = tempRTF;
            Fin(false, result);
          });
          break;

        // case ReadInt readInt:
        //   throw new NotImplementedException();
        //   break;
        //
        // case ReadText readStr:
        //   throw new NotImplementedException();
        //   break;
      }
    }
  }

  public static void ShowDialogs(this Screen.Screen screen, IEnumerable<IDialog> dialogs, Entity entity,
    Player.Player player, Action callBack)
  {
    int maxIndex = dialogs.Count();
    int index = 0;
    var list = dialogs.ToArray();
    string? tmp = null;
    
    screen.SaveRTF();

    void While()
    {
      if (index < maxIndex)
      {
        IDialog dialog = list[index];

        if (dialog is MultiSay multiSay)
          multiSay.PreviousText = tmp;

        screen.LoadRTF();
        screen.ShowDialog(dialog, entity, player, index < maxIndex, res =>
        {
          tmp = res;
          index += 1;
          While();
        });
      }
      else
      {
        callBack();
      }
    }

    While();
  }
}