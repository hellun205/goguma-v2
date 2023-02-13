using System;

namespace Goguma.Screen.Writing;

public interface IWritingButton
{
  public delegate void _onClicked(IWritingButton sender);
  
  public string Display { get; set; }

  public void OnClick();
  
  public string Tag { get; }

  public event _onClicked OnClicked;
}