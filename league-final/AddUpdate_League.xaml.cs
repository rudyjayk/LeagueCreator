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
    /// Interaction logic for AddUpdate_League.xaml
    /// </summary>
    public partial class AddUpdate_League : Window
    {
        IFirebaseClient client; //Firebase client
        string Action; //Action being perforemd
        string Node; //Node action is being performed on
        bool NameLabel; //Whether Name is being retrieved or not
        bool SportIDLabel; //Whether sportID is being retrieved or not
        bool NumTeamLabel; //Wehter number of teams are being retrieved or not

        //Initialization of window
        //Based on action that is being performed on the databse the window will adjust what is available to access
        public AddUpdate_League(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            //Initializes all variables to link to database
            this.client = client;
            this.Action = Action;
            this.Node = Node;

            //Enable all textboxes but ID textbox
            //Diables all labels
            if (Action == "Add")
            {
                this.txt1.IsEnabled = false;
                this.ID.IsEnabled = false;
                this.LeagueName.IsEnabled = false;
                this.SportID.IsEnabled = false;
                this.NumTeams.IsEnabled = false;
            }
            //Disables all labels except ID label
            //Disables all textboxes except ID textbox
            else if (Action == "Update")
            {
                this.LeagueName.IsEnabled = false;
                this.SportID.IsEnabled = false;
                this.NumTeams.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
            }
            //Disables all textboxes and enable all labels
            else if (Action == "Retrieve")
            {
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
            }
        }

        //Function that is performed when button is clicked
        //Adjusts depending on action to be performed and node being performed on
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); // Gets counter data
            Counter get = resp.ResultAs<Counter>(); //Counter data
            object data = null; //Data added to databse
            object obj = null; //Update counter data

            if (Action == "Add")
            {
                try
                {
                    //Sets data to new League object with user inputted data
                    data = new League
                    {
                        LeagueID = Convert.ToInt32(get.cnt) + 1,
                        LeagueName = txt2.Text,
                        SportID = int.Parse(txt3.Text),
                        numberOfTeams = int.Parse(txt4.Text)
                    };

                    //Sets obj to new counter object and increase count by 1
                    obj = new Counter
                    {
                        cnt = Convert.ToInt32(get.cnt) + 1
                    };
                }
                catch (Exception ex) //Catches exception when input data is wrong
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }


                string index = (get.cnt + 1).ToString(); //change count from int to string
                SetResponse response = await client.SetTaskAsync(Node + "/" + index, data); //Adds data to databse
                SetResponse response1 = await client.SetTaskAsync("Counter/" + Node, obj); //Updates counter object

                //Clears all textboxes
                this.txt1.Text = "";
                this.txt2.Text = "";
                this.txt3.Text = "";
                this.txt4.Text = "";

                MessageBox.Show("Data Inserted");
            }
            else if (Action == "Update")
            {
                try
                {
                    //Creates new league object with user inputted data
                    data = new League
                    {
                        LeagueID = int.Parse(txt1.Text),
                        LeagueName = txt2.Text,
                        SportID = int.Parse(txt3.Text),
                        numberOfTeams = int.Parse(txt4.Text)
                    };
                }
                catch (Exception ex) //Catches exception when input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                FirebaseResponse response = await client.UpdateTaskAsync(Node + "/" + txt1.Text, data); //Updates data

                MessageBox.Show("Data Updated");
            }
            else if (Action == "Retrieve")
            {
                string print = null; //String that holds information for retrieve data
                if (txt1.IsEnabled)
                {
                    FirebaseResponse response = await client.GetTaskAsync("Counter/" + Node); //Gets counter information
                    Counter cnt = response.ResultAs<Counter>(); //Turns response into counter object

                    try //Checks if ID inserted is a valid ID
                    {
                        int count = cnt.cnt; 

                        int ID = int.Parse(txt1.Text);

                        if (ID > count || ID < 1)
                        {
                            MessageBox.Show(txt1.Text + " is not a valid ID");
                            return;
                        }
                    }
                    catch (Exception ex) //Catches if input data is incorrect
                    {
                        MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                        return;
                    }

                    response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Gets ID inserted data

                    League result = response.ResultAs<League>(); //Turn resposne into league object

                    print = result.printAll(); //Print funtion that prints the league objects member variables

                    MessageBox.Show(print); //Show retrieve data to user
                }
                else
                {
                    List<string> elements = new List<string>(); //Holds all retrieve data information in a list of strings
                    for (int i = 1; i <= get.cnt; i++)
                    {

                        FirebaseResponse result = await client.GetTaskAsync(Node + "/" + i); //Gets data from database

                        try
                        {
                            League league = result.ResultAs<League>(); //Turn response inrto league object
                            bool addElement = true; //Checks whether to add element to list

                            if (NameLabel) //If label is activated
                            {
                                if (txt2.Text != "")//Checks cooresponding checkbox to see if user inputted text
                                {
                                    if (league.LeagueName != txt2.Text)//Checks if user input is equal to cooresponding variable in database
                                    {
                                        addElement = false; //Changes to false because the element was not equal to what the user was looking for
                                    }
                                }
                            }
                            if (SportIDLabel)
                            {
                                if (txt3.Text != "")
                                {
                                    if (league.SportID != int.Parse(txt3.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (NumTeamLabel)
                            {
                                if (txt4.Text != "")
                                {
                                    if (league.numberOfTeams != int.Parse(txt4.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }

                            if (addElement) //Adds string to list that will be printed out by messageboxes
                            {
                                elements.Add(league.printRestirction(SportIDLabel, NameLabel, NumTeamLabel));
                            }
                        }
                        catch (Exception ex) //Catch exception if user input data is incorrect
                        {
                            MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                            return;
                        }
                    }

                    if (elements.Count == 0) //If list is empty then no elements in databse had what the user was looking for
                    {
                        MessageBox.Show("There are no elements with the inputs inserted!");
                    }

                    foreach (string ele in elements)
                    {
                        MessageBox.Show(ele);
                    }
                }
            }
        }

        //Function for button when window is loaded
        //Sets content of button to cooresponding action that is being performed
        private void btn_Load(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            btn.Content = Action;
        }

        //Function for node label when window is loaded
        //Sets content of label to cooresponding node that action is being performed on
        private void nodeLabel_load(object sender, RoutedEventArgs e)
        {
            Label node = sender as Label;

            node.Content = Node;
        }

        //Function for when home button is clicked
        //Opens main window and closes current window
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
                Label label = sender as Label; //Get label that was selected

                //Switches between normal and bold font weight
                if (label.FontWeight == FontWeights.Normal) 
                {
                    label.FontWeight = FontWeights.Bold;

                    switch (label.Name)
                    {
                        //Enable only ID textbox and label
                        //Disable all other textboxes and labels and set font weight of all labels to normal
                        case "ID":
                            txt1.IsEnabled = true;
                            NameLabel = false;
                            LeagueName.FontWeight = FontWeights.Normal;
                            LeagueName.IsEnabled = NameLabel;
                            txt2.IsEnabled = false;
                            SportIDLabel = false;
                            SportID.FontWeight = FontWeights.Normal;
                            SportID.IsEnabled = SportIDLabel;
                            txt3.IsEnabled = false;
                            NumTeamLabel = false;
                            NumTeams.FontWeight = FontWeights.Normal;
                            NumTeams.IsEnabled = false;
                            txt4.IsEnabled = false;
                            break;
                        //Enable cooresponding textbox
                        case "LeagueName":
                            NameLabel = true;
                            txt2.IsEnabled = true;
                            break;
                        case "SportID":
                            SportIDLabel = true;
                            txt3.IsEnabled = true;
                            break;
                        case "NumTeams":
                            NumTeamLabel = true;
                            txt4.IsEnabled = true;
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
                            NameLabel = false;
                            LeagueName.IsEnabled = true;
                            SportIDLabel = false;
                            SportID.IsEnabled = true;
                            NumTeamLabel = false;
                            NumTeams.IsEnabled = true;
                            break;
                        //Disables cooresponding textbox
                        case "LeagueName":
                            NameLabel = false;
                            txt2.IsEnabled = false;
                            break;
                        case "SportID":
                            SportIDLabel = false;
                            txt3.IsEnabled = false;
                            break;
                        case "NumTeams":
                            NumTeamLabel = false;
                            txt4.IsEnabled = false;
                            break;
                    }
                }
            }
            else if (Action == "Update")
            {
                //Enables ID textbox
                //Disables all other textboxes 
                if (txt1.IsEnabled == false)
                {
                    txt1.IsEnabled = true;
                    this.txt2.IsEnabled = false;
                    this.txt3.IsEnabled = false;
                    this.txt4.IsEnabled = false;
                    this.txt2.Text = "";
                    this.txt3.Text = "";
                    this.txt4.Text = "";
                    return;
                }

                FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get count information from databse
                Counter cnt = resp.ResultAs<Counter>(); //Set response to counter object

                try
                {
                    //Checksif valid ID was entered

                    int count = cnt.cnt;

                    int ID = int.Parse(txt1.Text);

                    if (ID > count || ID < 1)
                    {
                        MessageBox.Show(txt1.Text + " is not a valid ID");
                        return;
                    }
                }
                catch (Exception ex) //Catches exception on input data error 
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                //Gains information to cooresponding ID entered
                resp = await client.GetTaskAsync(Node + "/" + txt1.Text);
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = true;
                this.txt3.IsEnabled = true;
                this.txt4.IsEnabled = true;

                //Creates League object and pulls data from league object to be displayed into textboxes
                League result = resp.ResultAs<League>();
                this.txt2.Text = result.LeagueName;
                this.txt3.Text = result.SportID.ToString();
                this.txt4.Text = result.numberOfTeams.ToString();

            }
        }
    }
}
