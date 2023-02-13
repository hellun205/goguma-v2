using Goguma;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Goguma.Screen;

/// <summary>
/// Interaction logic for Screen.xaml
/// </summary>
public partial class Screen : UserControl
{
  public object Font => FindResource("Galmuri11");

  public Brush BGColor
  {
    get => RTBMain.Background;
    set => RTBMain.Background = value;
  }

  public Brush FGColor
  {
    get => RTBMain.Foreground;
    set => RTBMain.Foreground = value;
  }

  public Brush BGColorWhenReadText { get; set; } = Brushes.DimGray;

  public KeySet<Key> KeySet { get; set; } = new KeySet<Key>(Key.Up, Key.Down, Key.Left, Key.Right, Key.Z, Key.X);

  public bool IsReadingText = false;
  public bool IsReadingKey = false;
  public Action<string> CallAfterReadingText;
  public Action<Key> CallAfterReadingKey;
  public string TempRTF;
  public Key? KeyToPress;
  public bool CanTask = true;
  public SubScreen SubScreen;
  public bool IsSubScreen = false;
  public bool IsOpenedSubScreen = false;
  public Action<object> CallBackAfterSubScreen;
  public Action? CallBackAfterSubScreenForceExit;
  public Screen Parent;

  private bool keyDownAvailability = true;
  private Key tempKey;

  public Screen()
  {
    InitializeComponent();
  }

  private void RTBMain_GotFocus(object sender, RoutedEventArgs e)
  {
    if (!IsOpenedSubScreen) TBInput.Focus();
  }

  /// <summary>
  /// 스크린에 글자를 출력합니다.
  /// </summary>
  /// <param name="text">출력할 글자</param>
  public void Print(string text)
  {
    Print(text, new Pair<Brush>(FGColor, BGColor));
  }

  /// <summary>
  /// 스크린에 글자를 출력합니다.
  /// </summary>
  /// <param name="text">출력할 글자</param>
  /// <param name="color">글자 색</param>
  public void Print(string text, Pair<Brush> color)
  {
    TextRange tr = new TextRange(RTBMain.Document.ContentEnd, RTBMain.Document.ContentEnd);
    tr.Text = text;
    tr.ApplyPropertyValue(TextElement.FontFamilyProperty, Font);
    tr.ApplyPropertyValue(TextElement.ForegroundProperty, color.X);
    tr.ApplyPropertyValue(TextElement.BackgroundProperty, color.Y);
  }

  private void RTBMain_TextChanged(object sender, TextChangedEventArgs e)
  {
    // RTBMain.ScrollToEnd();
  }

  /// <summary>
  /// 현재 스크린의 내용을 저장합니다.
  /// </summary>
  public void SaveRTF()
  {
    using (MemoryStream ms = new MemoryStream())
    {
      TextRange range2 = new TextRange(RTBMain.Document.ContentStart, RTBMain.Document.ContentEnd);
      range2.Save(ms, DataFormats.Rtf);
      ms.Seek(0, SeekOrigin.Begin);
      using (StreamReader sr = new StreamReader(ms))
      {
        TempRTF = sr.ReadToEnd();
      }
    }
  }

