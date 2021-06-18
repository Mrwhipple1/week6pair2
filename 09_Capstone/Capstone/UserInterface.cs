using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class UserInterface
    {
        private string connectionString;

        private LocationDAO locationDAO;
        private VenueDAO venueDAO;
        private SpaceDAO spaceDAO;
        private ReservationDAO reservationDAO;

        bool done = false;

        public UserInterface(string connectionString)
        {
            this.connectionString = connectionString;

            venueDAO = new VenueDAO(connectionString);
            spaceDAO = new SpaceDAO(connectionString);
            reservationDAO = new ReservationDAO(connectionString);
            locationDAO = new LocationDAO(connectionString);
        }

        public void Run()
        {
            while (!done)
            {
                Console.WriteLine("Welcome to Exelsior Venues' Homepage!");
                Console.WriteLine("Please make a selection below!");
                Console.WriteLine("_____________________________________");

                MainMenu();
                Console.WriteLine();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        List<Venue> venues = ShowVenues();
                        Console.WriteLine();
                        int venueSelection = Convert.ToInt32(Console.ReadLine());
                        List<Location> locations = locationDAO.GetLocations(venueSelection, venues);
                        int venueId = ShowVenueDetails(venueSelection, venues, locations);
                        List<Space> spaces = spaceDAO.GetSpaces(venueId);
                        List<Space> spacesForVenue = ShowSpaces(venueId, spaces);
                        int spaceSelection = Convert.ToInt32(Console.ReadLine());
                        break;
                    case "2":
                        QuitProgram();
                        break;
                }
            }
        }

        private void MainMenu()
        {
            Console.WriteLine("Choose \"1\" to diplay venues.");
            Console.WriteLine("Choose \"2\" to quit the program.");
        }

        private List<Venue> ShowVenues()
        {
            List<Venue> venues = venueDAO.GetVenues();

            try
            {
                Console.WriteLine();
                Console.WriteLine("Select a venue below for more information:");
                Console.WriteLine();

                int row = 0;

                foreach (Venue venue in venues)
                {
                    row++;

                    Console.WriteLine(row + ". " + venue.Name);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return venues;
        }

        public int ShowVenueDetails(int venueSelection, List<Venue> venues,
                                    List<Location> locations)
        {
            try
            {
                for (int i = 0; i < venues.Count; i++)
                {
                    if (i == venueSelection - 1)
                    {
                        Console.WriteLine(venues[i].Name);
                        for (int i2 = 0; i2 < locations.Count; i2++)
                        {
                            Console.WriteLine(locations[i2].Name + ", " +
                                              locations[i2].StateAbbreviation);
                        }
                        Console.WriteLine(venues[i].Description);
                        return venues[i].Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return -1;
        }

        private List<Space> ShowSpaces(int venueId, List<Space> spaces)
        {          
            try
            {
                Console.WriteLine();
                Console.WriteLine("Select a space below for more information:");
                Console.WriteLine();

                int row = 0;

                foreach (Space space in spaces)
                {
                    row++;
                    Console.WriteLine(row + ". " + space.Name);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return spaces;
        }

        private void QuitProgram()
        {
            Console.WriteLine();
            Console.WriteLine("_____________________________________");
            Console.WriteLine("Thanks for Using Exelsior Venues' matrix! Goodbye.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            done = true;
        }
    }
}
