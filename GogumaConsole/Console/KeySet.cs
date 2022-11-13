namespace GogumaConsole.Console;

public class KeySet<T>
{
  public T Up { get; set; }
  
  public T Down { get; set; }
  
  public T Left { get; set; }
  
  public T Right { get; set; }
  
  public T Enter { get; set; }
  
  public KeySet(T up, T down, T left, T right, T enter)
  {
    Up = up;
    Down = down;
    Left = left;
    Right = right;
    Enter = enter;
  }
}