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

namespace Goguma.Screen
{
  /// <summary>
  /// Interaction logic for SubScreen.xaml
  /// </summary>
  public partial class SubScreen : UserControl
  {
    protected SubScreen()
    {
      InitializeComponent();
      BGColor = screen.BGColor;
      FGColor = screen.FGColor;
      screen.IsSubScreen = true;
    }

    public SubScreen(Screen parent, string title, Size size) : this()
    {
      this.Width = size.Width;
      this.Height = size.Height;
      borderBox.Header = title;
      BGColor = parent.BGColor;
      FGColor = parent.FGColor;
      screen.Parent = parent;
    }
    
    public Brush BGColor
    {
      get => screen.BGColor;
      set
      {
        screen.BGColor = value;
        Background = value;
      }
    }
    
    public Brush FGColor
    {
      get => screen.FGColor;
      set
      {
        screen.FGColor = value;
        Foreground = value;
      }
    }

    private void SubScreen_OnLoaded(object sender, RoutedEventArgs e)
    {
      screen.Focus();
    }
  }
}
