using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationDAO
    {
        private string connectionString;


        public ReservationDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Reservation> GetReservations(List<Space> spacesForVenue, int spaceSelection)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string cmndText = "SELECT reservation_id, space_id, number_of_attendees, " +
                                  "start_date, end_date, reserved_for FROM reservation ";
                SqlCommand sqlCmnd = new SqlCommand(cmndText, conn);
                SqlDataReader reader = sqlCmnd.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["reservation_id"]);
                    int space_id = Convert.ToInt32(reader["space_id"]);
                    int attendees = Convert.ToInt32(reader["number_of_attendees"]);
                    DateTime startDate = Convert.ToDateTime(reader["start_date"]);
                    DateTime endDate = Convert.ToDateTime(reader["end_date"]);
                    string reservationName = Convert.ToString(reader["reserved_for"]);

                    Reservation reservation = new Reservation(id, space_id, attendees,
                                                              startDate, endDate,
                                                              reservationName);
                    reservations.Add(reservation);
                }
                return reservations;
            }
        }
    }
}
