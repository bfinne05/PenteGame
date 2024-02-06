using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Pente
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool isPVPGlobal;
		public bool player1turn = false;
		public string p1Name;
		public string p2Name;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow(bool isPVP, string player1, string player2)
		{
			InitializeComponent();
            isPVPGlobal = isPVP;
			Player1.Text = player1;
			Player2.Text = player2;
			p1Name = player1;
			p2Name = player2;

            switch (player1turn)
			{
				case true:
					PlayerTurn.Text = player1 + "'s Turn";
					break;
				case false:
					PlayerTurn.Text = player2 + "'s Turn";
					break;
			}

			TextBox box = new TextBox();
			box.Text = "O";
			Grid.SetColumn(box, 8);
			Grid.SetRow(box, 8);
			Board.Children.Add(box);

            if (!isPVP) AIMove();
		}

		private void Button_EndTurn(object sender, RoutedEventArgs e)
		{
			if (PlayerMoveX.Text == string.Empty || PlayerMoveY.Text == string.Empty)
			{
				Error.Text = "Please Enter A Value Before Ending Your Turn.";
			}
			else
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

				Error.Text = "";

				if (!isPVPGlobal)
				{
					AIMove();
				}
            }
        }

		public int RandomX()
		{
			Random rnd = new Random();
			int x = rnd.Next(1, 20);

			return x;
		}

        public int RandomY()
        {
            Random rnd = new Random();
            int y = rnd.Next(1, 20);

            return y;
        }

		public void AIMove()
		{
			int x = RandomX();
			int y = RandomY();

            TextBox box = new TextBox();

            box.Text = "X";
            player1turn = true;
            PlayerMoveX.Text = "";
            PlayerMoveY.Text = "";

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
