using System.Drawing;
using GogumaConsole.Console;

namespace GogumaConsole;

internal class Program
{
  private const bool PrintErrorOnScreen = true;

  static void Main(string[] args)
  {
    void Play()
    {
      Goguma.Main.Initialize();
      Goguma.Main.Start();
    }

    if (PrintErrorOnScreen)
    {
      try
      {
        Play();
      }
      catch (Exception ex)
      {
        ConsoleUtil.Print(
          $"\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}",
          new(Color.DarkRed.ToConsoleColor(), ConsoleUtil.ConsoleClrFromRGB(125, 125, 125)));
        ConsoleUtil.Pause();
        Environment.Exit(0);
      }
    }
    else
      Play();
  }
}