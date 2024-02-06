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

            Window game = new MainWindow(false, player1name, player2name);
            game.Show();
        }
    }
}
