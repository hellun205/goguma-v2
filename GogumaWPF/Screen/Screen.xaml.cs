using GogumaWPF;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace GogumaWPF.Screen;

/// <summary>
/// Interaction logic for Screen.xaml
/// </summary>
public partial class Screen : UserControl
{
  public object Font => FindResource("Galmuri11");

  public Brush BGColor => RTBMain.Background;

  public Brush FGColor => RTBMain.Foreground;

  public Brush BGColorWhenReadText { get; set; } = Brushes.DimGray;

  public KeySet<Key> KeySet { get; set; } = new KeySet<Key>(Key.Up, Key.Down, Key.Left, Key.Right, Key.Z);

  public bool IsReadingText = false;
  public bool IsReadingKey = false;
  public Action<string> CallAfterReadingText;
  public Action<Key> CallAfterReadingKey;
  public string TempRTF;
  public Key? KeyToPress;
  public bool CanTask = true;

  private bool keyDownAvailability = true;
  private Key tempKey;

  public Screen()
  {
    InitializeComponent();
  }

  private void RTBMain_GotFocus(object sender, RoutedEventArgs e)
  {
    TBInput.Focus();
  }

  public void Print(string text)
  {
    Print(text, new Pair<Brush>(FGColor, BGColor));
  }

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

  public void LoadRTF()
  {
    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(TempRTF));
    TextRange range = new TextRange(RTBMain.Document.ContentStart, RTBMain.Document.ContentEnd);
    range.Load(stream, DataFormats.Rtf);
    range.ApplyPropertyValue(TextElement.FontFamilyProperty, Font);
  }

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

  public void ReadKey(Action<Key> callBack) => ReadKey(null, callBack);

  public void ReadKey(Key? press, Action<Key> callBack)
  {
    if (CanTask)
    {
      IsReadingKey = true;
      CanTask = false;
      KeyToPress = press;
      CallAfterReadingKey = callBack;
    }
    else
      ThrowWhenCantTask();
  }

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

  public void ThrowWhenCantTask() => throw new Exception("It cannot be run while other operations are in progress.");
}