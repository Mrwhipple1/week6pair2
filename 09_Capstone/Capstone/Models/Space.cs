using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Space
    {
        public int Id { get; }
        public int VenueId { get; }
        public string Name { get; }
        public bool IsAccessible { get; }
        public string MonthOpen { get; }
        public string MonthClose { get; }
        public decimal Rate { get; }
        public int Occupancy { get; }

        public Space(int id, int venue_id, string name, bool is_accessible,
                      string open_from, string open_to, decimal daily_rate, int max_occupancy)
        {
            Id = id;
            VenueId = venue_id;
            Name = name;
            IsAccessible = is_accessible;
            MonthOpen = open_from;
            MonthClose = open_to;
            Rate = daily_rate;
            Occupancy = max_occupancy;
        }
    }
}
