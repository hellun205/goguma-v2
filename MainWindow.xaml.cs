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
using static goguma.ConsoleUtil;

namespace goguma
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      MainScreen = Screen;

      PrintF($"<fg='{Brushes.DeepSkyBlue}'>DeepSkyBlue <bg='{Brushes.Black}'>BlackBG <fg='{Brushes.Coral}' bg='{Brushes.Cyan}'>Coral And Cyan");
    }
  }
}
