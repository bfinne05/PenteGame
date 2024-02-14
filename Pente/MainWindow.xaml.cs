using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
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
		public int player1Captures = 0;
		public int player2Captures = 0;
		public int number;
		bool firstPlace = true;
		public int middlePlace = 0;
		public int time = 20;
		private DispatcherTimer timer;
		Grid gridSave;


		public MainWindow(bool isPVP, string player1, string player2, int num, bool player1turn, int player1captures, int player2captures, Grid grid)
		{
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Tick += Timer_Tick;
			timer.Start();
			isPVPGlobal = isPVP;
			Player1.Text = player1;
			Player2.Text = player2;
			p1Name = player1;
			p2Name = player2;
			number = num;
			gridSave = grid;
			player1Captures = player1captures;
			player2Captures = player2captures;
			middlePlace = (number / 2);
			Debug.WriteLine(middlePlace);

			switch (player1turn)
			{
				case true:
					PlayerTurn.Text = player1 + "'s Turn";
					break;
				case false:
					PlayerTurn.Text = player2 + "'s Turn";
					break;
			}

			SetGrid(num, num);
			

			if (!isPVP) AIMove();
		}

		public void Button_EndTurn(object sender, RoutedEventArgs e)
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
					if (player1turn && firstPlace)
					{
						if (Player1Set(x, y))
						{
							Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "O";
							player1turn = false;
							PlayerMoveX.Text = "";
							PlayerMoveY.Text = "";
							timer.Stop();
							time = 20;
							timer.Start();
						}
					}
					else if (player1turn)
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "O";
						player1turn = false;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
						checkPlayer1Take(x, y);
						CheckPlayer1Win(x, y);
						timer.Stop();
						time = 20;
						timer.Start();

					}
					else
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "X";
						player1turn = true;
						PlayerMoveX.Text = "";
						PlayerMoveY.Text = "";
						checkPlayer2Take(x, y);
						CheckPlayer2Win(x, y);
						timer.Stop();
						time = 20;
						timer.Start();
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
			int x = rnd.Next(1, number);

			return x;
		}

		public int RandomY()
		{
			Random rnd = new Random();
			int y = rnd.Next(1, number);

			return y;
		}

		public void AIMove()
		{
			int x = RandomX();
			int y = RandomY();
			if (CheckSpace(x, y))
			{
				Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y).Text = "X";
				checkPlayer2Take(x, y);
				CheckPlayer2Win(x, y);
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

		public bool CheckSpace(int rowIndex, int columnIndex)
		{
			TextBlock textBox = Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == columnIndex);
			if (textBox != null && textBox.Text == string.Empty && rowIndex >= 0 && rowIndex < number && columnIndex >= 0 && columnIndex < number)
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
				Board.RowDefinitions.Add(new RowDefinition());
			}
			for (int col = 0; col < columns; col++)
			{
				Board.ColumnDefinitions.Add(new ColumnDefinition());
			}
			for (int row = 0; row < rows; row++)
			{
				for (int col = 0; col < columns; col++)
				{
					TextBlock textBox = new TextBlock();
					TextBlock.SetTextAlignment(textBox, TextAlignment.Center);
					Board.Children.Add(textBox);
					Grid.SetRow(textBox, row);
					Grid.SetColumn(textBox, col);
				}
			}
			if (gridSave != null)
			{
				for (int row = 0; row < rows; row++)
				{
					for (int col = 0; col < columns; col++)
					{
						TextBlock saveTextBox = gridSave.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

	
						TextBlock gridTextBox = Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

						if (saveTextBox != null && gridTextBox != null)
						{
							gridTextBox.Text = saveTextBox.Text;
						}
					}
				}
			}
			else
			{
				Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == number / 2 && Grid.GetColumn(e) == number / 2).Text = "O";
			}
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
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "O" && newRowIndex < number)
			{
				tick++;
				newRowIndex++;
			}

			if (tick == 3)
			{
				Error.Text = p1Name + " Tria";
			}

			if (tick == 4)
			{
				Error.Text = p1Name + " Tessera";
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				Window game = new WinScreen(true);
				game.Show();
				this.Close();
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
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex < number)
			{
				tick++;
				Debug.Write("Horizontal right");
				newColumnIndex++;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				Window game = new WinScreen(true);
				game.Show();
				this.Close();
				return true;
			}

			//diagonal check
			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex < number && newRowIndex < number)
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
				Window game = new WinScreen(true);
				game.Show();
				this.Close();
				return true;
			}

			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex < number && newRowIndex > 0)
			{
				tick++;
				Debug.Write("DiagonalUp");
				newColumnIndex++;
				newRowIndex--;
			}
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "O" && newColumnIndex > 0 && newRowIndex < number)
			{
				tick++;
				Debug.Write("DiagonalDown");
				newColumnIndex--;
				newRowIndex++;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 1 win");
				Window game = new WinScreen(true);
				game.Show();
				this.Close();
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
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == ColumnIndex).Text == "X" && newRowIndex < number)
			{
				tick++;
				newRowIndex++;
			}

			if (tick == 3)
			{
				Error.Text = p2Name + " Tria";
			}

			if (tick == 4)
			{
				Error.Text = p2Name + " Tessera";
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 2 win");
				Window game = new WinScreen(false);
				game.Show();
				this.Close();
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
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex < number)
			{
				tick++;
				Debug.Write("Horizontal right");
				newColumnIndex++;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 2 win");
				Window game = new WinScreen(false);
				game.Show();
				this.Close();
				return true;
			}

			//diagonal check upper right and right down
			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex < number && newRowIndex < number)
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
				Window game = new WinScreen(false);
				game.Show();
				return true;
			}

			//diagonal check upper left and left down
			tick = 0;
			newRowIndex = rowIndex;
			newColumnIndex = ColumnIndex;
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex < number && newRowIndex > 0)
			{
				tick++;
				Debug.Write("DiagonalUp");
				newColumnIndex++;
				newRowIndex--;
			}
			while (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == newRowIndex && Grid.GetColumn(e) == newColumnIndex).Text == "X" && newColumnIndex > 0 && newRowIndex < number)
			{
				tick++;
				Debug.Write("DiagonalDown");
				newColumnIndex--;
				newRowIndex++;
			}

			if (tick >= 5)
			{
				Debug.WriteLine("Player 2 win");
				Window game = new WinScreen(false);
				game.Show();
				return true;
			}
			else
			{
				return false;
			}
		}

		public void checkPlayer1Take(int rowIndex, int ColumnIndex)
		{
			//horizontal check right
			if (rowIndex + 3 < number && ColumnIndex < number)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 3 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player1Captures++;
					}
				}

			}

			//horizontal check left
			if (rowIndex - 3 > 0 && ColumnIndex > 0)
			{
				if(Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 3 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player1Captures++;
					}
				}
			}

			//vertical check up
			if (rowIndex < number && ColumnIndex + 3 < number)
			{
				if(Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 3).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 1).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 1).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 2).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 2).Text = "";
						player1Captures++;
					}
				}
			}
			//verticale check down
			if (rowIndex > 0 && ColumnIndex - 3 > 0)
			{
				if(Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 3).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 1).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 1).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 2).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 2).Text = "";
						player1Captures++;
					}
				}
			}
			//diagonal check up
			if (rowIndex + 3 < number && ColumnIndex + 3 < number)
			{
				if(Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 3 && Grid.GetColumn(e) == ColumnIndex + 3).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex + 1).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex + 1).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex + 2).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex + 2).Text = "";
						player1Captures++;
					}
				}
			}
			//diagonal check down
			if (rowIndex - 3 > 0 && ColumnIndex - 3 > 0)
			{ 
				if(Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 3 && Grid.GetColumn(e) == ColumnIndex - 3).Text == "O")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex - 1).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex - 1).Text = "";
						player1Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex - 2).Text == "X")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex - 2).Text = "";
						player1Captures++;
					}
				}
			}
			if(player1Captures >= 5)
			{
				Debug.WriteLine("Player 1 won by captures");
				Window game = new WinScreen(true);
				game.Show();
				this.Close();
			}
		}

		public void checkPlayer2Take(int rowIndex, int ColumnIndex)
		{
			//horizontal check right
			if (rowIndex + 3 < number && ColumnIndex < number)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 3 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player2Captures++;
					}
				}

			}

			//horizontal check left
			if (rowIndex - 3 > 0 && ColumnIndex > 0)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 3 && Grid.GetColumn(e) == ColumnIndex).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex).Text = "";
						player2Captures++;
					}
				}
			}

			//vertical check up
			if (rowIndex < number && ColumnIndex + 3 < number)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 3).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 1).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 1).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 2).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex + 2).Text = "";
						player2Captures++;
					}
				}
			}
			//verticale check down
			if (rowIndex > 0 && ColumnIndex - 3 > 0)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 3).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 1).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 1).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 2).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == ColumnIndex - 2).Text = "";
						player2Captures++;
					}
				}
			}
			//diagonal check up
			if (rowIndex + 3 < number && ColumnIndex + 3 < number)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 3 && Grid.GetColumn(e) == ColumnIndex + 3).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex + 1).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 1 && Grid.GetColumn(e) == ColumnIndex + 1).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex + 2).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex + 2 && Grid.GetColumn(e) == ColumnIndex + 2).Text = "";
						player2Captures++;
					}
				}
			}
			//diagonal check down
			if (rowIndex - 3 > 0 && ColumnIndex - 3 > 0)
			{
				if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 3 && Grid.GetColumn(e) == ColumnIndex - 3).Text == "X")
				{
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex - 1).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 1 && Grid.GetColumn(e) == ColumnIndex - 1).Text = "";
						player2Captures++;
					}
					if (Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex - 2).Text == "O")
					{
						Board.Children.Cast<UIElement>().OfType<TextBlock>().FirstOrDefault(e => Grid.GetRow(e) == rowIndex - 2 && Grid.GetColumn(e) == ColumnIndex - 2).Text = "";
						player2Captures++;
					}
				}
			}
			if (player2Captures >= 5)
			{
				Debug.WriteLine("Player 2 won by captures");
				Window game = new WinScreen(false);
				game.Show();
				this.Close();
			}
		}

		public bool Player1Set(int x, int y)
		{
			if(x <= middlePlace + 3 && x >= middlePlace - 3 && y <= middlePlace + 3 && y >= middlePlace - 3)
			{
				firstPlace = false;
				return true;
			}
			else
			{
				Error.Text = "Player 1 Must Place Piece Close to Center on Turn 1";
				return false;
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			string directoryPath = "C:\\MyFile";
			string fileName = "save.txt";

			try
			{
				
				string filePath = Path.Combine(directoryPath, fileName);

				
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}

				StringBuilder dataBuilder = new StringBuilder();
				dataBuilder.AppendLine($"{isPVPGlobal}, {p1Name}, {p2Name}, {player1turn}, {player1Captures}, {player2Captures}, {number}");

				foreach (TextBlock tb in Board.Children)
				{
					int row = Grid.GetRow(tb);
					int column = Grid.GetColumn(tb);

					dataBuilder.AppendLine($"{row}, {column}, {tb.Text}");
				}

				// Write the string to a file
				File.WriteAllText(filePath, dataBuilder.ToString());
				Console.WriteLine("Data saved successfully.");
				this.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}

		void Timer_Tick(object sender, EventArgs e)
		{
			if (time > 0)
			{
				time--;
				Timer.Text = string.Format("00:0{0}:{1}", time/60, time%60);
			}
			else
			{
				timer.Stop();
				time = 20;
				
				if(player1turn == true) player1turn = false;
				else player1turn = true;


				switch (player1turn)
				{
					case true:
						PlayerTurn.Text = p1Name + "'s Turn";
						break;
					case false:
						PlayerTurn.Text = p2Name + "'s Turn";
						break;
				}

				timer.Start();
			}
		}
	}
}
