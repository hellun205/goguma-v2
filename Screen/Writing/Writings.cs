using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace GogumaWPF.Screen.Writing;

public static class Writings
{
  public static void ReadWriting(this Screen screen, Writing queue, Action<string> callBack)
  {
    if (screen.CanTask)
    {
      screen.CanTask = false;
      int? temp = null;
      Pair<int> selectingIndex = new(0, 0);
      Pair<int> maxIndex = queue.Count;

      screen.SaveRTF();

      void While()
      {
        screen.LoadRTF();
        screen.Println(2);
        screen.Print($" {queue.Text.FillEmpty(queue.Length, '.').ToSymbol()} ", new(screen.BGColor, screen.FGColor));
        screen.Println(2);

        var list = queue.Btns.ToArray();

        for (int y = 0; y < queue.Count.Y; y++)
        {
          for (int x = 0; x < queue.Count.X; x++)
          {
            int? index = queue.Location[x][y];
            if (index == null)
            {
              temp = null;
              continue;
            }

            if (temp != null && temp == index)
            {
              if (selectingIndex == new Pair<int>(x, y))
                selectingIndex.X -= 1;
              continue;
            }

            Pair<Brush> color = new(screen.FGColor, screen.BGColor);
            if (new Pair<int>(x, y) == selectingIndex)
            {
              color = new(screen.BGColor, screen.FGColor);
            }

            screen.Print($" [ {list[index.Value].Display} ] ", color);

            screen.Print(" ");
            temp = index;
          }

          screen.Println(2);
        }

        // screen.Print($"s: {selectingIndex}, {queue.Location[selectingIndex.X][selectingIndex.Y]}");

        screen.CanTask = true;
        screen.ReadKey(key =>
        {
          var pos = selectingIndex;

          if (key == screen.KeySet.Enter)
          {
            list[queue.Location[selectingIndex.X][selectingIndex.Y].Value].OnClick();

            if (!queue.isWhile)
            {
              screen.CanTask = true;
              callBack(queue.Text);
              return;
            }
          }
          else if (key == screen.KeySet.Up)
            pos.Y -= 1;
          else if (key == screen.KeySet.Down)
            pos.Y += 1;
          else if (key == screen.KeySet.Left)
            pos.X -= 1;
          else if (key == screen.KeySet.Right)
            pos.X += 1;

          if (pos.X >= 0 && pos.X < queue.Count.X &&
              pos.Y >= 0 && pos.Y < queue.Count.Y &&
              queue.Location[pos.X][pos.Y] != null)
            selectingIndex = pos;

          While();
        });
      }

      While();
    }
    else
      screen.ThrowWhenCantTask();
  }

  public static void ReadWritingEng(this Screen screen,int length, Action<string> callBack) =>
    screen.ReadWriting(new English(length), callBack);
}