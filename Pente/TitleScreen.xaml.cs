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
            string p1Name = "";

			try
			{
				// Read the contents of the file into a string
				p1Name = File.ReadAllText(filePath);
				Console.WriteLine("String loaded successfully:");
				Console.WriteLine(p1Name);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
            Window game = new MainWindow(true, p1Name, "Bob", 20);
            game.Show();
            this.Close();
		}
	}
}
