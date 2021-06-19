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
            try
            {
                while (!done)
                {
                    Console.WriteLine("Welcome to Exelsior Venues' Homepage!");
                    Console.WriteLine("Please make a selection below!");
                    Console.WriteLine("_____________________________________");

                    MainMenu();
                    Console.WriteLine();
                    string input = Console.ReadLine().ToUpper();

                    switch (input)
                    {
                        case "V":
                            List<Venue> venues = venueDAO.GetVenues();
                            ShowVenues(venues);
                            int venueSelection = Convert.ToInt32(Console.ReadLine());
                            if (venueSelection == 0)
                            {
                                break;
                            }
                            List<Location> locations = locationDAO.GetLocations(venueSelection, venues);
                            int venueId = ShowVenueDetails(venueSelection, venues, locations);
                            List<Category> categories = venueDAO.GetCategories(venueId);
                            ShowCategories(categories);
                            List<Space> spaces = spaceDAO.GetSpaces(venueId);
                            List<Space> spacesForVenue = ShowSpaces(venueId, spaces);
                            int spaceSelection = Convert.ToInt32(Console.ReadLine());
                            if (spaceSelection == 0)
                            {
                                break;
                            }

                            break;
                        case "Q":
                            QuitProgram();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
        }

        private void MainMenu()
        {
            Console.WriteLine("Choose \"V\" to diplay venues.");
            Console.WriteLine("Choose \"Q\" to quit the program.");
        }

        private void ShowVenues(List<Venue> venues)
        {
            try
            {
                Console.WriteLine("_____________________________________");
                Console.WriteLine("Select a venue below for more information or select 0 " +
                                  "to return to the main menu:");
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
                        Console.WriteLine("_____________________________________");
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

        public void ShowCategories(List<Category> categories)
        {
            try
            {
                int count = 0;

                foreach (Category category in categories)
                {
                    count++;
                    Console.WriteLine("Category #" + count + ": " + category.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
        }

        private List<Space> ShowSpaces(int venueId, List<Space> spaces)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("Select one of this fine venue's spaces below to" +
                                  " check availability, or select 0 to return to the" +
                                  " main menu.");
                Console.WriteLine();

                int row = 0;

                foreach (Space space in spaces)
                {
                    row++;
                    Console.WriteLine(row + ".");
                    Console.WriteLine(space.Name);
                    Console.WriteLine("Daily rate = " + "$" + space.Rate);
                    Console.WriteLine("Is wheelchair accessible: " + space.IsAccessible);
                    Console.WriteLine("Maximum Occupancy: " + space.Occupancy);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return spaces;
        }

        public void QuitProgram()
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
