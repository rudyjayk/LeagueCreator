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

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace league_final
{
    /// <summary>
    /// Interaction logic for LeagueWindow.xaml
    /// </summary>
    public partial class LeagueWindow : Window
    {
        //Firebase connection
        IFirebaseClient client;
        public LeagueWindow(IFirebaseClient client)
        {
            InitializeComponent();
            //Assigns firebase connection
             this.client = client;
        }


        private async void Insert_Click(object sender, RoutedEventArgs e)
        {
            var data = new League
            {
                LeagueID = int.Parse(textBox1.Text),
                LeagueName = textBox2.Text,
                SportID = int.Parse(textBox3.Text),
                numberOfTeams = int.Parse(textBox4.Text)
            };

            SetResponse response = await client.SetTaskAsync("League/" + textBox1.Text, data);
            League result = response.ResultAs<League>();

            MessageBox.Show("Data Inserted " + result.LeagueName);
        }

        private async void Retrieve_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("League/" + textBox1.Text);
            League obj = response.ResultAs<League>();

            textBox1.Text = obj.LeagueID.ToString();
            textBox2.Text = obj.LeagueName;
            textBox3.Text = obj.SportID.ToString();
            textBox4.Text = obj.numberOfTeams.ToString();

            MessageBox.Show("Data has been retireved");
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            var data = new League
            {
                LeagueID = int.Parse(textBox1.Text),
                LeagueName = textBox2.Text,
                SportID = int.Parse(textBox3.Text),
                numberOfTeams = int.Parse(textBox4.Text)
            };

            FirebaseResponse response = await client.UpdateTaskAsync("League/" + textBox1.Text, data);
            League result = response.ResultAs<League>();
            MessageBox.Show("Data updated at ID: " + result.LeagueID);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("League/" + textBox1.Text);

            MessageBox.Show("Deleted Element of ID: " + textBox1.Text);

        }

        private async void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("League/");
            MessageBox.Show("All Elements Deleted / League node has been deleted");
        }
    }
}
