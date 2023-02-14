using System;
using System.IO;
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
  /// <summary>
  /// 현재 포커싱 되어 있는 스크린입니다.
  /// </summary>
  public static Screen? MainScreen;

  public static Panel ParentGrid;

  /// <summary>
  /// 스크린의 폰트입니다.
  /// </summary>
  public object Font => FindResource("Galmuri11");

  /// <summary>
  /// 스크린의 키 셋팅입니다.
  /// </summary>
  public KeySet<Key> KeySet { get; set; } = new KeySet<Key>(Key.Up, Key.Down, Key.Left, Key.Right, Key.Z, Key.X);

  /// <summary>
  /// 배경 색
  /// </summary>
  public Brush BGColor
  {
    get => RTBMain.Background;
    set => RTBMain.Background = value;
  }

  /// <summary>
  /// 글자 색
  /// </summary>
  public Brush FGColor
  {
    get => RTBMain.Foreground;
    set => RTBMain.Foreground = value;
  }

  /// <summary>
  /// 현재 텍스트를 읽고 있는 지에 대한 여부입니다.
  /// </summary>
  public bool IsReadingText { get; private set; } = false;

  /// <summary>
  /// 현재 키를 읽고 있는 지에 대한 여부입니다.
  /// </summary>
  public bool IsReadingKey { get; private set; } = false;

  /// <summary>
  /// 현재 보조스크린이 열려 있는 지에 대한 여부입니다.
  /// </summary>
  public bool IsOpenedSubScreen { get; private set; } = false;

  /// <summary>
  /// (확장 기능 전용)
  /// 저장 된 스크린의 내용입니다. 
  /// </summary>
  public string SavedRTF { get; set; }

  /// <summary>
  /// 현재 이 스크린이 다른 작업을 시행 중인지에 대한 여부입니다.
  /// </summary>
  public bool CanTask { get; set; } = true;

  /// <summary>
  /// 현재 스크린의 보조스크린 입니다.
  /// </summary>
  public SubScreen? SubScreen { get; private set; }

  /// <summary>
  /// 현재 스크린이 보조스크린인 지에 대한 여부입니다.
  /// </summary>
  public bool IsSubScreen { get; set; } = false;

  /// <summary>
  /// (현재 스크린이 보조스크린인 경우)
  /// 이 스크린의 부모 스크린입니다.
  /// </summary>
  public Screen? Parent { get; set; }

  public delegate void _onKeyPress(Screen sender, KeyEventArgs keyEventArgs);

  /// <summary>
  /// 키가 입력 받았을 때 호출 됩니다.
  /// </summary>
  public static event _onKeyPress? OnKeyPress;

  /// <summary>
  /// 키가 입력 받았을 때 호출되는 이벤트를 무시합니다.
  /// </summary>
  public static bool IgnoreKeyPressEvent { get; set; } = false;

  /// <summary>
  /// 스크린의 내용이 변경 되었을 때 자동으로 맨 밑으로 스크롤 되는 지에 대한 여부입니다.
  /// </summary>
  public bool ScrollToEnd { get; set; } = true;

  public bool AutoSetTextAlign { get; set; } = false;

  public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

  private Brush BGColorWhenReadText = Brushes.DimGray;
  private bool keyDownAvailability = true;
  private Key tempKey;
  private Key? keyToPress;
  private Action<string> callBackAfterReadingText;
  private Action<Key> callBackAfterReadingKey;
  private Action<object> callBackAfterSubScreen;
  private Action? callBackAfterSubScreenForceExit;

  public Screen()
  {
    InitializeComponent();
  }

  #region event

  private void GotFocuss(object sender, RoutedEventArgs e)
  {
    if (MainScreen == null || MainScreen == this)
    {
      MainScreen = this;
      TBInput.Focus();
    }
    else
    {
      MainScreen.TBInput.Focus();
    }
  }

  private void TBInput_OnGotFocus(object sender, RoutedEventArgs e)
  {
    // if (IsOpenedSubScreen) SubScreen.screen.TBInput.Focus();
  }

  private void RTBMain_TextChanged(object sender, TextChangedEventArgs e)
  {
    if (ScrollToEnd) RTBMain.ScrollToEnd();
    if (AutoSetTextAlign) SetTextAlignment(TextAlignment);
  }

  private void OnKeyDown(object sender, KeyEventArgs e)
  {
    if (keyDownAvailability)
    {
      keyDownAvailability = false;
      tempKey = e.Key;
      if (!IgnoreKeyPressEvent) OnKeyPress?.Invoke(this, e);
      if (IsReadingKey)
      {
        if (keyToPress != null && e.Key != keyToPress) return;

        IsReadingKey = false;
        CanTask = true;
        callBackAfterReadingKey(e.Key);
      }
      else if (IsReadingText)
      {
        if (e.Key == Key.Enter)
        {
          IsReadingText = false;
          CanTask = true;
          callBackAfterReadingText(TBInput.Text);
        }
      }
    }
  }

  private void OnKeyUp(object sender, KeyEventArgs e)
  {
    if (tempKey == e.Key)
    {
      keyDownAvailability = true;
    }
  }

  private void OnTextChanged(object sender, TextChangedEventArgs e)
  {
    if (IsReadingText)
    {
      LoadRTF();
      Print($" {TBInput.Text} ", new Pair<Brush>(BGColor, BGColorWhenReadText));
    }
  }

  #endregion

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
        SavedRTF = sr.ReadToEnd();
      }
    }
  }

  /// <summary>
  /// 저장했던 스크린의 내용을 불러옵니다.
  /// </summary>
  /// <exception cref="Exception">저장 된 내용이 없을 경우</exception>
  public void LoadRTF()
  {
    if (!String.IsNullOrEmpty(SavedRTF))
    {
      MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(SavedRTF));
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
      callBackAfterReadingText = callBack;
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
      keyToPress = key;
      callBackAfterReadingKey = callBack;
    }
    else
      ThrowWhenCantTask();
  }

  /// <summary>
  /// 읽어오는 것을 취소합니다.
  /// </summary>
  /// <exception cref="Exception">현재 읽고 있지 않을 경우</exception>
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

  /// <summary>
  /// (추가 기능 전용) 스크린이 이미 다른 작업을 시행 중일 경우 새로운 작업을 수행하려 할 때 예외 합니다.
  /// </summary>
  public void ThrowWhenCantTask() => throw new Exception("It cannot be run while other operations are in progress.");

  /// <summary>
  /// 보조스크린을 엽니다.
  /// </summary>
  /// <param name="title">이름</param>
  /// <param name="size">크기</param>
  /// <param name="action">보조스크린에 대한 작업</param>
  /// <param name="callBack">보조스크린이 종료된 후 리턴 값을 가지고 콜백합니다.</param>
  /// <param name="callBackOnForceExit">보조스크린이 강제로 종료될 경우 콜백합니다.</param>
  /// <exception cref="Exception">이미 보조스크린이 열려 있는 경우</exception>
  public void OpenSubScreen(string title, Size size, Action<Screen> action, Action<object> callBack,
    Action callBackOnForceExit = null)
  {
    if (!IsOpenedSubScreen)
    {
      callBackAfterSubScreen = callBack;
      SubScreen = new SubScreen(this, title, size);
      ParentGrid.Children.Add(SubScreen);
      IsOpenedSubScreen = true;
      action(SubScreen.screen);
      MainScreen = SubScreen.screen;
    }
    else throw new Exception("이미 보조스크린이 열려 있습니다.");
  }

  /// <summary>
  /// 열려 있는 보조스크린을 종료합니다.
  /// </summary>
  /// <param name="force">강제로 닫을지에 대한 여부</param>
  /// <exception cref="Exception">보조스크린이 열려 있지 않은 경우</exception>
  public void CloseSubScreen(bool force = true)
  {
    if (IsOpenedSubScreen)
    {
      ParentGrid.Children.Remove(SubScreen);
      SubScreen = null;
      IsOpenedSubScreen = false;
      if (force) callBackAfterSubScreenForceExit();
      MainScreen = this;
    }
    else throw new Exception("보조스크린이 열려 있지 않습니다");
  }

  /// <summary>
  /// (현재 스크린이 보조스크린일 경우)
  /// 이 스크린을 종료합니다. 종료 후 부모 스크린에게 리턴 값을 보냅니다.
  /// </summary>
  /// <param name="return">부모 스크린에 보낼 값</param>
  /// <exception cref="Exception">현재 스크린이 보조스크린이 아닐 경우</exception>
  public void ExitSub(object @return = null)
  {
    if (IsSubScreen)
    {
      Parent.CloseSubScreen(false);
      Parent.callBackAfterSubScreen(@return);
      Parent.TBInput.Focus();
      Parent.keyDownAvailability = true;
      MainScreen = Parent;
    }
    else throw new Exception("이 스크린은 보조스크린이 아닙니다.");
  }
  
  public void SetTextAlignment(TextAlignment textAlignment) {
    BlockCollection MyBC = RTBMain.Document.Blocks;
    foreach (Block b in MyBC)
      b.TextAlignment = textAlignment;    
  }
}