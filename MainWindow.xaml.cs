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
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      Screen.Print("READ: ");
      Screen.ReadText(() =>
      {
        Screen.Print($"\n you inputted \"{Screen.TextOfRead}\"");
        Screen.Print("\nPress any key to continue!");
        Screen.ReadKey(() =>
        {
          Screen.Print($"\nGood! you press \"{Screen.KeyOfRead}\"");
        });
      });
    }
  }
}
