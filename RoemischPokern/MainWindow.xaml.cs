using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DLib.Bot.RoemischPokern;

namespace RoemischPokern
{
    public partial class MainWindow : Window
    {
        Game game = new Game();
        Player player;
        bool running;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new Player(game);
            running = true;
            MessageBox.Show("Der Computer MUSS erster der Runde sein!");
            UpdateUI();
            ThreadRoutine();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(textBox.Text, out var b) && b >= 0 && b <= 7)
            {
                if (b <= 7)
                    game.MaxFilledFields = b;
                else
                    MessageBox.Show("Es gibt nicht mehr als 7 Felder");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (game.MaxFilledFields < 7)
            {
                button.IsEnabled = false;
                new Thread(player.PlayARound).Start();
            }
            else
                MessageBox.Show("der computer darf keinen weiteren zug spielen");
        }

        void ThreadRoutine()
        {
            new Thread(() => {
                while (running)
                {
                    if (!player.Playing)
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(UpdateUI));
                    Thread.Sleep(500);
                }
            }).Start();
        }

        void UpdateUI()
        {
            if (!game.Finished)
                button.IsEnabled = true;
            listBox.Items.Clear();
            foreach (string wert in Array.ConvertAll(player.Table, x => x == 0 ? "" : x.ToString()))
                listBox.Items.Add(wert);
            textBox.Text = game.MaxFilledFields.ToString();
            textBox1.Text = player.Points.ToString();
            checkBox.IsChecked = player.Cards[0];
            checkBox1.IsChecked = player.Cards[1];
            checkBox2.IsChecked = player.Cards[2];
            checkBox3.IsChecked = player.Cards[3];
            checkBox4.IsChecked = player.Cards[4];
            checkBox5.IsChecked = player.Cards[5];
        }
    }
}
