using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{

    public class Venue
    {
        public int Id { get; }
        public string Name { get; }
        public int CityId { get; }
        public string Description { get; }

        public Venue(int id, string name, int city_id, string description)
        {
            Id = id;
            Name = name;
            CityId = city_id;
            Description = description;
        }
    }
}
