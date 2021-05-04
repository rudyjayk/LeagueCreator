using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class Players
    {
        public int Age { get; set; } //Player age
        public string FName { get; set; }//Player first name
        public string Height { get; set; }//Player height
        public int jersey_no { get; set; }//Player jersey number
        public string LName { get; set; }//Player last name
        public int PlayerID { get; set; }//Unique player ID
        public string Position { get; set; }//Player position
        public int TeamID { get; set; }//Unique team ID
        public int Weight { get; set; }//Player weight

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "Player ID: " + PlayerID + "\nFirst Name: "
                            + FName + "\nLast Name: " + LName
                            + "\nAge: " + Age + "\nHeight: "
                            + Height + "\nWeight : " + Weight + "\nJersey Number: " + jersey_no + "\nPosition: " + Position + "\nTeam ID: " + TeamID;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
        public string printRestirction(bool Age, bool FName, bool LName, bool PhoneNum, bool Role, bool JerseyNum, bool Position, bool TeamID)
        {
            string print = "Player ID: " + PlayerID;
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
                print += "\nAge: " + this.Age;
            }
            if (Age)
            {
                print += "\nHeight: " + Height;
            }
            if (Role)
            {
                print += "\nWeight : " + Weight;
            }
            if (JerseyNum)
            {
                print += "\nJersey Number: " + jersey_no;
            }
            if (Position)
            {
                print += "\nPosition " + Position;
            }
            if (TeamID)
            {
                print += "\nTeam ID: " + this.TeamID;
            }

            return print;
        }
    }
}



