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
    /// Interaction logic for AddUpdate_Teams.xaml
    /// </summary>
    public partial class AddUpdate_Teams : Window
    {
        IFirebaseClient client; //Database connection
        string Action; //Action being performed
        string Node; //Node action is being performed on
        bool LeagueIDLabel; //Whether league is to be retrieved or not
        bool SportIDLabel; //Whether SportID is to be retrieved or not
        bool TeamNameLabel; //Whether Team Name is to be retrieved or not
        bool numCoachesLabel; //Whether Number of coaches is to be retrieved or not
        bool numExtrasLabel; //Whether Number of extras is to be retrieved or not
        bool numPlayersLabel; //Whether Number of players is to be retrieved or not
        public AddUpdate_Teams(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            this.client = client; //Sets client
            this.Action = Action; //Sets action
            this.Node = Node; //Sets Node

            if (Action == "Add")
            {
                //Disables ID textbox and all Labels
                //Enables all other textboxes
                this.txt1.IsEnabled = false;
                this.ID.IsEnabled = false;
                this.LeagueID.IsEnabled = false;
                this.SportID.IsEnabled = false;
                this.TeamName.IsEnabled = false;
                this.numCoaches.IsEnabled = false;
                this.numExtras.IsEnabled = false;
                this.numPlayers.IsEnabled = false;
            }
            else if (Action == "Update")
            {
                //Disables all Labels except ID
                //Disables all textboxes except ID
                this.LeagueID.IsEnabled = false;
                this.SportID.IsEnabled = false;
                this.TeamName.IsEnabled = false;
                this.numCoaches.IsEnabled = false;
                this.numExtras.IsEnabled = false;
                this.numPlayers.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
                this.txt5.IsEnabled = false;
                this.txt6.IsEnabled = false;
                this.txt7.IsEnabled = false;
            }
            else if (Action == "Retrieve")
            {
                //Disables all textboxes
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
                this.txt5.IsEnabled = false;
                this.txt6.IsEnabled = false;
                this.txt7.IsEnabled = false;
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get counter node
            Counter get = resp.ResultAs<Counter>(); //Sets counter object with response
            object data = null;
            object obj = null;

            if (Action == "Add")
            {
                try
                {
                    //Set sport team object with user input 
                    data = new SportTeams
                    {
                        TeamID = Convert.ToInt32(get.cnt) + 1,
                        LeagueID = int.Parse(txt2.Text),
                        SportID = int.Parse(txt3.Text),
                        TeamName = txt4.Text,
                        numberOfCoaches = int.Parse(txt5.Text),
                        numberOfExtras = int.Parse(txt6.Text),
                        numberOfPlayers = int.Parse(txt7.Text)
                    };
                    //Create new counter object and increasing count by 1
                    obj = new Counter
                    {
                        cnt = Convert.ToInt32(get.cnt) + 1
                    };
                }
                catch (Exception ex) //Catch exception when input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                //Clears all textboxes 
                this.txt1.Text = "";
                this.txt2.Text = "";
                this.txt3.Text = "";
                this.txt4.Text = "";
                this.txt5.Text = "";
                this.txt6.Text = "";
                this.txt7.Text = "";

                string index = (get.cnt + 1).ToString();
                SetResponse response = await client.SetTaskAsync(Node + "/" + index, data); //Adds data to database 
                SetResponse response1 = await client.SetTaskAsync("Counter/" + Node, obj); //Updates count in database


                MessageBox.Show("Data Inserted");
            }
            else if (Action == "Update")
            {
                try
                {
                    //Create new sport team object with data inserted by user
                    data = new SportTeams
                    {
                        TeamID = int.Parse(txt1.Text),
                        LeagueID = int.Parse(txt2.Text),
                        SportID = int.Parse(txt3.Text),
                        TeamName = txt4.Text,
                        numberOfCoaches = int.Parse(txt5.Text),
                        numberOfExtras = int.Parse(txt6.Text),
                        numberOfPlayers = int.Parse(txt7.Text)
                    };
                }
                catch (Exception ex) //Catch exception when input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                FirebaseResponse response = await client.UpdateTaskAsync(Node + "/" + txt1.Text, data); //Update Node on database

                MessageBox.Show("Data Updated");
            }
            else if (Action == "Retrieve")
            {
                string print = null;
                if (txt1.IsEnabled)
                {
                    FirebaseResponse response = await client.GetTaskAsync("Counter/" + Node); //Get counter object
                    Counter cnt = response.ResultAs<Counter>(); //Set counter object to response

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
                    catch (Exception ex) //Catches exception when input data is incorrect
                    {
                        MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                        return;
                    }

                    response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Gets node information from database

                    //Turns response into sport object and get string 
                    SportTeams result = response.ResultAs<SportTeams>();
                    print = result.printAll();

                    MessageBox.Show(print);
                }
                else
                {
                    List<string> elements = new List<string>(); //Holds lsit of strings which are information of selected nodes
                    //Iterates through each instance on node
                    for (int i = 1; i <= get.cnt; i++)
                    {
                        FirebaseResponse result = await client.GetTaskAsync(Node + "/" + i); //Gets node information

                        try
                        {
                            //Sets resulst to a sport team object
                            SportTeams team = result.ResultAs<SportTeams>();
                            //Bool if addElement is true then 
                            bool addElement = true;

                            //If cooresponding label is enabled
                            if (LeagueIDLabel)
                            {
                                if (txt2.Text != "")// Checkcs if cooresponding textbox is enabled
                                {
                                    if (team.LeagueID != int.Parse(txt2.Text))// Checks if the text and information in the data is the same
                                    {
                                        addElement = false; //If not then turn addElement to false
                                    }
                                }
                            }
                            if (SportIDLabel)
                            {
                                if (txt3.Text != "")
                                {
                                    if (team.SportID != int.Parse(txt3.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (TeamNameLabel)
                            {
                                if (txt4.Text != "")
                                {
                                    if (team.TeamName != txt4.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (numCoachesLabel)
                            {
                                if (txt5.Text != "")
                                {
                                    if (team.numberOfCoaches != int.Parse(txt5.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (numExtrasLabel)
                            {
                                if (txt6.Text != "")
                                {
                                    if (team.numberOfExtras != int.Parse(txt6.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (numPlayersLabel)
                            {
                                if (txt7.Text != "")
                                {
                                    if (team.numberOfPlayers != int.Parse(txt7.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }

                            if (addElement) 
                            {
                                //Adds element to list and calls print function
                                elements.Add(team.printRestirction(numCoachesLabel, LeagueIDLabel, SportIDLabel, TeamNameLabel, numExtrasLabel, numPlayersLabel));
                            }
                        }
                        catch (Exception ex) //Catch exception of input data incorrect
                        {
                            MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                            return;
                        }
                    }

                    if (elements.Count == 0) //If list is empty then let user know
                    {
                        MessageBox.Show("There are no elements with the inputs inserted!");
                    }

                    foreach (string ele in elements) //Prints all information 
                    {
                        MessageBox.Show(ele);
                    }

                }
            }
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

        //Function when label is enabled and allowed for user interaction
        //For Update: Label ID is used to grab data of certain key 
        //For Retrieve: Select labels that you want to be returned to you
        private async void Select(object sender, MouseButtonEventArgs e)
        {
            if (Action == "Retrieve")
            {
                Label label = sender as Label; //Get selected label 

                //Switches between normal and bold font weight
                if (label.FontWeight == FontWeights.Normal)
                {
                    label.FontWeight = FontWeights.Bold;

                    switch (label.Name)
                    {
                        //Enables ID textbox
                        //Disables all textboxes and labels 
                        //Sets all other labels to normal font weight
                        case "ID":
                            txt1.IsEnabled = true;
                            LeagueIDLabel = false;
                            LeagueID.FontWeight = FontWeights.Normal;
                            LeagueID.IsEnabled = LeagueIDLabel;
                            txt2.IsEnabled = false;
                            SportIDLabel = false;
                            SportID.FontWeight = FontWeights.Normal;
                            SportID.IsEnabled = SportIDLabel;
                            txt3.IsEnabled = false;
                            TeamNameLabel = false;
                            TeamName.FontWeight = FontWeights.Normal;
                            TeamName.IsEnabled = TeamNameLabel;
                            txt4.IsEnabled = false;
                            numCoachesLabel = false;
                            numCoaches.FontWeight = FontWeights.Normal;
                            numCoaches.IsEnabled = numCoachesLabel;
                            txt5.IsEnabled = false;
                            numExtrasLabel = false;
                            numExtras.FontWeight = FontWeights.Normal;
                            numExtras.IsEnabled = numExtrasLabel;
                            txt6.IsEnabled = false;
                            numPlayersLabel = false;
                            numPlayers.FontWeight = FontWeights.Normal;
                            numPlayers.IsEnabled = numPlayersLabel;
                            txt7.IsEnabled = false;
                            break;
                        //Enables cooresponding label and textbox
                        case "LeagueID":
                            LeagueIDLabel = true;
                            txt2.IsEnabled = true;
                            break;
                        case "SportID":
                            SportIDLabel = true;
                            txt3.IsEnabled = true;
                            break;
                        case "TeamName":
                            TeamNameLabel = true;
                            txt4.IsEnabled = true;
                            break;
                        case "numCoaches":
                            numCoachesLabel = true;
                            txt5.IsEnabled = true;
                            break;
                        case "numExtras":
                            numExtrasLabel = true;
                            txt6.IsEnabled = true;
                            break;
                        case "numPlayers":
                            numPlayersLabel = true;
                            txt7.IsEnabled = true;
                            break;
                    }
                }
                else
                {
                    label.FontWeight = FontWeights.Normal;

                    switch (label.Name)
                    {
                        //Disables ID textbox and label
                        //Enables all other labels
                        case "ID":
                            txt1.IsEnabled = false;
                            LeagueIDLabel = false;
                            LeagueID.IsEnabled = true;
                            SportIDLabel = false;
                            SportID.IsEnabled = true;
                            TeamNameLabel = false;
                            TeamName.IsEnabled = true;
                            numCoachesLabel = false;
                            numCoaches.IsEnabled = true;
                            numExtrasLabel = false;
                            numExtras.IsEnabled = true;
                            numPlayersLabel = false;
                            numPlayers.IsEnabled = true;
                            break;
                        //Disables cooresponding label and textbox
                        case "LeagueID":
                            LeagueIDLabel = false;
                            txt2.IsEnabled = false;
                            break;
                        case "SportID":
                            SportIDLabel = false;
                            txt3.IsEnabled = false;
                            break;
                        case "TeamName":
                            TeamNameLabel = false;
                            txt4.IsEnabled = false;
                            break;
                        case "numCoaches":
                            numCoachesLabel = false;
                            txt5.IsEnabled = false;
                            break;
                        case "numExtras":
                            numExtrasLabel = false;
                            txt6.IsEnabled = false;
                            break;
                        case "numPlayers":
                            numPlayersLabel = false;
                            txt7.IsEnabled = false;
                            break;
                    }
                    
                }
            }
            else if (Action == "Update")
            {
                
                if (txt1.IsEnabled == false)
                {
                    //Enables ID textbox
                    //Disables all other textbox
                    //Clears all other textbox
                    txt1.IsEnabled = true;
                    this.txt2.IsEnabled = false;
                    this.txt3.IsEnabled = false;
                    this.txt4.IsEnabled = false;
                    this.txt5.IsEnabled = false;
                    this.txt6.IsEnabled = false;
                    this.txt7.IsEnabled = false;
                    this.txt2.Text = "";
                    this.txt3.Text = "";
                    this.txt4.Text = "";
                    this.txt5.Text = "";
                    this.txt6.Text = "";
                    this.txt7.Text = "";
                    return;
                }

                FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //retrieve counter information
                Counter cnt = resp.ResultAs<Counter>(); //Set counter object

                try
                {
                    //Check if ID entered was valid

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
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                //Disable ID textbox
                //Enable all other textboxes
                resp = await client.GetTaskAsync(Node + "/" + txt1.Text);
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = true;
                this.txt3.IsEnabled = true;
                this.txt4.IsEnabled = true;
                this.txt5.IsEnabled = true;
                this.txt6.IsEnabled = true;
                this.txt7.IsEnabled = true;

                //Fill textboxes with information from data
                SportTeams result = resp.ResultAs<SportTeams>();
                txt2.Text = result.LeagueID.ToString();
                txt3.Text = result.SportID.ToString();
                txt4.Text = result.TeamName;
                txt5.Text = result.numberOfCoaches.ToString();
                txt6.Text = result.numberOfExtras.ToString();
                txt7.Text = result.numberOfPlayers.ToString();
            }
        }

    }
}
