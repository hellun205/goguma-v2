using System.Windows.Documents;
using System.Windows.Media;

namespace Goguma.Logger;

public class Logger
{
  private LoggerUI loggerUI = new LoggerUI();

  public string Logs
  {
    get
    {
      TextRange textRange = new TextRange(
        loggerUI.tbLog.Document.ContentStart,
        loggerUI.tbLog.Document.ContentEnd
      );
      return textRange.Text;
    }
  }

  public void OpenLogger()
  {
    loggerUI.Show();  
  }

  public void CloseLogger()
  {
    loggerUI.Close();
  }

  public void Log(string message, object? sender = null)
  {
    loggerUI.Log($"[{(sender != null ? $"{sender.GetType().Name}:" : "")}Log] : {message}");
  }
  
  public void Error(string message, object? sender = null)
  {
    loggerUI.Log($"[{(sender != null ? $"{sender.GetType().Name}:" : "")}Error] : {message}", Brushes.DarkRed);
  }  
  
  public void Warning(string message, object? sender = null)
  {
    loggerUI.Log($"[{(sender != null ? $"{sender.GetType().Name}:" : "")}Warning] : {message}", Brushes.Yellow);
  }
}