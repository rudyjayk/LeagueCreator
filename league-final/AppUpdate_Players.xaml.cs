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
    /// Interaction logic for AppUpdate_Players.xaml
    /// </summary>
    public partial class AppUpdate_Players : Window
    {
        IFirebaseClient client; //Databse client
        string Action; //Action being performed
        string Node; //Node action being performed on
        bool AgeLabel; //Whether age is to be retrieved or not
        bool FNameLabel; //Whether First name is to be retrieved or not
        bool LNameLabel; //Whether Last name is to be retrieved or not
        bool JerseyNumLabel; //Whether Jersey num is to be retrieved or not
        bool PositionLabel; //Whether Position is to be retrieved or not
        bool TeamIDLabel; //Whether Team ID is to be retrieved or not
        bool HeightLabel; //Whether Height is to be retrieved or not
        bool WeightLabel; //Whether Weight is to be retrieved or not
        public AppUpdate_Players(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            this.client = client; //Sets client
            this.Action = Action; //Sets action
            this.Node = Node; //Sets node

            if (Action == "Add")
            {
                //Disables ID textbox and all labels
                this.txt1.IsEnabled = false;
                this.ID.IsEnabled = false;
                this.Age.IsEnabled = false;
                this.FName.IsEnabled = false;
                this.LName.IsEnabled = false;
                this.JerseyNum.IsEnabled = false;
                this.Position.IsEnabled = false;
                this.TeamID.IsEnabled = false;
                this.Height.IsEnabled = false;
                this.Weight.IsEnabled = false;
            }
            else if (Action == "Update")
            {
                //Enables ID textbox and label
                //Disable all other textboxes and labels
                this.Age.IsEnabled = false;
                this.FName.IsEnabled = false;
                this.LName.IsEnabled = false;
                this.JerseyNum.IsEnabled = false;
                this.Position.IsEnabled = false;
                this.TeamID.IsEnabled = false;
                this.Height.IsEnabled = false;
                this.Weight.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
                this.txt5.IsEnabled = false;
                this.txt6.IsEnabled = false;
                this.txt7.IsEnabled = false;
                this.txt8.IsEnabled = false;
                this.txt9.IsEnabled = false;
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
                this.txt8.IsEnabled = false;
                this.txt9.IsEnabled = false;
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get counter data from databse
            Counter get = resp.ResultAs<Counter>(); //Set response to a counter object
            object data = null;
            object obj = null;

            if (Action == "Add")
            {
                try
                {
                    //Create a new Player object with user inputted data
                    data = new Players
                    {
                        PlayerID = Convert.ToInt32(get.cnt) + 1,
                        Age = int.Parse(txt2.Text),
                        FName = txt3.Text,
                        LName = txt4.Text,
                        jersey_no = int.Parse(txt5.Text),
                        Position = txt6.Text,
                        TeamID = int.Parse(txt7.Text),
                        Height = txt8.Text,
                        Weight = int.Parse(txt9.Text)
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

                string index = (get.cnt + 1).ToString();
                SetResponse response = await client.SetTaskAsync(Node + "/" + index, data); //Add data to databse
                SetResponse response1 = await client.SetTaskAsync("Counter/" + Node, obj); //Update counter information

                //Clear all textboxes
                this.txt1.Text = "";
                this.txt2.Text = "";
                this.txt3.Text = "";
                this.txt4.Text = "";
                this.txt5.Text = "";
                this.txt6.Text = "";
                this.txt7.Text = "";
                this.txt8.Text = "";
                this.txt9.Text = "";


                MessageBox.Show("Data Inserted");
            }
            else if (Action == "Update")
            {
                try
                {
                    //Create new player object with user inputted data
                    data = new Players
                    {
                        PlayerID = int.Parse(txt1.Text),
                        Age = int.Parse(txt2.Text),
                        FName = txt3.Text,
                        LName = txt4.Text,
                        jersey_no = int.Parse(txt5.Text),
                        Position = txt6.Text,
                        TeamID = int.Parse(txt7.Text),
                        Height = txt8.Text,
                        Weight = int.Parse(txt9.Text)
                    };
                }
                catch (Exception ex) //Catch exception whenn input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                FirebaseResponse response = await client.UpdateTaskAsync(Node + "/" + txt1.Text, data); //Update information on database

                MessageBox.Show("Data Updated");
            }
            else if (Action == "Retrieve")
            {
                string print = null;
                if (txt1.IsEnabled)
                {
                    FirebaseResponse response = await client.GetTaskAsync("Counter/" + Node); //Get counter information
                    Counter cnt = response.ResultAs<Counter>(); //Set count object to response

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
                        MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                        return;
                    }

                    response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Get data from databse

                    Players result = response.ResultAs<Players>(); //Set response to player object
                    print = result.printAll(); //Get information as a string

                    MessageBox.Show(print); //Show information to user

                }
                else
                {
                    List<string> elements = new List<string>(); //List of strings that hold information on nodes that are selected
                    //Iterate through all instances on node
                    for (int i = 1; i <= get.cnt; i++)
                    {
                        FirebaseResponse result = await client.GetTaskAsync(Node + "/" + i); //Get data from database

                        try
                        {
                            Players player = result.ResultAs<Players>(); //Set response to player object
                            //Whether response should be added to list
                            bool addElement = true;

                            if (AgeLabel) //If cooresponding label is enabled
                            {
                                if (txt2.Text != "") //If textbox is empty
                                {
                                    if (player.Age != int.Parse(txt2.Text)) //Check if user input is equal to response data
                                    {
                                        addElement = false; //Put addElement to false
                                    }
                                }
                            }
                            if (FNameLabel)
                            {
                                if (txt3.Text != "")
                                {
                                    if (player.FName != txt3.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (LNameLabel)
                            {
                                if (txt4.Text != "")
                                {
                                    if (player.LName != txt4.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (JerseyNumLabel)
                            {
                                if (txt5.Text != "")
                                {
                                    if (player.jersey_no != int.Parse(txt5.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (PositionLabel)
                            {
                                if (txt6.Text != "")
                                {
                                    if (player.Position != txt6.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (TeamIDLabel)
                            {
                                if (txt7.Text != "")
                                {
                                    if (player.TeamID != int.Parse(txt7.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (HeightLabel)
                            {
                                if (txt8.Text != "")
                                {
                                    if (player.Height != txt8.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }
                            if (WeightLabel)
                            {
                                if (txt9.Text != "")
                                {
                                    if (player.Weight != int.Parse(txt9.Text))
                                    {
                                        addElement = false;
                                    }
                                }
                            }

                            //Calls print function and adds to list of string
                            if (addElement)
                            {
                                elements.Add(player.printRestirction(HeightLabel, FNameLabel, LNameLabel, AgeLabel, WeightLabel, JerseyNumLabel, PositionLabel, TeamIDLabel));
                            }
                        }
                        catch (Exception ex) //Catch exception when input data is incorrect
                        {
                            MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                            return;
                        }

                    }

                    if (elements.Count == 0) //If list is empty then inform user
                    {
                        MessageBox.Show("There are no elements with the inputs inserted!");
                    }

                    foreach (string ele in elements) //Prints information that is selected
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
                        //Disables all other texboxes and labels
                        //Sets all other labels to normal font weight
                        case "ID":
                            txt1.IsEnabled = true;
                            AgeLabel = false;
                            Age.FontWeight = FontWeights.Normal;
                            Age.IsEnabled = AgeLabel;
                            txt2.IsEnabled = false;
                            FNameLabel = false;
                            FName.FontWeight = FontWeights.Normal;
                            FName.IsEnabled = FNameLabel;
                            txt3.IsEnabled = false;
                            LNameLabel = false;
                            LName.FontWeight = FontWeights.Normal;
                            LName.IsEnabled = LNameLabel;
                            txt4.IsEnabled = false;
                            JerseyNumLabel = false;
                            JerseyNum.FontWeight = FontWeights.Normal;
                            JerseyNum.IsEnabled = JerseyNumLabel;
                            txt5.IsEnabled = false;
                            PositionLabel = false;
                            Position.FontWeight = FontWeights.Normal;
                            Position.IsEnabled = PositionLabel;
                            txt6.IsEnabled = false;
                            TeamIDLabel = false;
                            TeamID.FontWeight = FontWeights.Normal;
                            TeamID.IsEnabled = TeamIDLabel;
                            txt7.IsEnabled = false;
                            HeightLabel = false;
                            Height.FontWeight = FontWeights.Normal;
                            Height.IsEnabled = HeightLabel;
                            txt8.IsEnabled = false;
                            WeightLabel = false;
                            Weight.FontWeight = FontWeights.Normal;
                            Weight.IsEnabled = WeightLabel;
                            txt9.IsEnabled = false;
                            break;
                        //Enables cooresponding label to true
                        //Enables cooresponding textbox to true
                        case "Age":
                            AgeLabel = true;
                            txt2.IsEnabled = true;
                            break;
                        case "FName":
                            FNameLabel = true;
                            txt3.IsEnabled = true;
                            break;
                        case "LName":
                            LNameLabel = true;
                            txt4.IsEnabled = true;
                            break;
                        case "JerseyNum":
                            JerseyNumLabel = true;
                            txt5.IsEnabled = true;
                            break;
                        case "Position":
                            PositionLabel = true;
                            txt6.IsEnabled = true;
                            break;
                        case "TeamID":
                            TeamIDLabel = true;
                            txt7.IsEnabled = true;
                            break;
                        case "Height":
                            HeightLabel = true;
                            txt8.IsEnabled = true;
                            break;
                        case "Weight":
                            WeightLabel = true;
                            txt9.IsEnabled = true;
                            break;
                    }
                }
                else
                {
                    label.FontWeight = FontWeights.Normal;

                    switch (label.Name)
                    {
                        //Disables ID textbox
                        //Enables all other labels
                        case "ID":
                            txt1.IsEnabled = false;
                            AgeLabel = false;
                            Age.IsEnabled = true;
                            FNameLabel = false;
                            FName.IsEnabled = true;
                            LNameLabel = false;
                            LName.IsEnabled = true;
                            JerseyNumLabel = false;
                            JerseyNum.IsEnabled = true;
                            PositionLabel = false;
                            Position.IsEnabled = true;
                            TeamIDLabel = false;
                            TeamID.IsEnabled = true;
                            HeightLabel = false;
                            Height.IsEnabled = true;
                            WeightLabel = false;
                            Weight.IsEnabled = true;
                            break;
                        //Disable cooresponding label and textbox
                        case "Age":
                            AgeLabel = false;
                            txt2.IsEnabled = false;
                            break;
                        case "FName":
                            FNameLabel = false;
                            txt3.IsEnabled = false;
                            break;
                        case "LName":
                            LNameLabel = false;
                            txt4.IsEnabled = false;
                            break;
                        case "JerseyNum":
                            JerseyNumLabel = false;
                            txt5.IsEnabled = false;
                            break;
                        case "Position":
                            PositionLabel = false;
                            txt6.IsEnabled = false;
                            break;
                        case "TeamID":
                            TeamIDLabel = false;
                            txt7.IsEnabled = false;
                            break;
                        case "Height":
                            HeightLabel = false;
                            txt8.IsEnabled = false;
                            break;
                        case "Weight":
                            WeightLabel = false;
                            txt9.IsEnabled = false;
                            break;
                    }
                }
            }
            else if (Action == "Update")
            {
                if (txt1.IsEnabled == false)
                {
                    //Enables ID textbox
                    //Disables all other textboxecs
                    //Clears all other textboxes
                    txt1.IsEnabled = true;
                    this.txt2.IsEnabled = false;
                    this.txt3.IsEnabled = false;
                    this.txt4.IsEnabled = false;
                    this.txt5.IsEnabled = false;
                    this.txt6.IsEnabled = false;
                    this.txt7.IsEnabled = false;
                    this.txt8.IsEnabled = false;
                    this.txt9.IsEnabled = false;
                    this.txt2.Text = "";
                    this.txt3.Text = "";
                    this.txt4.Text = "";
                    this.txt5.Text = "";
                    this.txt6.Text = "";
                    this.txt7.Text = "";
                    this.txt8.Text = "";
                    this.txt9.Text = "";
                    return;
                }

                FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get counter information from database
                Counter cnt = resp.ResultAs<Counter>(); //Set response to counter object

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
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                //Disables ID textbox
                //Enables all other textboxes
                resp = await client.GetTaskAsync(Node + "/" + txt1.Text);
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = true;
                this.txt3.IsEnabled = true;
                this.txt4.IsEnabled = true;
                this.txt5.IsEnabled = true;
                this.txt6.IsEnabled = true;
                this.txt7.IsEnabled = true;
                this.txt8.IsEnabled = true;
                this.txt9.IsEnabled = true;

                Players result = resp.ResultAs<Players>(); //Sets response to player object

                //Sets textboxes with information from response
                this.txt2.Text = result.Age.ToString();
                this.txt3.Text = result.FName;
                this.txt4.Text = result.LName;
                this.txt5.Text = result.jersey_no.ToString();
                this.txt6.Text = result.Position;
                this.txt7.Text = result.TeamID.ToString();
                this.txt8.Text = result.Height;
                this.txt9.Text = result.Weight.ToString();
            }
        }
    }
}

