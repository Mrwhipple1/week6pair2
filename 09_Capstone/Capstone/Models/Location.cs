using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Location
    {
        public int Id { get; }
        public string Name { get; }
        public string StateAbbreviation { get; }

        public Location(int id, string name, string state_abbreviation)
        {
            Id = id;
            Name = name;
            StateAbbreviation = state_abbreviation;
        }
    }
}
