﻿using System;
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
using static System.Net.Mime.MediaTypeNames;

namespace Pente
{
    /// <summary>
    /// Interaction logic for PVP.xaml
    /// </summary>
    public partial class PVP : Window
    {
        public string player1name;
        public string player2name;

        public PVP()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonStartPVP_Click(object sender, RoutedEventArgs e)
        {
            if (Player1Name.Text != null || Player1Name.Text != string.Empty) player1name = Player1Name.Text ;
            if (Player1Name.Text == null || Player1Name.Text == string.Empty) player1name = "Player One";
            if (Player2Name.Text != null || Player1Name.Text != string.Empty) player2name = Player2Name.Text;
            if (Player2Name.Text == null || Player1Name.Text == string.Empty) player2name = "Player Two";

            Window game = new MainWindow(true, player1name, player2name);
            game.Show();
        }
    }
}
