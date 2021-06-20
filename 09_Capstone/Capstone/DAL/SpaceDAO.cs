using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    class SpaceDAO
    {
        private string connectionString;

        public SpaceDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Space> GetSpaces(int venueId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                List<Space> spaces = new List<Space>();

                conn.Open();

                string cmndText = "SELECT id, venue_id, name, is_accessible, " +
                                  "open_from, open_to, daily_rate, max_occupancy " +
                                  "FROM space WHERE venue_id = @venue_id " +
                                  "ORDER BY name";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
                sqlCmnd.Parameters.AddWithValue("@venue_id", venueId);
                SqlDataReader reader = sqlCmnd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["open_from"] != DBNull.Value)
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        venueId = Convert.ToInt32(reader["venue_id"]);
                        string name = Convert.ToString(reader["name"]);
                        bool isAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        string openFrom = Convert.ToString(reader["open_from"]);
                        string openTo = Convert.ToString(reader["open_to"]);
                        decimal dailyRate = Convert.ToInt32(reader["daily_rate"]);
                        int maxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

                        Space space = new Space(id, venueId, name, isAccessible, openFrom, openTo,
                                                dailyRate, maxOccupancy);
                        spaces.Add(space);
                    }
                    else
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        venueId = Convert.ToInt32(reader["venue_id"]);
                        string name = Convert.ToString(reader["name"]);
                        bool isAccessible = Convert.ToBoolean(reader["is_accessible"]);
                        string openFrom = "1";
                        string openTo = "12";
                        decimal dailyRate = Convert.ToInt32(reader["daily_rate"]);
                        int maxOccupancy = Convert.ToInt32(reader["max_occupancy"]);

                        Space space = new Space(id, venueId, name, isAccessible, openFrom, openTo,
                                                dailyRate, maxOccupancy);
                        spaces.Add(space);
                    }
                }
                return spaces;
            }
        }
    }
}
