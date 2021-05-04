using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class Sport
    {
        public int SportID { get; set; }//Unique ID
        public string SportName { get; set; }//Sport name
        public string SportRules { get; set; }//Rules of sport

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "Sport ID: " + SportID + "\n Sport Name: "
                            + SportName + "\nSport Rules: " + SportRules;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
        public string printRestirction(bool Rules, bool Name)
        {
            string print = "Sport ID: " + SportID;
            if (Name)
            {
                print += "\nSport Name: " + this.SportName;
            }
            if (Rules)
            {
                print += "\nSport Rules: " + this.SportRules;
            }

            return print;
        }
    }
}



