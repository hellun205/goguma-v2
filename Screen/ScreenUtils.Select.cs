using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Goguma.Game;

namespace Goguma.Screen;

public static partial class ScreenUtils
{
  /// <summary>
  /// 선택 시작
  /// </summary>
  /// <param name="screen">스크린</param>
  /// <param name="queue">선택지 (key: 표기 / value: 값)</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="upKey">-index key</param>
  /// <param name="downKey">+index key</param>
  /// <param name="optionsNewline">선택지 간 줄 바꿈 여부</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void Select(this Screen screen, Dictionary<string, string> queue, string cancelText, bool hasCancelKey,
    Action<string> callBack, Key upKey, Key downKey, bool optionsNewline, int startIndex = 0)
  {
    if (screen.CanTask)
    {
      Main.Logger.Log($"start select({(optionsNewline ? "V" : "H")})");
      screen.CanTask = false;
      bool cancellable = !string.IsNullOrEmpty(cancelText);
      int selectingIndex = startIndex;
      int maxIndex = queue.Count - (cancellable ? 0 : 1);
      string[] options = queue.Keys.ToArray();

      screen.SaveRTF();

      void While()
      {
        screen.LoadRTF();
        screen.Println();

        for (int i = 0; i < queue.Count + (cancellable ? 1 : 0); i++)
        {
          Pair<Brush> color = new(screen.FGColor, screen.BGColor);
          if (i == selectingIndex)
          {
            color = new(screen.BGColor, screen.FGColor);
          }

          screen.Print($" [ {(cancellable && i == maxIndex ? cancelText : options[i])} ] ", color);
          if (optionsNewline) screen.Println();
          else screen.Print(" ");
        }

        screen.CanTask = true;
        Main.Logger.Log($"isReadingKey: {screen.IsReadingKey}");
        screen.ReadKey(key =>
        {
          if (key == screen.KeySet.Enter)
          {
            Main.Logger.Log($"successful get result from select: {selectingIndex}");
            screen.CanTask = true;
            callBack((((cancellable && selectingIndex == maxIndex) ? null : queue[options[selectingIndex]]) ??
                      string.Empty));
          }
          else if (key == upKey)
          {
            if (selectingIndex == 0)
              selectingIndex = maxIndex;
            else
              selectingIndex -= 1;
            Main.Logger.Log($"select: {selectingIndex}");
            While();
          }
          else if (key == downKey)
          {
            if (selectingIndex == maxIndex)
              selectingIndex = 0;
            else
              selectingIndex += 1;
            Main.Logger.Log($"select: {selectingIndex}");
            While();
          }
          else if (hasCancelKey && key == screen.KeySet.Exit)
          {
            Main.Logger.Log($"select: cancel key");
            callBack(string.Empty);
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

  /// <summary>
  /// 세로 선택 시작 (선택 후 해당 작업 실행)
  /// </summary>
  /// <param name="screen">스크린</param>
  /// <param name="queue">선택지 (key: 표기 / value: 작업)(</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="cancelCallBack">취소 시 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, Action> queue,
    string cancelText, bool hasCancelKey, Action? cancelCallBack, int startIndex = 0)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    SelectV(screen, dict, cancelText, hasCancelKey, value =>
    {
      if ((cancellable && string.IsNullOrEmpty(value)) || (hasCancelKey && string.IsNullOrEmpty(value)))
        cancelCallBack?.Invoke();
      else
        queue[value]();
    }, startIndex);
  }

  /// <summary>
  /// 가로 선택 시작 (선택 후 해당 작업 실행)
  /// </summary>
  /// <param name="screen">스크린</param>
  /// <param name="queue">선택지 (key: 표기 / value: 작업)</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="cancelCallBack">취소 시 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, Action> queue,
    string cancelText, bool hasCancelKey, Action? cancelCallBack, int startIndex = 0)
  {
    bool cancellable = !string.IsNullOrEmpty(cancelText);
    Dictionary<string, string> dict = queue.ToDictionary(x => x.Key, x => x.Key);

    SelectH(screen, dict, cancelText, hasCancelKey, value =>
    {
      if (cancellable && string.IsNullOrEmpty(value))
        cancelCallBack?.Invoke();
      else
        queue[value]();
    }, startIndex);
  }

  /// <summary>
  /// 세로 선택 시작
  /// </summary>
  /// <param name="screen">스크린</param>
  /// <param name="queue">선택지 (key: 표기 / value: 값)(</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, string> queue, string cancelText,
    bool hasCancelKey, Action<string> callBack, int startIndex = 0) =>
    Select(screen, queue, cancelText, hasCancelKey, callBack, screen.KeySet.Up, screen.KeySet.Down, true, startIndex);

  /// <summary>
  /// 가로 선택 시작
  /// </summary>
  /// <param name="screen">스크린</param>
  /// <param name="queue">선택지 (key: 표기 / value: 값)(</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, string> queue, string cancelText,
    bool hasCancelKey, Action<string> callBack, int startIndex = 0) =>
    Select(screen, queue, cancelText, hasCancelKey, callBack, screen.KeySet.Left, screen.KeySet.Right, false,
      startIndex);

  /// <summary>
  /// 해당 스크린에서 세로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, Action> queue, int startIndex = 0) =>
    SelectV(screen, queue, string.Empty, false, null, startIndex);


  /// <summary>
  /// 해당 스크린에서 취소 선택지를 포함 한 세로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="callWhenCancel"/>)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="callWhenCancel">취소 시 작업</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, Action> queue
    , string cancelText, Action callWhenCancel, bool hasCancelKey = true, int startIndex = 0) =>
    SelectV(screen, queue, cancelText, hasCancelKey, callWhenCancel, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 세로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="queueIndexWhenCancelKey"/>에 따른 작업 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="queueIndexWhenCancelKey">취소 키를 입력 받을 경우 행할 작업 인덱스</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, Action> queue, int queueIndexWhenCancelKey,
    int startIndex = 0) =>
    SelectV(screen, queue, string.Empty, true, queue.Values.ToList()[queueIndexWhenCancelKey], startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 세로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="callWhenCancelKey"/> 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="callWhenCancelKey">취소 시 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, Action> queue, Action callWhenCancelKey,
    int startIndex = 0) =>
    SelectV(screen, queue, string.Empty, true, callWhenCancelKey, startIndex);


  /// <summary>
  /// 해당 스크린에서 세로 선택 작업을 시작합니다.
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 선택지에 따른 반환 값</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, string> queue, Action<string> callBack,
    int startIndex = 0) =>
    SelectV(screen, queue, string.Empty, false, callBack, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 세로 선택 작업을 시작합니다. 
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행, 취소 시 <paramref name="callWhenCancel"/> 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 반환 값</param>
  /// <param name="callBack">선택을 완료 했을 때 행할 작업</param>
  /// <param name="callWhenCancel">취소 키를 입력 받았을 때 행할 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, string> queue, Action<string> callBack,
    Action callWhenCancel, int startIndex = 0) =>
    SelectV(screen, queue, string.Empty, true, result =>
    {
      if (string.IsNullOrEmpty(result))
        callWhenCancel();
      else
        callBack(result);
    }, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 선택지를 포함 한 세로 선택 작업을 시작합니다. 
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행, 취소 시 <see cref="string.Empty"/> 반환)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 반환 값</param>
  /// <param name="cancelText">취소 선택지의 표기</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="callWhenCancel">취소 시 작업</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectV(this Screen screen, Dictionary<string, string> queue, string cancelText,
    Action<string> callBack, Action callWhenCancel, bool hasCancelKey = true, int startIndex = 0) =>
    SelectV(screen, queue, cancelText, hasCancelKey, result =>
    {
      if (string.IsNullOrEmpty(result))
        callWhenCancel();
      else
        callBack(result);
    }, startIndex);


  /// <summary>
  /// 해당 스크린에서 가로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, Action> queue, int startIndex = 0) =>
    SelectH(screen, queue, string.Empty, false, null, startIndex);


  /// <summary>
  /// 해당 스크린에서 취소 선택지를 포함 한 가로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="callWhenCancel"/> 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="callWhenCancel">취소 시 작업</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, Action> queue
    , string cancelText, Action callWhenCancel, bool hasCancelKey = true, int startIndex = 0) =>
    SelectH(screen, queue, cancelText, hasCancelKey, callWhenCancel, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 가로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="queueIndexWhenCancelKey"/>에 따른 작업 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="queueIndexWhenCancelKey">취소 키를 입력 받을 경우 행할 작업 인덱스</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, Action> queue, int queueIndexWhenCancelKey,
    int startIndex = 0) =>
    SelectH(screen, queue, string.Empty, true, queue.Values.ToList()[queueIndexWhenCancelKey], startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 가로 선택 작업을 시작합니다.
  /// (선택 후 해당 작업 실행, 취소 시 <paramref name="callWhenCancelKey"/>에 따른 작업 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 작업</param>
  /// <param name="callWhenCancelKey">취소 키 입력 시 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, Action> queue, Action callWhenCancelKey,
    int startIndex = 0) =>
    SelectH(screen, queue, string.Empty, true, callWhenCancelKey, startIndex);


  /// <summary>
  /// 해당 스크린에서 가로 선택 작업을 시작합니다.
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 반환 값</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, string> queue, Action<string> callBack,
    int startIndex = 0) =>
    SelectH(screen, queue, string.Empty, false, callBack, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 키를 포함 한 가로 선택 작업을 시작합니다.
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행, 취소 시 <see cref="string.Empty"/> 반환)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 반환 값</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="callWhenCancel">취소 키 입력 시 작업</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, string> queue, Action<string> callBack,
    Action callWhenCancel, int startIndex = 0) =>
    SelectH(screen, queue, string.Empty, true, result =>
    {
      if (string.IsNullOrEmpty(result))
        callWhenCancel();
      else
        callBack(result);
    }, startIndex);

  /// <summary>
  /// 해당 스크린에서 취소 선택지를 포함 한 가로 선택 작업을 시작합니다.
  /// (선택 후 <paramref name="callBack"/>에 값을 반환하며 실행, 취소 시 <paramref name="callWhenCancel"/> 실행)
  /// </summary>
  /// <param name="screen">작업을 시작할 스크린</param>
  /// <param name="queue">선택지, Key: 표기 / Value: 반환 값</param>
  /// <param name="cancelText">취소 선택지 표기</param>
  /// <param name="callBack">선택 완료 후 작업</param>
  /// <param name="callWhenCancel">선택 취소 시 작업</param>
  /// <param name="hasCancelKey">취소 키 여부</param>
  /// <param name="startIndex">시작 선택지 인덱스</param>
  public static void SelectH(this Screen screen, Dictionary<string, string> queue, string cancelText,
    Action<string> callBack, Action callWhenCancel, bool hasCancelKey = true, int startIndex = 0) =>
    SelectH(screen, queue, cancelText, hasCancelKey, result =>
    {
      if (string.IsNullOrEmpty(result))
        callWhenCancel();
      else
        callBack(result);
    }, startIndex);
}