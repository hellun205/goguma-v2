using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Goguma.Screen;

/// <summary>
/// 스크린 확장 기능
/// </summary>
public static partial class ScreenUtils
{
  public static readonly char BGChr = '*';
  public static readonly char FGChr = '%';
  public static readonly char ResetChr = '!';
  
  /// <summary>
  /// 글자 색 정보를 가진 텍스트를 변환하여 글자를 출력합니다.
  /// </summary>
  /// <param name="screen">글자를 출력할 스크린</param>
  /// <param name="coloredText">글자 색 정보를 가진 텍스트</param>
  public static void PrintF(this Screen screen, string coloredText)
  {
    var ftxts = screen.GetFTexts(coloredText);

    foreach (var ftxt in ftxts)
      screen.Print(ftxt.Y, ftxt.X);
  }

  /// <summary>
  /// 글자 색 정보를 가진 텍스트를 색과 텍스트로 분리한 묶음으로 가져옵니다.
  /// </summary>
  /// <param name="screen">기본 색을 가져올 스크린</param>
  /// <param name="coloredText">글자 색 정보를 가진 텍스트</param>
  /// <param name="convertToSymbol">일반 문자를 심볼 문자로 변환 할지에 대한 여부</param>
  /// <returns></returns>
  public static Pair<Pair<Brush>, string>[] GetFTexts(this Screen screen, string coloredText,
    bool convertToSymbol = false)
  {
    var defaultRes = new[]
      {new Pair<Pair<Brush>, string>(new(screen.FGColor, screen.BGColor), coloredText)};
    var result = new List<Pair<Pair<Brush>, string>>();

    if (coloredText.Contains("&{"))
    {
      string[] split = coloredText.Split("&{");

      for (int i = 1; i < split.Length; i++)
      {
        string text = split[i];
        string[] tagSplit = text.Split('}');
        try
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);

          if (!tagSplit[0].Contains(ResetChr))
          {
            if (tagSplit[0].Contains(FGChr))
            {
              color.X = (Brush) (new BrushConverter().ConvertFromString(tagSplit[0].Split(FGChr)[1]) ?? screen.FGColor);
            }

            if (tagSplit[0].Contains(BGChr))
            {
              color.Y = (Brush) (new BrushConverter().ConvertFromString(tagSplit[0].Split(BGChr)[1]) ?? screen.BGColor);
            }
          }

          result.Add(new Pair<Pair<Brush>, string>(color, (convertToSymbol ? tagSplit[1].ToSymbol() : tagSplit[1])));
        }
        catch
        {
          return defaultRes;
        }
      }

      return result.ToArray();
    }
    else
      return defaultRes;
  }

  /// <summary>
  /// 글자 색 정보를 가진 텍스트의 텍스트 부분을 가져옵니다.
  /// </summary>
  /// <param name="screen">기본 색을 가져올 스크린</param>
  /// <param name="coloredText">글자 색 정보를 가진 텍스트</param>
  /// <returns></returns>
  public static string GetStringOfCText(this Screen screen, string coloredText)
  {
    var pairs = screen.GetFTexts(coloredText);
    var sb = new StringBuilder();

    foreach (var pair in pairs)
      sb.Append(pair.Y);

    return sb.ToString();
  }
  

  public static void Select2d(this Screen screen, Dictionary<string, List<string>> queue,
    string cancelText, Action<Pair<int>?> callBack)
  {
    if (screen.CanTask)
    {
      screen.CanTask = false;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      List<string> rows = queue.Keys.ToList();
      Pair<int> selectingIndexes = new();
      Pair<int> maxIndexes = new(rows.Count - (cancellable ? 0 : 1), 0);

      screen.SaveRTF();

      void While()
      {
        screen.LoadRTF();
        screen.Println();

        for (int i = 0; i < rows.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndexes.X)
          {
            color = new(screen.BGColor, screen.FGColor);
          }

          screen.Print("  ");
          screen.Print($" [ {(cancellable && i == rows.Count ? cancelText : rows[i])} ] ", color);
          screen.Print("  ");
        }

        screen.Print("\n");
        if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
          for (int i = 0; i < queue[rows[selectingIndexes.X]].Count; i++)
          {
            Pair<Brush> color = new(screen.FGColor, screen.BGColor);
            if (i == selectingIndexes.Y)
            {
              color = new(screen.BGColor, screen.FGColor);
            }

            screen.Print("    ");
            screen.Print($" [ {queue[rows[selectingIndexes.X]][i]} ] ", color);
            screen.Println();
          }

        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            screen.CanTask = true;
            callBack((!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X) ? selectingIndexes : null));
          }
          else if (key == screen.KeySet.Left)
          {
            if (selectingIndexes.X == 0)
              selectingIndexes.X = maxIndexes.X;
            else
              selectingIndexes.X -= 1;
            selectingIndexes.Y = 0;
            if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
              maxIndexes.Y = queue[rows[selectingIndexes.X]].Count - 1;
            While();
          }
          else if (key == screen.KeySet.Right)
          {
            if (selectingIndexes.X == maxIndexes.X)
              selectingIndexes.X = 0;
            else
              selectingIndexes.X += 1;
            selectingIndexes.Y = 0;
            if (!cancellable || (cancellable && selectingIndexes.X != maxIndexes.X))
              maxIndexes.Y = queue[rows[selectingIndexes.X]].Count - 1;
            While();
          }
          else if (key == screen.KeySet.Up)
          {
            if (selectingIndexes.Y == 0)
              selectingIndexes.Y = maxIndexes.Y;
            else
              selectingIndexes.Y -= 1;
            While();
          }
          else if (key == screen.KeySet.Down)
          {
            if (selectingIndexes.Y == maxIndexes.Y)
              selectingIndexes.Y = 0;
            else
              selectingIndexes.Y += 1;
            While();
          }
          else
          {
            While();
          }
        });
      }

      While();
    }
    else
      screen.ThrowWhenCantTask();
  }

  public static void Pause(this Screen screen, string text, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(callBack);
  }

  public static void Pause(this Screen screen, Action<Key> callBack) =>
    screen.Pause("계속하려면 아무 키나 누르십시오...", callBack);

  public static void Pause(this Screen screen, string text, Key press, Action<Key> callBack)
  {
    if (!string.IsNullOrEmpty(text))
      screen.Print($"\n{text}\n");
    screen.ReadKey(press, callBack);
  }

  public static void Pause(this Screen screen, Key press, Action<Key> callBack) =>
    screen.Pause($"계속하려면 {press}키를 누르십시오...", press, callBack);

  public static void Println(this Screen screen, uint count = 1)
  {
    if (count == 0) return;
    var sb = new StringBuilder();
    for (var i = 0; i < count; i++)
      sb.Append('\n');

    screen.Print(sb.ToString());
  }

  public static double GetProperHeight(int line = 1) => Screen.SFHeight * 2 + Screen.FHeight * line + 31;
} 