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
using System.Windows.Navigation;
using System.Windows.Shapes;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace league_final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Connecting to Firebase realtime database
        IFirebaseConfig config = new FirebaseConfig
        {
            //These credentials are copy and pasted from our firebase project
            AuthSecret = "zHobAS1dvslLEaaYnZc0IHML913umLyhGIe7sLtl",
            BasePath = "https://league-up-96c08.firebaseio.com/"
        };

        //Firebase client variable used to gain access and ability to update the database
        IFirebaseClient client;
        string Action;
        string Node;


        public MainWindow()
        {
            InitializeComponent();
            //Assign client to firebase database
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                //Shows if connection was successful
                MessageBox.Show("Connection Successful");
            }
        }


        //Loading function for the Combo selection box
        //User selects a node to update
        private void comboBox_load(Object sender, RoutedEventArgs e)
        {
            //Creates a list 
            List<string> data = new List<string>();
            data.Add("Coaches");
            data.Add("Extras");
            data.Add("League");
            data.Add("Players");
            data.Add("Sports");
            data.Add("Sport Teams");

            var comboBox = sender as ComboBox;

            //Assigns list to combobox 
            comboBox.ItemsSource = data;

            //comboBox.SelectedIndex = 0;
            
        }


        private void ActionBox_load(Object sender, RoutedEventArgs e)
        {
            //Creates a list 
            List<string> data = new List<string>();
            data.Add("Add");
            data.Add("Update");
            data.Add("Delete");
            data.Add("Retrieve");

            var comboBox = sender as ComboBox;

            //Assigns list to combobox 
            comboBox.ItemsSource = data;

            //comboBox.SelectedIndex = 0;

        }

        //Function for when a new selection is picked for the combobox
        //User selects an option and opens another window based on the option they selected
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string value = comboBox.SelectedItem as string;

            Node = value;
        }

        //Function for when a new selection is picked for the combobox
        //User selects an option and opens another window based on the option they selected
        private void ActionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string value = comboBox.SelectedItem as string;

            Action = value;

            
        }

        //Function when button is clicked after user selects an action and node
        //Reacts according to node and action selected
        //Opens up another window when clicked
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (Action == "Add" || Action == "Update" || Action == "Retrieve")
            {
                if (Node == "Coaches" || Node == "Extras")
                {
                    AddUpdate_CoachExtra obj = new AddUpdate_CoachExtra(client, Action, Node);

                    this.Close();

                    obj.Show();
                }
                else if (Node == "Players")
                {
                    AppUpdate_Players obj = new AppUpdate_Players(client, Action, Node);

                    this.Close();

                    obj.Show();
                }
                else if (Node == "Sports")
                {
                    AddUpdate_Sports obj = new AddUpdate_Sports(client, Action, Node);

                    this.Close();

                    obj.Show();
                }
                else if (Node == "Sport Teams")
                {
                    AddUpdate_Teams obj = new AddUpdate_Teams(client, Action, Node);

                    this.Close();

                    obj.Show();
                }
                else if (Node == "League")
                {
                    AddUpdate_League obj = new AddUpdate_League(client, Action, Node);

                    this.Close();

                    obj.Show();
                }


            }
            else if (Action == "Delete")
            {
                Delete obj = new Delete(client, Action, Node);

                this.Close();

                obj.Show();
            }
        }

    }
}
