using System;
using System.Diagnostics;
using System.Linq;
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
			SetGrid(19, 19);	

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
				//checkWin(x, y, player1turn);

				if (CheckSpace(x, y))
				{
					if (player1turn)
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "O";
						player1turn = false;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
					}
					else
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "X";
						player1turn = true;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
					}

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
			if (CheckSpace(x, y))
			{
				Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "X";
				player1turn = true;
				PlayerMoveX.Text = "";
				PlayerMoveY.Text = "";

				

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
			else
			{
				AIMove();
			}
		}

		public bool CheckSpace(int columnIndex, int rowIndex)
		{
			TextBlock textBox = Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == columnIndex);
			if (textBox != null && textBox.Text == string.Empty)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private void SetGrid(int rows, int columns)
		{
			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < columns; col++)
				{
					TextBlock textBox = new TextBlock();
					Board.Children.Add(textBox);

					Grid.SetRow(textBox, row);
					Grid.SetColumn(textBox, col);
				}
			}
			Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == 8 && Grid.GetColumn(e) == 8).Text = "O";
		}

		public void checkWin(int columnIndex, int rowIndex, bool player1)
		{
			int tick = 0;
			//left check
			for(int i = rowIndex; i > rowIndex - 5; i--)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == columnIndex).Text == "O")
				{
					tick++;
				}
				if(tick == 4)
				{
					Debug.WriteLine("Win");
				}
			}
			tick = 0;
			//check right
			for (int i = rowIndex; i > rowIndex + 5; i++)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == columnIndex).Text == "O")
				{
					tick++;
				}
				if (tick == 4)
				{
					Debug.WriteLine("Win");
				}
			}
		}
	}
}
