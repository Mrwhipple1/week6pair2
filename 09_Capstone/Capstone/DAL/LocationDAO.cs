using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class LocationDAO
    {
        private string connectionString;

        public LocationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Location> GetLocations(int venueSelection, List<Venue> venues)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                List<Location> locations = new List<Location>();

                conn.Open();

                string cmndText = "SELECT city.id, city.name, state_abbreviation FROM city JOIN venue " +
                                  "ON city.id = venue.city_id WHERE venue.id = @venueId " +
                                  "ORDER BY venue.name";
                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);

                for (int i = 0; i < venues.Count; i++)
                {
                    if (i == venueSelection - 1)
                    {
                        int venueId = venues[i].Id;
                        sqlCmnd.Parameters.AddWithValue("@venueId", venueId);
                    }
                }

                SqlDataReader reader = sqlCmnd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string name = Convert.ToString(reader["name"]);
                    string stateAbbreviation = Convert.ToString(reader["state_abbreviation"]);

                    Location location = new Location(id, name, stateAbbreviation);
                    locations.Add(location);
                }
                return locations;
            }
        }
    }
}

