using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Id { get; }
        public int SpaceId { get; }
        public int Attendees { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public string ReservationName { get; }

        public Reservation (int id, int space_id, int num_of_attendees, DateTime start_date,
                            DateTime end_date, string reservation_name)
        {
            Id = id;
            SpaceId = space_id;
            Attendees = num_of_attendees;
            StartDate = start_date;
            EndDate = end_date;
            ReservationName = reservation_name;
        }
    }
}
