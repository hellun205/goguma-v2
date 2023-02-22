using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Goguma.Logger;

public partial class LoggerUI
{
  public LoggerUI()
  {
    InitializeComponent();
  }

  private void OnLoggerChanged(object sender, TextChangedEventArgs e)
  {
    tbLog.ScrollToEnd();
  }
  
  private void Print(string text) => Print(text, Foreground, Background);
  private void Print(string text, Brush fgColor) => Print(text,fgColor, Background);
  private void Print(string text, Brush fgColor, Brush bgColor)
  {
    TextRange tr = new TextRange(tbLog.Document.ContentEnd, tbLog.Document.ContentEnd);
    tr.Text = text;
    tr.ApplyPropertyValue(TextElement.ForegroundProperty, fgColor);
    tr.ApplyPropertyValue(TextElement.BackgroundProperty, bgColor);
  }

  public void Log(string message) => Log(message, Foreground,Background);
  public void Log(string message, Brush fgColor) => Log(message, fgColor, Background);
  public void Log(string message, Brush fgColor, Brush bgColor)
  {
    Print($"{GetTime()} {message}", fgColor, bgColor);
    Print("\n");
  }

  private string GetTime() => DateTime.Now.ToString("[yy/MM/dd] [HH:mm:ss]");
}