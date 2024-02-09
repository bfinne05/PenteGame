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
using System.Windows.Shapes;

namespace Pente
{
    /// <summary>
    /// Interaction logic for WinScreen.xaml
    /// </summary>
    public partial class WinScreen : Window
    {
        public WinScreen(bool playerWin)
        {
            InitializeComponent();
        }

		private void PlayAgain_Click(object sender, RoutedEventArgs e)
		{
            Window game = new TitleScreen();
            game.Show();
            this.Close();
		}

		private void Quit_Click(object sender, RoutedEventArgs e)
		{
            this.Close();
		}
	}
}
