using System;
using System.Collections.Generic;
using System.Windows;

namespace GogumaWPF.Screen.Writing;

public sealed class English : Writing
{
  public English(int length) : base(7, 5, length)
  {
    var keys = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    Btns = new List<IWritingButton>();

    var i = 0;
    var y = 0;
    foreach (var key in keys)
    {
      ((List<IWritingButton>) Btns).Add(new Button("input", key.ToSymbol().ToString()));
      Location[(i + 1 - (y * 7)) - 1][y] = i;
      if ((i + 1) % 7 == 0) y++;
      i++;
    }

    ((List<IWritingButton>) Btns).AddRange(new IWritingButton[]
    {
      new Toggle("shift", "↑", "↓"),
      new Button("space", "　"),
      new Button("backspace", "←"),
      new Button("submit", "↵")
    });

    for (var j = 0; j < 4; j++)
    {
      Location[j][y + 1] = i + j;
    }
    
    AddEvent("input", btn => Text += btn.Display.ToDefault());
    AddEvent("shift", btn =>
    {
      BtnSetter("input",
        input => input.Display = (((Toggle) btn).IsClicked
          ? input.Display.ToDefault().ToUpper().ToSymbol()
          : input.Display.ToDefault().ToLower().ToSymbol()));
    });
    AddEvent("space", btn => Text += ' ');
    AddEvent("backspace", btn =>
    {
      if (Text.Length > 0)
        Text = Text.Remove(Text.Length - 1);
    });
    AddEvent("submit", btn => Submit());

  }
}