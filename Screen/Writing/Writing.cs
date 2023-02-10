using System;
using System.Collections.Generic;
using System.Linq;

namespace GogumaWPF.Screen.Writing;

public class Writing
{
  private string _text = string.Empty;

  public string Text
  {
    get => _text;
    protected set
    {
      if (value.Length <= Length)
        _text = value;
    }
  }

  public IEnumerable<IWritingButton> Btns { get; protected set; }

  public List<List<int?>> Location { get; protected set; }

  public Dictionary<string, Action> TagEvents { get; protected set; }

  public Pair<int> Count { get; private set; }

  public IEnumerable<IWritingButton>? GetBtns(string tag) =>
    (Btns.FirstOrDefault(x => x.Tag == tag) != null ? Btns.Where(x => x.Tag == tag) : null);

  public bool isWhile { get; private set; } = true;

  public int Length { get; }

  public void AddEvent(string tag, Action<IWritingButton> action) =>
    BtnSetter(tag, btn => btn.OnClicked += sender => action(sender));

  public void BtnSetter(string tag, Action<IWritingButton> setter)
  {
    var btns = GetBtns(tag);
    if (btns != null)
    {
      foreach (var btn in btns)
        setter(btn);
    }
    else throw new Exception("invalid tag");
  }

  public Writing(int x, int y, int length)
  {
    Count = new(x, y);
    Length = length;
    Location = new List<List<int?>>();
    for (var i = 0; i < x; i++)
    {
      Location.Add(new List<int?>());
      for (var j = 0; j < y; j++)
      {
        Location[i].Add(null);
      }
    }
  }

  protected void Submit()
  {
    isWhile = false;
  }
}