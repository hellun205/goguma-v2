using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace goguma
{
  /// <summary>
  /// Interaction logic for Screen.xaml
  /// </summary>
  public partial class Screen : UserControl
  {
    public object Font => FindResource("Galmuri11");
    public Brush BGColor => RTBMain.Background;

    public Screen()
    {
      InitializeComponent();
    }

    private void RTBMain_GotFocus(object sender, RoutedEventArgs e)
    {
      TBInput.Focus();
    }

    public void Print(string text)
    {
      Print(text, Brushes.White, BGColor);
    }

    public void Print(string text, Brush fgColor, Brush bgColor)
    {
      TextRange tr = new TextRange(RTBMain.Document.ContentEnd, RTBMain.Document.ContentEnd);
      tr.Text = text;
      tr.ApplyPropertyValue(TextElement.FontFamilyProperty, Font);
      tr.ApplyPropertyValue(TextElement.ForegroundProperty, fgColor);
      tr.ApplyPropertyValue(TextElement.BackgroundProperty, bgColor);
    }

    private void RTBMain_TextChanged(object sender, TextChangedEventArgs e)
    {
      RTBMain.ScrollToEnd();
    }


  }
}
