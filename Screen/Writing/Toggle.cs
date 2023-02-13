using System;

namespace Goguma.Screen.Writing;

public class Toggle : Button
{
  public string DisplayOnTrue { get;  }
  
  public string DisplayOnFalse { get; }
  
  public bool IsClicked { get; private set; }
  
  public override void OnClick()
  {
    IsClicked = !IsClicked;
    Display = (IsClicked ? DisplayOnTrue : DisplayOnFalse);
    base.OnClick();
  }

  public Toggle(string tag, string displayOnFalse, string displayOnTrue, bool isClicked = false) : base(tag, (isClicked ? displayOnTrue : displayOnFalse))
  {
    IsClicked = isClicked;
    DisplayOnFalse = displayOnFalse;
    DisplayOnTrue = displayOnTrue;
  }
  
}