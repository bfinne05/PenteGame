using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : Window
    {
        public TitleScreen()
        {
            InitializeComponent();
        }

        private void PVP_Click(object sender, RoutedEventArgs e)
        {
            Window game = new PVP();
            game.Show();
            this.Close();
        }

        private void PVAI_Click(object sender, RoutedEventArgs e)
        {
            Window game = new PVAI();
            game.Show();
            this.Close();
        }

		private void Load_Click(object sender, RoutedEventArgs e)
		{
			string filePath = "C:\\MyFile\\save.txt";
			bool isPVP = false;
			string p1Name = "";
			string p2Name = "";
			bool playerTurn = false;
			int captures1 = 0;
			int captures2 = 0;
			int num = 20;
			List<string> boardData = new List<string>();

			try
			{
				string[] lines = File.ReadAllLines(filePath);

				// Parse and use the loaded data as needed
				string[] gameData = lines[0].Split(',');
				isPVP = bool.Parse(gameData[0].Trim());
				p1Name = gameData[1].Trim();
				p2Name = gameData[2].Trim();
				playerTurn = bool.Parse(gameData[3].Trim());
				captures1 = int.Parse(gameData[4].Trim());
				captures2 = int.Parse(gameData[5].Trim());
				num = int.Parse(gameData[6].Trim());

				// Load board data
				for (int i = 7; i < lines.Length; i++)
				{
					boardData.Add(lines[i]);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

			// Pass all loaded data to the main window constructor
			Window game = new MainWindow(isPVP, p1Name, p2Name, num, playerTurn, captures1, captures2);
			game.Show();
			this.Close();
		}
	}
}
