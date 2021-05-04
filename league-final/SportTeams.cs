using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace league_final
{
    internal class SportTeams
    {
        public int LeagueID { get; set; } //League ID
        public int SportID { get; set; }//Sport ID
        public int TeamID { get; set; }//Unique ID
        public string TeamName { get; set; }//Team name
        public int numberOfCoaches { get; set; }//Number of coaches on team
        public int numberOfExtras { get; set; }//Number of extras on team
        public int numberOfPlayers { get; set; }//Number of players on team

        //Function that creates a string of all information for instance of class
        //Returns string to display to user
        public string printAll()
        {
            string print = "Team ID: " + TeamID + "\nLeague ID: "
                            + LeagueID + "\nSport ID: " + SportID
                            + "\nTeam Name: " + TeamName + "\nNumber of Coaches: "
                            + numberOfCoaches + "\nNumber of Extras: " + numberOfExtras + "\nNumber of Players: " + numberOfPlayers;

            return print;
        }

        //Function that creates a string of information that was only selected by the user
        //Returns string to display to user
        public string printRestirction(bool Age, bool FName, bool LName, bool PhoneNum, bool Role, bool TeamID)
        {
            string print = "Team ID: " + TeamID;
            if (FName)
            {
                print += "\nLeague ID: " + this.LeagueID;
            }
            if (LName)
            {
                print += "\nSport ID: " + this.SportID;
            }
            if (PhoneNum)
            {
                print += "\nTeam Name: " + this.TeamName;
            }
            if (Age)
            {
                print += "\nNumber of coaches: " + this.numberOfCoaches;
            }
            if (Role)
            {
                print += "\nNumber of Extras: " + this.numberOfExtras;
            }
            if (TeamID)
            {
                print += "\nNumber of Players: " + this.numberOfPlayers;
            }

            return print;
        }
    }
}




