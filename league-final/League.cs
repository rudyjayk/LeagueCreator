using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class League
    {
        public int LeagueID {get; set;} //Unique ID
        public string LeagueName { get; set; }//League name
        public int SportID { get; set; }//Unique sportID
        public int numberOfTeams { get; set; }//Number of teams

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "League ID: " + LeagueID + "\n League Name: "
                            + LeagueName + "\nSport ID: " + SportID + "\nNumber of Teams: " + numberOfTeams;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
        public string printRestirction(bool SportID, bool Name, bool Numteams)
        {
            string print = "League ID: " + this.LeagueID;
            if (Name)
            {
                print += "\nLeague Name: " + this.LeagueName;
            }
            if (SportID)
            {
                print += "\nSport ID: " + this.SportID;
            }
            if (Numteams)
            {
                print += "\nNumber of Teams: " + this.numberOfTeams;
            }

            return print;
        }
    }
}



