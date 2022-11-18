using System;

namespace GogumaWPF.Screen.Writing;

public class Button : IWritingButton
{
  public string Display { get; set; }

  public virtual void OnClick()
  {
    OnClicked?.Invoke(this);
  }

  public string Tag { get; }
  
  public event IWritingButton._onClicked? OnClicked;

  public Button(string tag, string display)
  {
    Display = display;
    Tag = tag;
  }
}