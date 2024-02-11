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
    /// Interaction logic for PVAI.xaml
    /// </summary>
    public partial class PVAI : Window
    {
        public string player1name;
        public string player2name;

        public PVAI()
        {
            InitializeComponent();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (Player1Name.Text != null || Player1Name.Text != string.Empty) player1name = Player1Name.Text;
            if (Player1Name.Text == null || Player1Name.Text == string.Empty ) player1name = "Player One";
            player2name = "Matt";
            int num = int.Parse(GridSet.Text);

            if(num >= 9 && num < 40 && num % 2 != 0)
            {
                Window game = new MainWindow(false, player1name, player2name, num + 1, false, 0, 0);
                game.Show();
                this.Close();
            }
            else if(num == 0 || GridSet.Text == string.Empty)
            {
				Window game = new MainWindow(false, player1name, player2name, 20, false, 0, 0);
				game.Show();
				this.Close();
			}
        }
    }
}
