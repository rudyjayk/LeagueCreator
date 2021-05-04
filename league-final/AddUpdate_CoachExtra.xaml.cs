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
    /// Interaction logic for AddUpdate_CoachExtra.xaml
    /// </summary>
    public partial class AddUpdate_CoachExtra : Window
    {
        IFirebaseClient client; //Firebase client
        string Action; //Action you will be taking on databse
        string Node; //Node you will be doing the action on
        bool AgeLabel; //Whether to display age
        bool FNameLabel; //Whether to display First Name
        bool LNameLabel; //Whether to display Last Name
        bool PhoneNumLabel; //Whether to display Phone Number
        bool RoleLabel; //Whether to display Role
        bool TeamIDLabel;//Whether to display TeamID

        //Initialization of window
        //Based on action that is being performed on the databse the window will adjust what is available to access
        public AddUpdate_CoachExtra(IFirebaseClient client, string Action, string Node)
        {
            InitializeComponent();
            //Initializes all variables to link database
            this.client = client;
            this.Action = Action;
            this.Node = Node;

            //Disables use of all labels
            //Disable of use of ID textbox
            if (Action == "Add")
            {
                this.txt1.IsEnabled = false;
                this.ID.IsEnabled = false;
                this.Age.IsEnabled = false;
                this.FName.IsEnabled = false;
                this.LName.IsEnabled = false;
                this.PhoneNum.IsEnabled = false;
                this.Role.IsEnabled = false;
                this.TeamID.IsEnabled = false;
            }
            //Disables all textboxes and labels
            //Only allows ID textbox and label to be access for use
            else if (Action == "Update")
            {
                //this.ID.IsEnabled = false;
                this.Age.IsEnabled = false;
                this.FName.IsEnabled = false;
                this.LName.IsEnabled = false;
                this.PhoneNum.IsEnabled = false;
                this.Role.IsEnabled = false;
                this.TeamID.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
                this.txt5.IsEnabled = false;
                this.txt6.IsEnabled = false;
                this.txt7.IsEnabled = false;
            }
            //Disables all textboxes
            else if (Action == "Retrieve")
            {
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = false;
                this.txt3.IsEnabled = false;
                this.txt4.IsEnabled = false;
                this.txt5.IsEnabled = false;
                this.txt6.IsEnabled = false;
                this.txt7.IsEnabled = false;


                
            }
            
        }

        //Function that is performed when button is clicked
        //Adjusts depending on action to be performed and node being performed on
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            object data = null; //Data that is added to databse
            object obj = null; //obj is used to get unique key data
            FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Gets data for counter
            Counter get = resp.ResultAs<Counter>(); //Sets counter object to response from database

            //MessageBox.Show(get.cnt.ToString());
            
            if (Action == "Add") //Add
            {

                //Catches exception
                try
                {
                    if (Node == "Coaches") //Coaches
                    {
                        //Sets data to new coaches object with information that was inserted from user
                        data = new Coaches
                        {
                            CoachId = Convert.ToInt32(get.cnt) + 1,
                            Age = int.Parse(txt2.Text),
                            FName = txt3.Text,
                            LName = txt4.Text,
                            Phone_no = txt5.Text,
                            Role = txt6.Text,
                            TeamID = int.Parse(txt7.Text)
                        };

                        //Sets obj to new counter object and increases count by 1 to represent amount of unique key IDs in a node
                        obj = new Counter
                        {
                            cnt = Convert.ToInt32(get.cnt) + 1
                        };
                    }
                    else if (Node == "Extras") //Extras
                    {
                        //Sets data to new Extras object with user inputs
                        data = new Extras
                        {
                            ExtraID = Convert.ToInt32(get.cnt) + 1,
                            Age = int.Parse(txt2.Text),
                            FName = txt3.Text,
                            LName = txt4.Text,
                            Phone_no = txt5.Text,
                            Role = txt6.Text,
                            TeamID = int.Parse(txt7.Text)
                        };

                        //Sets obj to new Counter object and increasing amount of key IDs by 1
                        obj = new Counter
                        {
                            cnt = Convert.ToInt32(get.cnt) + 1
                        };
                    }
                }
                catch (Exception ex) //Catch exception when a string is inserted in a textbox but an integer is needed
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }


                //Used to access the correct data for node and not overwriting data
                string index = (get.cnt + 1).ToString();

                SetResponse response = await client.SetTaskAsync(Node + "/" + index, data); //Updates data in Node



                SetResponse response1 = await client.SetTaskAsync("Counter/" + Node, obj); //Updates count
                
                //After button pressed all textboxes will clear
                this.txt1.Text = "";
                this.txt2.Text = "";
                this.txt3.Text = "";
                this.txt4.Text = "";
                this.txt5.Text = "";
                this.txt6.Text = "";
                this.txt7.Text = "";

                //Messagebox that informs user data was inserted
                MessageBox.Show("Data Inserted");
                return;
            }else if (Action == "Update") //Update
            {

                //Catches exception
                try
                {
                    if (Node == "Coaches")
                    {
                        //Sets data to new coaches object with user inputted data
                        data = new Coaches
                        {
                            CoachId = int.Parse(txt1.Text),
                            Age = int.Parse(txt2.Text),
                            FName = txt3.Text,
                            LName = txt4.Text,
                            Phone_no = txt5.Text,
                            Role = txt6.Text,
                            TeamID = int.Parse(txt7.Text)
                        };

                    }
                    else if (Node == "Extras")
                    {
                        //Sets data to new Extras object with user inputted data
                        data = new Extras
                        {
                            ExtraID = int.Parse(txt1.Text),
                            Age = int.Parse(txt2.Text),
                            FName = txt3.Text,
                            LName = txt4.Text,
                            Phone_no = txt5.Text,
                            Role = txt6.Text,
                            TeamID = int.Parse(txt7.Text)
                        };

                    }
                }
                catch (Exception ex) //Catches exception when input data is not correct
                {
                    MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                    return;
                }


                FirebaseResponse response = await client.UpdateTaskAsync(Node + "/" + txt1.Text, data); //Updates node data
                MessageBox.Show("Data Updated"); //Messagebox that showws after data has been updated
            }
            else if (Action == "Retrieve") //Retrieve
            {
                   
                string print = null; //String that used for returning data wanting to be retrieved 
                if (txt1.IsEnabled == true) //If ID textbox is enabled on window
                {

                    FirebaseResponse response = await client.GetTaskAsync("Counter/" + Node); //Gets counter to check if ID entered is valid
                    Counter cnt = response.ResultAs<Counter>(); //Sets response to a counter object

                    try //Catches exception
                    {
                        int count = cnt.cnt; //Sets count

                        int ID = int.Parse(txt1.Text); //Sets ID

                        if (ID > count || ID < 1) //Checks if ID entered is between 1 and count
                        {
                            MessageBox.Show(txt1.Text + " is not a valid ID");
                            return;
                        }
                    }
                    catch (Exception ex) //Catches exception when input data is entered wrong
                    {
                        MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                        return;
                    }

                    response = await client.GetTaskAsync(Node + "/" + txt1.Text); //Get data with ID inputted

                    //Creates correct object data and calls print function
                    if (Node == "Coaches")
                    {
                        Coaches result = response.ResultAs<Coaches>();
                        print = result.printAll();
                    }
                    else if (Node == "Extras")
                    {
                        Extras result = response.ResultAs<Extras>();
                        print = result.printAll();
                    }

                    MessageBox.Show(print); //Prints data to a messagebox

                    //List<string> elements = new List<string>();
                    //elements.Add(print);

                    //DataWindow dataWindow = new DataWindow(Node, elements);

                    //dataWindow.Show();

                }
                else
                {
                    List<string> elements = new List<string>(); //List of strings that hold different data from databse.

                    //Runs through all key IDs
                    for (int i = 1; i <= get.cnt; i++)
                    {
                        FirebaseResponse result = await client.GetTaskAsync(Node + "/" + i); //Gets node

                        //Catches exceptions
                        try {

                            if (Node == "Coaches")
                            {
                                Coaches coach = result.ResultAs<Coaches>(); //Turns response into coaches object
                                bool addElement = true; //Sets add element to true
                                //******The next lines of code was a lot of copy and paste and save myself I will explain it clearly once
                                if (AgeLabel) //Checks if Label is enabled on window
                                {
                                    if (txt2.Text != "") //Checks if user inputted into textbox that corresponds with label
                                    {
                                        if (coach.Age != int.Parse(txt2.Text)) //If the data inserted by the user and data in the databse does not match
                                        {
                                            addElement = false; //Sets to false because this element is not being added
                                        }
                                    }

                                }
                                if (FNameLabel)
                                {
                                    if (txt3.Text != "")
                                    {
                                        if (coach.FName != txt3.Text)
                                        {

                                            addElement = false;
                                        }
                                    }
                                }
                                if (LNameLabel)
                                {
                                    if (txt4.Text != "")
                                    {
                                        if (coach.LName != txt4.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (PhoneNumLabel)
                                {
                                    if (txt5.Text != "")
                                    {
                                        if (coach.Phone_no != txt5.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (RoleLabel)
                                {
                                    if (txt6.Text != "")
                                    {
                                        if (coach.Role != txt6.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (TeamIDLabel)
                                {
                                    if (txt7.Text != "")
                                    {
                                        if (coach.TeamID != int.Parse(txt7.Text))
                                        {
                                            addElement = false;
                                        }
                                    }
                                }

                                if (addElement)// If addElement is not false
                                {
                                    elements.Add(coach.printRestirction(AgeLabel, FNameLabel, LNameLabel, PhoneNumLabel, RoleLabel, TeamIDLabel)); //Calss print function and adds to list of strings
                                }
                            }
                            else if (Node == "Extras")
                            {
                                Extras extras = result.ResultAs<Extras>();//Turns respons into Extra object
                                bool addElement = true; //Sets add element to true
                                //******The next lines of code was a lot of copy and paste and save myself it is explained above at line 297
                                if (AgeLabel)
                                {
                                    if (txt2.Text != "")
                                    {
                                        if (extras.Age != int.Parse(txt2.Text))
                                        {
                                            addElement = false;
                                        }
                                    }

                                }
                                if (FNameLabel)
                                {
                                    if (txt3.Text != "")
                                    {
                                        if (extras.FName != txt3.Text)
                                        {

                                            addElement = false;
                                        }
                                    }
                                }
                                if (LNameLabel)
                                {
                                    if (txt4.Text != "")
                                    {
                                        if (extras.LName != txt4.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (PhoneNumLabel)
                                {
                                    if (txt5.Text != "")
                                    {
                                        if (extras.Phone_no != txt5.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (RoleLabel)
                                {
                                    if (txt6.Text != "")
                                    {
                                        if (extras.Role != txt6.Text)
                                        {
                                            addElement = false;
                                        }
                                    }
                                }
                                if (TeamIDLabel)
                                {
                                    if (txt7.Text != "")
                                    {
                                        if (extras.TeamID != int.Parse(txt7.Text))
                                        {
                                            addElement = false;
                                        }
                                    }
                                }

                                if (addElement)
                                {
                                    elements.Add(extras.printRestirction(AgeLabel, FNameLabel, LNameLabel, PhoneNumLabel, RoleLabel, TeamIDLabel));
                                }
                            }
                        }
                        catch (Exception ex) //Catches exception when input data is incorrect
                        {
                            MessageBox.Show(ex.Message + ", Check what you inserted in the Textboxes");
                            return;
                        }
                       
                        
                    }


                    if (elements.Count == 0) //If there are no strings in list shows that no elements cooresponded with what the user was looking for
                    {
                        MessageBox.Show("There are no elements with the inputs inserted!");
                        return;
                    }

                    //DataWindow dataWindow = new DataWindow(Node, elements);

                    //dataWindow.Show();
                    
                    foreach (string ele in elements){ //Shows all data in message boxes
                        MessageBox.Show(ele);
                    }
                }

                

                //foreach (string ele in arr){
                  //  MessageBox.Show(ele);
                //}

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
        //For Retrieve: Select labels that you want to be retruned to you
        private async void Select(object sender, MouseButtonEventArgs e)
        {
            if (Action == "Retrieve")
            {
                Label label = sender as Label; //Sets label

                if (label.FontWeight == FontWeights.Normal) //If font weight is Normal - dont be retrieved
                {
                    label.FontWeight = FontWeights.Bold; //Set font weight to bold

                    switch (label.Name) //Uses label name for switch
                    {
                        case "ID": //If ID label
                            //Enables only ID label and textbox
                            //Disables all other labels and textboxes and sets the labels font weight to normal
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
                            PhoneNumLabel = false;
                            PhoneNum.FontWeight = FontWeights.Normal;
                            PhoneNum.IsEnabled = PhoneNumLabel;
                            txt5.IsEnabled = false;
                            RoleLabel = false;
                            Role.FontWeight = FontWeights.Normal;
                            Role.IsEnabled = RoleLabel;
                            txt6.IsEnabled = false;
                            TeamIDLabel = false;
                            TeamID.FontWeight = FontWeights.Normal;
                            TeamID.IsEnabled = TeamIDLabel;
                            txt7.IsEnabled = false;
                            break;
                        case "Age": //Changes cooresponding label and textbox to enabled for all below cases
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
                        case "PhoneNum":
                            PhoneNumLabel = true;
                            txt5.IsEnabled = true;
                            break;
                        case "Role":
                            RoleLabel = true;
                            txt6.IsEnabled = true;
                            break;
                        case "TeamID":
                            TeamIDLabel = true;
                            txt7.IsEnabled = true;
                            break;

                    }

                }
                else
                {
                    label.FontWeight = FontWeights.Normal; //Changes weight to normal

                    switch (label.Name)
                    {
                        case "ID": //If ID label is pressed
                            //Disables ID textbox
                            //Enables all other textboxes and labels
                            txt1.IsEnabled = false;
                            AgeLabel = false;
                            Age.IsEnabled = true;
                            FNameLabel = false;
                            FName.IsEnabled = true;
                            LNameLabel = false;
                            LName.IsEnabled = true;
                            PhoneNumLabel = false;
                            PhoneNum.IsEnabled = true;
                            RoleLabel = false;
                            Role.IsEnabled = true;
                            TeamIDLabel = false;
                            TeamID.IsEnabled = true;
                            break;
                        case "Age": //Changes cooresponding label and textbox to disables for below cases
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
                        case "PhoneNum":
                            PhoneNumLabel = false;
                            txt5.IsEnabled = false;
                            break;
                        case "Role":
                            RoleLabel = false;
                            txt6.IsEnabled = false;
                            break;
                        case "TeamID":
                            TeamIDLabel = false;
                            txt7.IsEnabled = false;
                            break;

                    }

                }
            }
            else if (Action == "Update")
            {
                if(txt1.IsEnabled == false) //If ID textbox is not enabled then enables that textbox and disables and clears all other textboxes
                {
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

                FirebaseResponse resp = await client.GetTaskAsync("Counter/" + Node); //Gets count data 
                Counter cnt = resp.ResultAs<Counter>(); //Sets to counter object

                try //Catches exception
                {
                    int count = cnt.cnt;

                    int ID = int.Parse(txt1.Text);
                    if (ID > count || ID < 1) //If ID entered is not valid informs user
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




                resp = await client.GetTaskAsync(Node + "/" + txt1.Text); //Gets data for cooresponding ID

                //Disables ID textbox
                //Enables all other textboxes
                this.txt1.IsEnabled = false;
                this.txt2.IsEnabled = true;
                this.txt3.IsEnabled = true;
                this.txt4.IsEnabled = true;
                this.txt5.IsEnabled = true;
                this.txt6.IsEnabled = true;
                this.txt7.IsEnabled = true;

                //Puts information from cooresponding ID into textboxes to let user update data
                if (Node == "Coaches")
                {
                    
                    Coaches result = resp.ResultAs<Coaches>();
                    this.txt2.Text = result.Age.ToString();
                    this.txt3.Text = result.FName;
                    this.txt4.Text = result.LName;
                    this.txt5.Text = result.Phone_no;
                    this.txt6.Text = result.Role;
                    this.txt7.Text = result.TeamID.ToString();

                    
                }
                else if (Node == "Extras")
                {
                    Extras result = resp.ResultAs<Extras>();
                    this.txt2.Text = result.Age.ToString();
                    this.txt3.Text = result.FName;
                    this.txt4.Text = result.LName;
                    this.txt5.Text = result.Phone_no;
                    this.txt6.Text = result.Role;
                    this.txt7.Text = result.TeamID.ToString();
                }
            }
        }

    }
}
