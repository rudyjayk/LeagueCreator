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
    /// Interaction logic for Delete.xaml
    /// </summary>
    public partial class Delete : Window
    {
        IFirebaseClient client; //Database connection
        string Action; //Action being performed
        string Node; //Node action is being performed on
        public Delete(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            this.client = client; //Set clinet
            this.Action = Action; //Set action
            this.Node = Node; //Set node
        }


        //Function that calls when delete button is clicked
        //Adjust depending on node that was selected
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {

            int startID = 0; 
            

            FirebaseResponse response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Get instance from databse

            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get amount of instances for a certain node
            Counter cnt = resp.ResultAs<Counter>(); //Set counter object to the response

            try
            {
                //Check if ID entered is valid

                int count = cnt.cnt;

                int ID = int.Parse(txt1.Text);
                if (ID > count || ID < 1)
                {
                    MessageBox.Show(txt1.Text + " is not a valid ID");
                    return;
                }
            }
            catch (Exception ex) //Catch exception when input data is incorrect
            {
                MessageBox.Show(ex.Message + ", Check what you inserted in the Textbox");   
                return;
            }

            //Depending on node that is selected
            //Create new instance and set starting ID to the instance ID
            if (Node == "Coaches")
            {
                Coaches data = response.ResultAs<Coaches>();
                startID = data.CoachId;
            }
            else if (Node == "Extras")
            {
                Extras data = response.ResultAs<Extras>();
                startID = data.ExtraID;
            }
            else if (Node == "League")
            {
                League data = response.ResultAs<League>();
                startID = data.LeagueID;
            }
            else if (Node == "Sports")
            {
                Sport data = response.ResultAs<Sport>();
                startID = data.SportID;
            }
            else if (Node == "Sport Teams")
            {
                SportTeams data = response.ResultAs<SportTeams>();
                startID = data.SportID;
            }
            else if (Node == "Players")
            {
                Players data = response.ResultAs<Players>();
                startID = data.PlayerID;
            }
            
            
            
            
            
            //In order to keep unique IDs and count we had to overwrite data
            //This for loop goes througth all instances from the startId to the last instance in database
            //It overwrites the startId instance with the next instance in the database and so on
            //While overwriting it is changing the unique ID 
            //If it is on the last instance then it deletes that instance
            for (int i = startID; i <= cnt.cnt; i++)
            {
                if (i == cnt.cnt)
                {
                    FirebaseResponse delete = await client.DeleteTaskAsync(Node + "/" + cnt.cnt);
                    cnt.cnt -= 1;
                    resp = await client.UpdateTaskAsync("Counter/" + Node, cnt);
                    break;
                }

                FirebaseResponse overwrite = await client.GetTaskAsync(Node + "/" + (i + 1));

                if (Node == "Coaches")
                {
                    Coaches data = overwrite.ResultAs<Coaches>();
                    data.CoachId = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }
                else if (Node == "Extras")
                {
                    Extras data = overwrite.ResultAs<Extras>();
                    data.ExtraID = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }
                else if (Node == "League")
                {
                    League data = overwrite.ResultAs<League>();
                    data.LeagueID = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }
                else if (Node == "Sports")
                {
                    Sport data = overwrite.ResultAs<Sport>();
                    data.SportID = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }
                else if (Node == "Sport Teams")
                {
                    SportTeams data = overwrite.ResultAs<SportTeams>();
                    data.TeamID = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }
                else if (Node == "Players")
                {
                    Players data = overwrite.ResultAs<Players>();
                    data.PlayerID = i;
                    FirebaseResponse updateIndex = await client.UpdateTaskAsync(Node + "/" + i, data);
                }

            }

            MessageBox.Show("Deleted Element of ID: " + txt1.Text);

        }

        //Function that is performed when button is clicked
        //Adjusts depending on action to be performed and node being performed on
        private void btn_Load(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            btn.Content = Action;
        }

        //Function for button when window is loaded
        //Sets content of button to cooresponding action that is being performed
        private void nodeLabel_load(object sender, RoutedEventArgs e)
        {
            Label node = sender as Label;

            node.Content = Node;
        }

        //Function for node label when window is loaded
        //Sets content of label to cooresponding node that action is being performed on
        private void Home_click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();

            this.Close();

            window.Show();
        }
    }
}
