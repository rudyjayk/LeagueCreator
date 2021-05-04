using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class Extras
    {
        public int Age { get; set; } //Extra age
        public int ExtraID { get; set; } //Unique ID
        public string FName { get; set; }//First name of extra
        public string LName { get; set; }//Last name of extra
        public string Role { get; set; }//Role of extra
        public int TeamID { get; set; }//Unique Team ID

        public string Phone_no { get; set; }

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "Extra ID: " + ExtraID + "\nFirst Name: "
                            + FName + "\nLast Name: " + LName
                            + "\nPhone Number: " + Phone_no + "\nAge:"
                            + Age + "\nRole: " + Role + "\nTeam ID: " + TeamID;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
        public string printRestirction(bool Age, bool FName, bool LName, bool PhoneNum, bool Role, bool TeamID)
        {
            string print = "Extra ID: " + ExtraID;
            if (FName)
            {
                print += "\nFirst Name: " + this.FName;
            }
            if (LName)
            {
                print += "\nLast Name: " + this.LName;
            }
            if (PhoneNum)
            {
                print += "\nPhone Number: " + Phone_no;
            }
            if (Age)
            {
                print += "\nAge: " + this.Age;
            }
            if (Role)
            {
                print += "\nRole: " + this.Role;
            }
            if (TeamID)
            {
                print += "\nTeam ID: " + this.TeamID;
            }

            return print;
        }
    }

}



