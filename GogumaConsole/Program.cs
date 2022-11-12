using GogumaConsole.Goguma;

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
          $"{AnsiColor.RED + AnsiColor.WHITE_BG}\nERROR: {ex.Message}\nSOURCE: {ex.Source ?? ""}\nTARGETSITE: {ex.TargetSite}\nSTACKTRACE ---\n{ex.StackTrace ?? ""}");
        ConsoleUtil.Pause();
        Environment.Exit(0);
      }
    }
    else
      Play();
  }
}