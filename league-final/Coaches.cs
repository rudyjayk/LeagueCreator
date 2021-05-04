using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class Coaches
    {
        public int Age { get; set; } //Age of coach
        public int CoachId { get; set; } //Unique ID
        public string FName { get; set; } //First name of coach
        public string LName { get; set; } //Last name of coach
        public string Phone_no { get; set; } //Phone number of coach
        public string Role { get; set; } //Coach's role
        public int TeamID { get; set; } //Unique TeamID

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "Coach ID: " + CoachId + "\nFirst Name: "
                            + FName + "\nLast Name: " + LName
                            + "\nPhone Number: " + Phone_no + "\nAge:"
                            + Age + "\nRole: " + Role + "\nTeam ID: " + TeamID;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
       public string printRestirction(bool Age, bool FName, bool LName, bool PhoneNum, bool Role, bool TeamID)
       {
            string print = "Coach ID: " + CoachId;
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