  /// <summary>
  /// 저장했던 스크린의 내용을 불러옵니다.
  /// </summary>
  /// <exception cref="Exception">저장 된 내용이 없을 경우</exception>
  public void LoadRTF()
  {
    if (!String.IsNullOrEmpty(TempRTF))
    {
      MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(TempRTF));
      TextRange range = new TextRange(RTBMain.Document.ContentStart, RTBMain.Document.ContentEnd);
      range.Load(stream, DataFormats.Rtf);
      range.ApplyPropertyValue(TextElement.FontFamilyProperty, Font);
    }
    else throw new Exception("저장 된 내용이 없습니다.");
  }

  /// <summary>
  /// 글자를 읽어옵니다.
  /// </summary>
  /// <param name="callBack">글자를 읽어오고 콜백합니다.</param>
  /// <exception cref="Exception">현재 스크린이 다른 작업을 수행 중인 경우</exception>
  public void ReadText(Action<string> callBack)
  {
    if (CanTask)
    {
      IsReadingText = true;
      CanTask = false;
      TBInput.Clear();
      CallAfterReadingText = callBack;
      SaveRTF();
      Print("  ", new Pair<Brush>(BGColor, BGColorWhenReadText));
    }
    else
      ThrowWhenCantTask();
  }

  /// <summary>
  /// 키를 읽어옵니다.
  /// </summary>
  /// <param name="callBack">키를 읽어오고 콜백합니다.</param>
  /// <exception cref="Exception">현재 스크린이 다른 작업을 수행 중인 경우</exception>
  public void ReadKey(Action<Key> callBack) => ReadKey(null, callBack);

  /// <summary>
  /// 키를 읽어옵니다.
  /// </summary>
  /// <param name="key">입력 받을 키</param>
  /// <param name="callBack">키를 읽어오고 콜백합니다.</param>
  /// <exception cref="Exception">현재 스크린이 다른 작업을 수행 중인 경우</exception>
  public void ReadKey(Key? key, Action<Key> callBack)
  {
    if (CanTask)
    {
      IsReadingKey = true;
      CanTask = false;
      KeyToPress = key;
      CallAfterReadingKey = callBack;
    }
    else
      ThrowWhenCantTask();
  }

  /// <summary>
  /// 읽어오는 것을 취소합니다.
  /// </summary>
  /// <exception cref="Exception">현재 읽고있지 않을 경우</exception>
  public void ExitRead()
  {
    if (IsReadingKey || IsReadingText)
    {
      IsReadingKey = false;
      IsReadingText = false;
      CanTask = true;
    }
    else
      throw new Exception("not currently reading");
  }

  /// <summary>
  /// 스크린의 내용을 지웁니다.
  /// </summary>
  public void Clear()
  {
    if (CanTask)
    {
      RTBMain.Document.Blocks.Clear();
    }
    else
      ThrowWhenCantTask();
  }

  private void TBInput_KeyDown(object sender, KeyEventArgs e)
  {
    if (keyDownAvailability)
    {
      keyDownAvailability = false;
      tempKey = e.Key;
      if (IsReadingKey)
      {
        if (KeyToPress != null && e.Key != KeyToPress) return;

        IsReadingKey = false;
        CanTask = true;
        CallAfterReadingKey(e.Key);
      }
      else if (IsReadingText)
      {
        if (e.Key == Key.Enter)
        {
          IsReadingText = false;
          CanTask = true;
          CallAfterReadingText(TBInput.Text);
        }
      }
    }
  }

  private void TBInput_PreviewKeyUp(object sender, KeyEventArgs e)
  {
    if (tempKey == e.Key)
    {
      keyDownAvailability = true;
    }
  }

  private void TBInput_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (IsReadingText)
    {
      LoadRTF();
      Print($" {TBInput.Text} ", new Pair<Brush>(BGColor, BGColorWhenReadText));
    }
  }

  /// <summary>
  /// (추가 기능 전용) 스크린이 이미 작업 중일 경우 새로운 작업을 수행하려 시도 할 때 예외 합니다.
  /// </summary>
  public void ThrowWhenCantTask() => throw new Exception("It cannot be run while other operations are in progress.");

  /// <summary>
  /// 보조스크린을 엽니다.
  /// </summary>
  /// <param name="title">이름</param>
  /// <param name="size">크기</param>
  /// <param name="action">보조스크린</param>
  /// <param name="callBack">보조스크린이 종료된 후 리턴 값을 가지고 콜백합니다.</param>
  /// <param name="callBackOnForceExit">보조스크린이 강제로 종료될 경우 콜백합니다.</param>
  /// <exception cref="Exception">이미 보조스크린이 열려 있는 경우</exception>
  public void OpenSubScreen(string title, Size size, Action<Screen> action, Action<object> callBack,
    Action callBackOnForceExit = null)
  {
    if (!IsOpenedSubScreen)
    {
      CallBackAfterSubScreen = callBack;
      SubScreen = new SubScreen(this, title, size);
      grid.Children.Add(SubScreen);
      IsOpenedSubScreen = true;
      action(SubScreen.screen);
    }
    else throw new Exception("이미 보조스크린이 열려 있습니다.");
  }

  /// <summary>
  /// 열려 있는 보조스크린을 종료합니다.
  /// </summary>
  /// <param name="force">강제로 닫을지 여부</param>
  /// <exception cref="Exception">보조스크린이 열려 있지 않은 경우</exception>
  public void CloseSubScreen(bool force = true)
  {
    if (IsOpenedSubScreen)
    {
      grid.Children.Remove(SubScreen);
      SubScreen = null;
      IsOpenedSubScreen = false;
      if (force) CallBackAfterSubScreenForceExit();
    }
    else throw new Exception("보조스크린이 열려 있지 않습니다");
  }

  /// <summary>
  /// (현재 스크린이 보조스크린일 경우)
  /// 이 스크린을 종료합니다. 종료 후 부모 스크린에게 리턴 값을 보냅니다.
  /// </summary>
  /// <param name="return">부모 스크린에 보낼 값</param>
  /// <exception cref="Exception">현재 스크린이 보조스크린이 아닐 경우</exception>
  public void ExitSub(object @return)
  {
    if (IsSubScreen)
    {
      Parent.CloseSubScreen(false);
      Parent.CallBackAfterSubScreen(@return);
    }
    else throw new Exception("이 스크린은 보조스크린이 아닙니다.");
  }
}