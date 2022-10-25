using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace goguma
{
  /// <summary>
  /// Interaction logic for Screen.xaml
  /// </summary>
  public partial class Screen : UserControl
  {
    public object Font => FindResource("Galmuri11");
    public Brush BGColor => RTBMain.Background;
    public Brush FGColor => RTBMain.Foreground;
    public Brush BGColorWhenReadText { get; set; } = Brushes.DimGray;
    public string TextOfRead;
    public Key KeyOfRead;

    private bool isReadingText = false;
    private bool isReadingKey = false;
    private Action CallAfterReadingText;
    private Action CallAfterReadingKey;
    private string tempRTF;

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
      RTBMain.ScrollToEnd();
    }

    private void SaveRTF()
    {
      using (MemoryStream ms = new MemoryStream())
      {
        TextRange range2 = new TextRange(RTBMain.Document.ContentStart, RTBMain.Document.ContentEnd);
        range2.Save(ms, DataFormats.Rtf);
        ms.Seek(0, SeekOrigin.Begin);
        using (StreamReader sr = new StreamReader(ms))
        {
          tempRTF = sr.ReadToEnd();
        }
      }
    }

    private void LoadRTF()
    {
      MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(tempRTF));
      TextRange range = new TextRange(RTBMain.Document.ContentStart, RTBMain.Document.ContentEnd);
      range.Load(stream, DataFormats.Rtf);
      range.ApplyPropertyValue(TextElement.FontFamilyProperty, Font);
    }

    /// <summary>
    /// 텍스트를 입력 받고, 받은 후 함수를 호출합니다.
    /// </summary>
    /// <param name="CallAfterReading">텍스트를 입력 받은 후 호출될 함수</param>
    public void ReadText(Action CallAfterReading)
    {
      if (!isReadingText && !isReadingKey)
      {
        isReadingText = true;
        TBInput.Clear();
        CallAfterReadingText = CallAfterReading;
        SaveRTF();
        Print("  ", new Pair<Brush>(BGColor, BGColorWhenReadText));
      }
      else if (isReadingText)
        throw new Exception("이미 텍스트를 읽고 있습니다.");
      else if (isReadingKey)
        throw new Exception("현재 키를 읽고 있으므로 텍스트를 읽을 수 없습니다.");
    }

    public void ReadKey(Action CallAfterReading)
    {
      if (!isReadingText && !isReadingKey)
      {
        isReadingKey = true;
        CallAfterReadingKey = CallAfterReading;
      }
      else if (isReadingKey)
        throw new Exception("이미 키를 읽고 있습니다.");
      else if (isReadingText)
        throw new Exception("현재 텍스트를 읽고 있으므로 키를 읽을 수 없습니다.");
    }

    public void Clear()
    {
      if (!isReadingText && !isReadingKey)
      {
        RTBMain.Document.Blocks.Clear();
      }
      else if (isReadingText)
        throw new Exception("텍스트를 읽는 중에는 모든 텍스트를 지울 수 없습니다.");
      else if (isReadingKey)
        throw new Exception("키를 읽는 중에는 모든 텍스트를 지울 수 없습니다.");

    }

    private void TBInput_KeyDown(object sender, KeyEventArgs e)
    {
      if (isReadingKey)
      {
        isReadingKey = false;
        KeyOfRead = e.Key;
        CallAfterReadingKey();
      }
      else if (isReadingText)
      {
        if (e.Key == Key.Enter)
        {
          isReadingText = false;
          TextOfRead = TBInput.Text;
          CallAfterReadingText();
        }
      }

    }

    private void TBInput_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (isReadingText)
      {
        LoadRTF();
        Print($" {TBInput.Text} ", new Pair<Brush>(BGColor, BGColorWhenReadText));
      }
    }
  }
}
