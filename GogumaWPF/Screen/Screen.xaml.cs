﻿using GogumaWPF;
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

  public KeySet<Key> KeySet { get; set; } = new KeySet<Key>(Key.Up, Key.Down, Key.Left, Key.Right, Key.Enter);

  public bool IsReadingText = false;
  public bool IsReadingKey = false;
  public Action<string> CallAfterReadingText;
  public Action<Key> CallAfterReadingKey;
  public string TempRTF;
  public Key? KeyToPress;
  
  /// <summary>
  /// X : 키 다운 가능 여부, Y : 키 다운했던 키
  /// </summary>
  private Pair<bool, Key> keyDown;

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
    if (!IsReadingText && !IsReadingKey)
    {
      IsReadingText = true;
      TBInput.Clear();
      CallAfterReadingText = callBack;
      SaveRTF();
      Print("  ", new Pair<Brush>(BGColor, BGColorWhenReadText));
    }
    else if (IsReadingText)
      throw new Exception("이미 텍스트를 읽고 있습니다.");
    else if (IsReadingKey)
      throw new Exception("현재 키를 읽고 있으므로 텍스트를 읽을 수 없습니다.");
  }

  public void ReadKey(Action<Key> callBack) => ReadKey(null, callBack);

  public void ReadKey(Key? press, Action<Key> callBack)
  {
    if (!IsReadingText && !IsReadingKey)
    {
      IsReadingKey = true;
      KeyToPress = press;
      CallAfterReadingKey = callBack;
    }
    else if (IsReadingKey)
      throw new Exception("이미 키를 읽고 있습니다.");
    else if (IsReadingText)
      throw new Exception("현재 텍스트를 읽고 있으므로 키를 읽을 수 없습니다.");
  }

  public void ExitRead()
  {
    if (IsReadingKey || IsReadingText)
    {
      IsReadingKey = false;
      IsReadingText = false;
    }
    else 
      throw new Exception("현재 읽는 중이 아닙니다.");
  }

  public void Clear()
  {
    if (!IsReadingText && !IsReadingKey)
    {
      RTBMain.Document.Blocks.Clear();
    }
    else if (IsReadingText)
      throw new Exception("텍스트를 읽는 중에는 모든 텍스트를 지울 수 없습니다.");
    else if (IsReadingKey)
      throw new Exception("키를 읽는 중에는 모든 텍스트를 지울 수 없습니다.");
  }

  private void TBInput_KeyDown(object sender, KeyEventArgs e)
  {
    if (!keyDown.X)
    {
      keyDown.X = true;
      keyDown.Y = e.Key;
      if (IsReadingKey)
      {
        if (KeyToPress != null && e.Key != KeyToPress) return;

        IsReadingKey = false;
        CallAfterReadingKey(e.Key);
      }
      else if (IsReadingText)
      {
        if (e.Key == Key.Enter)
        {
          IsReadingText = false;
          CallAfterReadingText(TBInput.Text);
        }
      }
    }
  }

  private void TBInput_PreviewKeyUp(object sender, KeyEventArgs e)
  {
    if (keyDown.Y == e.Key)
    {
      keyDown.X = false;
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
}