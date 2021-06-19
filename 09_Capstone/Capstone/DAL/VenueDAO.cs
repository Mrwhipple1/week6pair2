using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class VenueDAO
    {
        private string connectionString;

        public VenueDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Venue> GetVenues()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                List<Venue> venues = new List<Venue>();

                conn.Open();

                string cmndText = "SELECT id, name, city_id, description FROM venue" +
                                  " ORDER BY name";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
                SqlDataReader reader = sqlCmnd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string name = Convert.ToString(reader["name"]);
                    int city_id = Convert.ToInt32(reader["city_id"]);
                    string description = Convert.ToString(reader["description"]);

                    Venue venue = new Venue(id, name, city_id, description);
                    venues.Add(venue);
                }
                return venues;
            }
        }

        public List<Category> GetCategories(int venueId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                List<Category> categories = new List<Category>();

                conn.Open();

                string cmndText = "SELECT id, name FROM category JOIN category_venue ON" +
                                  " category.id = category_venue.category_id WHERE " +
                                  "venue_id = @venueId";

                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
                sqlCmnd.Parameters.AddWithValue("@venueId", venueId);
                SqlDataReader reader = sqlCmnd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string name = Convert.ToString(reader["name"]);

                    Category category = new Category(id, name);

                    categories.Add(category);
                }
                return categories;
            }
        }
    }
}
//public string GetDescription(int selection)
//{
//    using (SqlConnection conn = new SqlConnection(connectionString))
//    {
//        List<string> names = new List<string>();
//        List<string> descriptions = new List<string>();

//        conn.Open();

//        string cmndText = "SELECT name, description FROM venue ORDER BY name";

//        SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
//        SqlDataReader reader = sqlCmnd.ExecuteReader();

//        while (reader.Read())
//        {
//            string name = Convert.ToString(reader["name"]);
//            string description = Convert.ToString(reader["description"]);

//            names.Add(name);
//            descriptions.Add(description);
//        }

//        string[] namesArray = names.ToArray();
//        string[] descriptionsArray = descriptions.ToArray();

//        for (int i = 0; i < namesArray.Length; i++)
//        {
//            if (i == selection - 1)
//            {
//                return descriptionsArray[i];
//            }
//        }
//    }
//    return null;
//}

