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
    /// Interaction logic for AddUpdate_Sports.xaml
    /// </summary>
    public partial class AddUpdate_Sports : Window
    {
        IFirebaseClient client; //Database connection
        string Action; //Action being performed
        string Node; //Node the action will be performed on
        bool NameLabel; //Whether Name will be retrieved or not
        bool RuleLabel; //Whether Rule will be retrieved or not
        public AddUpdate_Sports(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            this.client = client; //Assigns client
            this.Action = Action; //Assigns Action
            this.Node = Node; //Assigns Node

            if (Action == "Add")
            {
                //Disables ID textbox and all labels
                //Enables all other textboxes
                this.txt1.IsEnabled = false;
                this.ID.IsEnabled = false;
                this.SportName.IsEnabled = false;
                this.Rule.IsEnabled = false;
            }
            else if (Action == "Update")
            {
                //Enables only ID textbox
                //Disables all other textboxes
                this.SportName.IsEnabled = false;
                this.Rule.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
            }
            else if (Action == "Retrieve")
            {
                //Disables all textboxes
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
            }
        }

        //Function that is performed when button is clicked
        //Adjusts depending on action to be performed and node being performed on
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get counter data
            Counter get = resp.ResultAs<Counter>(); //Set response to counter object
            object data = null;
            object obj = null;

            if (Action == "Add")
            {
                try
                {
                    //Creates new Sport object with data inserted from user
                    data = new Sport
                    {
                        SportID = Convert.ToInt32(get.cnt) + 1,
                        SportName = txt2.Text,
                        SportRules = txt3.Text,
                    };

                    //Create new counter object with cnt being the same as the ID of the sport object created above
                    obj = new Counter
                    {
                        cnt = Convert.ToInt32(get.cnt) + 1
                    };
                }
                catch (Exception ex) //Catches exception when input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }

                string index = (get.cnt + 1).ToString();
                SetResponse response = await client.SetTaskAsync(Node + "/" + index, data); //Adds sport object to databse
                SetResponse response1 = await client.SetTaskAsync("Counter/" + Node, obj); //Updates count in database

                //Resets all textboxes back to blank
                this.txt1.Text = "";
                this.txt2.Text = "";
                this.txt3.Text = "";


                MessageBox.Show("Data Inserted");
            }
            else if (Action == "Update")
            {
                try
                {
                    //Creates new sport object with data inserted in the textboxes
                    data = new Sport
                    {
                        SportID = int.Parse(txt1.Text),
                        SportName = txt2.Text,
                        SportRules = txt3.Text,
                    };
                }
                catch (Exception ex) //Catches exception when input data is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }
                FirebaseResponse response = await client.UpdateTaskAsync(Node + "/" + txt1.Text, data); //Updates information on database

                MessageBox.Show("Data Updated");
            }
            else if (Action == "Retrieve")
            {
                string print = null;
                if (txt1.IsEnabled)
                {
                    FirebaseResponse response = await client.GetTaskAsync("Counter/" + Node); //Gets counter information
                    Counter cnt = response.ResultAs<Counter>(); //Sets counter object to response

                    try
                    {
                        //Checks if ID entered is valid

                        int count = cnt.cnt;

                        int ID = int.Parse(txt1.Text);

                        if (ID > count || ID < 1)
                        {
                            MessageBox.Show(txt1.Text + " is not a valid ID");
                            return;
                        }

                    }
                    catch (Exception ex)//Catches exception when input data is incorrect
                    {
                        MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                        return;
                    }

                    response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Retrieves data that user prompted

                    Sport result = response.ResultAs<Sport>(); //Set to Sport result

                    print = result.printAll(); //Calls print function to get information in a string

                    MessageBox.Show(print); //Shows information to user
                }
                else
                {
                    List<string> elements = new List<string>(); //Creates a list that holds strings with all selected information

                    //Iterates through all instances of a certain node
                    for (int i = 1; i <= get.cnt; i++)
                    {
                        FirebaseResponse result = await client.GetTaskAsync(Node + "/" + i); //Gets node information

                        try
                        {
                            //Set response to a Sport object
                            Sport sport = result.ResultAs<Sport>();
                            //Bool to check if node being analyzed in this iteration can be added to the list
                            bool addElement = true;

                            //If cooresponding label is enabled
                            if (NameLabel)
                            {
                                if (txt2.Text != "") //Check whether user inputted into cooresponding textbox
                                {
                                    if (sport.SportName != txt2.Text) //Checks if user input does not cooresponds to data on databse
                                    {
                                        addElement = false; //Sets addElement to false
                                    }
                                }
                            }
                            //Look at above comments
                            if (RuleLabel)
                            {
                                if (txt3.Text != "")
                                {
                                    if (sport.SportRules != txt3.Text)
                                    {
                                        addElement = false;
                                    }
                                }
                            }

                            if (addElement) //Calls print function and adds element to list
                            {
                                elements.Add(sport.printRestirction(RuleLabel, NameLabel));
                            }
                        }
                        catch (Exception ex) //Catches exception when input data is incorrect
                        {
                            MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                            return;
                        }
                    }

                    if (elements.Count == 0) //If list is empty then inform user
                    {
                        MessageBox.Show("There are no elements with the inputs inserted!");
                    }

                    foreach (string ele in elements){ //Print all selected information according to user
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
                Label label = sender as Label; //Gets selected label

                //Switches between normal and bold font weight
                if (label.FontWeight == FontWeights.Normal)
                {
                    label.FontWeight = FontWeights.Bold;

                    switch (label.Name)
                    {
                        //Enables ID textbox
                        //Disables all other textboxes
                        //Sets all font weights to normal
                        case "ID":
                            txt1.IsEnabled = true;
                            NameLabel = false;
                            SportName.FontWeight = FontWeights.Normal;
                            SportName.IsEnabled = NameLabel;
                            RuleLabel = false;
                            Rule.FontWeight = FontWeights.Normal;
                            Rule.IsEnabled = RuleLabel;
                            break;
                        //Enables cooresponding textbox
                        case "SportName":
                            NameLabel = true;
                            txt2.IsEnabled = true;
                            break;
                        case "Rule":
                            RuleLabel = true;
                            txt3.IsEnabled = true;
                            break;
                    }
                }
                else
                {
                    label.FontWeight = FontWeights.Normal;

                    switch (label.Name)
                    {
                        //Disables ID textbox
                        //Enables all other textboxes
                        case "ID":
                            txt1.IsEnabled = false;
                            NameLabel = false;
                            SportName.IsEnabled = true;
                            RuleLabel = false;
                            Rule.IsEnabled = true;
                            break;
                        //Disables cooresponding textbox
                        case "SportName":
                            NameLabel = false;
                            txt2.IsEnabled = false;
                            break;
                        case "Rule":
                            RuleLabel = false;
                            txt3.IsEnabled = false;
                            break;
                    }
                }
            }
            else if (Action == "Update")
            {
                if (txt1.IsEnabled == false)
                {
                    //Enables ID textbox
                    //Disables and clears all text boxes
                    txt1.IsEnabled = true;
                    this.txt2.IsEnabled = false;
                    this.txt3.IsEnabled = false;
                    this.txt2.Text = "";
                    this.txt3.Text = "";
                    return;
                }

                FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Get counter information
                Counter cnt = resp.ResultAs<Counter>(); //Set response to counter object

                try
                {
                    //Checks if ID entered is valid

                    int count = cnt.cnt;

                    int ID = int.Parse(txt1.Text);

                    if (ID > count || ID < 1)
                    {
                        MessageBox.Show(txt1.Text + " is not a valid ID");
                        return;
                    }
                }
                catch (Exception ex) //Catches exception when input data enetered is incorrect
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }
                
                
                resp = await client.GetTaskAsync(Node + "/" + txt1.Text);

                //Disables ID textbox
                //Enables all other textboxes for editing 
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = true;
                this.txt3.IsEnabled = true;

                //Display information from response to the cooresponding textboxes
                Sport result = resp.ResultAs<Sport>();
                this.txt2.Text = result.SportName;
                this.txt3.Text = result.SportRules;

            }
        }
    }
}
