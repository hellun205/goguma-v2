namespace GogumaConsole;

[Obsolete("Not supported by Windows cmd", true)]
public static class AnsiColor
{
  private const string ESCCODE = "\u001B";
  // public const string RESET = "$"{ESCCODE}[0m";
  public const string RESET = $"{ESCCODE}[40m{ESCCODE}[37m";

  public const string BLACK = $"{ESCCODE}[30m";
  public const string RED = $"{ESCCODE}[31m";
  public const string GREEN = $"{ESCCODE}[32m";
  public const string YELLOW = $"{ESCCODE}[33m";
  public const string BLUE = $"{ESCCODE}[34m";
  public const string PURPLE = $"{ESCCODE}[35m";
  public const string CYAN = $"{ESCCODE}[36m";
  public const string WHITE = $"{ESCCODE}[37m";
  public const string DEF = $"{ESCCODE}[39m";

  public const string BLACK_BG = $"{ESCCODE}[40m";
  public const string RED_BG = $"{ESCCODE}[41m";
  public const string GREEN_BG = $"{ESCCODE}[42m";
  public const string YELLOW_BG = $"{ESCCODE}[43m";
  public const string BLUE_BG = $"{ESCCODE}[44m";
  public const string PURPLE_BG = $"{ESCCODE}[45m";
  public const string CYAN_BG = $"{ESCCODE}[46m";
  public const string WHITE_BG = $"{ESCCODE}[47m";
  public const string DEF_BG = $"{ESCCODE}[49m";

  public static string GetRGBFG(byte red, byte green, byte blue) => $"{ESCCODE}[38;2;{red};{green};{blue}m";
  
  public static string GetRGBBG(byte red, byte green, byte blue) => $"{ESCCODE}[48;2;{red};{green};{blue}m";
  
  public static string Get256FG(byte id) => $"{ESCCODE}[38;5;{id}m";
  
  public static string Get256BG(byte id) => $"{ESCCODE}[48;5;{id}m";
  
}