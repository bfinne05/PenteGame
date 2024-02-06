using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

namespace Pente
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		PVP pvp;
		//PVAI pvai;
		public bool isPVP = true;
		public bool player1turn = false;
		public string p1Name;
		public string p2Name;
		
		public MainWindow(PVP pvpI, string player1, string player2)
		{
			InitializeComponent();
			Player1.Text = player1;
			Player2.Text = player2;
			pvp = pvpI;
			p1Name = player1;
			p2Name = player2;
			if (isPVP)
			{
                switch (player1turn)
                {
                    case true:
                        PlayerTurn.Text = player1 + "'s Turn";
                        break;
                    case false:
						PlayerTurn.Text = player2 + "'s Turn";
                        break;
                }
            }

			TextBox box = new TextBox();
			box.Text = "O";
			Grid.SetColumn(box, 8);
			Grid.SetRow(box, 8);
			Board.Children.Add(box);
		}

		private void Button_EndTurn(object sender, RoutedEventArgs e)
		{
			int x = int.Parse(PlayerMoveX.Text);
			int y = int.Parse(PlayerMoveY.Text);

			TextBox box = new TextBox();
			if (player1turn)
			{
				box.Text = "O";
				player1turn = false;
				PlayerMoveX.Text = "";
				PlayerMoveY.Text = "";
			}
			else
			{
				box.Text = "X";
				player1turn = true;
				PlayerMoveX.Text = "";
				PlayerMoveY.Text = "";
			}

			// Set the column for the TextBox
			Grid.SetColumn(box, x);
			Grid.SetRow(box, y);

			// Add the TextBox to the specified row and column in the Board grid
			Board.Children.Add(box);

            switch (player1turn)
            {
                case true:
                    PlayerTurn.Text = p1Name + "'s Turn";
                    break;
                case false:
                    PlayerTurn.Text = p2Name + "'s Turn";
                    break;
            }
        }
	}
}
