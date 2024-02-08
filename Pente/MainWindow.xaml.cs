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

				if (CheckSpace(x, y))
				{
					if (player1turn)
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "O";
						player1turn = false;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
						CheckPlayer1Win(x, y);
					}
					else
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "X";
						player1turn = true;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
						CheckPlayer2Win(x, y);
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

				if (!isPVPGlobal && !player1turn)
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
				Error.Text = "";
				return true;
			}
			else
			{
				Error.Text = "Piece is already placed there";
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

		public bool CheckPlayer1Win(int rowIndex, int ColumnIndex)
		{
			int tick = 0;
			int newRowIndex = rowIndex;
			int newColumnIndex = ColumnIndex;
			TextBlock text = Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex);
			Debug.WriteLine(text.Text);

			//horizontal check
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "O" && newRowIndex > 0)
			{
				tick++;
				newRowIndex--;
			}
			newRowIndex = rowIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "O" && newRowIndex < 20)
			{
				tick++;
				newRowIndex++;
			}

			if(tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				return true;
			}

			tick = 0;
			//vertical Check
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex > 0)
			{
				tick++;
				Debug.Write("Horizontal left");
				newColumnIndex--;
			}
			newColumnIndex = ColumnIndex; // Reset the column index
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex < 20)
			{
				tick++;
				Debug.Write("Horizontal right");
				newColumnIndex++;
			}

			//diagonal check
			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex < 20 && newRowIndex < 20)
			{
				tick++;
				Debug.Write("DiagonalUp");
				newColumnIndex++;
				newRowIndex++;
			}
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex > 0 && newRowIndex > 0)
			{
				tick++;
				Debug.Write("DiagonalDown");
				newColumnIndex--;
				newRowIndex--;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool CheckPlayer2Win(int rowIndex, int ColumnIndex)
		{
			int tick = 0;
			int newRowIndex = rowIndex;
			int newColumnIndex = ColumnIndex;
			TextBlock text = Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex);
			Debug.WriteLine(text.Text);

			//horizontal check
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "X" && newRowIndex > 0)
			{
				tick++;
				newRowIndex--;
			}
			newRowIndex = rowIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "X" && newRowIndex < 20)
			{
				tick++;
				newRowIndex++;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				return true;
			}

			tick = 0;
			//vertical Check
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex > 0)
			{
				tick++;
				Debug.Write("Horizontal left");
				newColumnIndex--;
			}
			newColumnIndex = ColumnIndex; // Reset the column index
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex < 20)
			{
				tick++;
				Debug.Write("Horizontal right");
				newColumnIndex++;
			}

			//diagonal check
			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex < 20 && newRowIndex < 20)
			{
				tick++;
				Debug.Write("DiagonalUp");
				newColumnIndex++;
				newRowIndex++;
			}
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex > 0 && newRowIndex > 0)
			{
				tick++;
				Debug.Write("DiagonalDown");
				newColumnIndex--;
				newRowIndex--;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 2 win");
				return true;
			}
			else
			{
				return false;
			}
		}


	}
}
