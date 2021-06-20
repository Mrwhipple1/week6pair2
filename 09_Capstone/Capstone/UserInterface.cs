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
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
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
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            List<Reservation> reservations = reservationDAO.GetReservations(spacesForVenue, spaceSelection);
                            Space space = ShowReservations(spacesForVenue, spaceSelection, reservations);
                            int bookOrNot = Convert.ToInt32(Console.ReadLine());
                            if (bookOrNot == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            if (bookOrNot == 1)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                DateTime startDate = GetStartDate(space);
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                int lengthOfBooking = GetLengthOfBooking();
                                bool open = IsOpen(space, startDate, lengthOfBooking);
                                if (open == false)
                                {
                                    break;
                                }
                                else
                                {
                                    //IsAvailable
                                }
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
            Console.WriteLine("Choose \"V\" to diplay venues or \"Q\" to quit the program.");
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

        public Space ShowReservations(List<Space> spacesForVenue, int spaceSelection, List<Reservation> reservations)
        {
            for (int i = 0; i < spacesForVenue.Count; i++)
            {
                if (i == spaceSelection - 1)
                {
                    string spaceName = spacesForVenue[i].Name;

                    Console.WriteLine();
                    Console.WriteLine("_____________________________________");
                    Console.WriteLine(spaceName + " is open from 2021/" +
                                      spacesForVenue[i].MonthOpen + "/01,"
                                      + " to the end of 2021/" + spacesForVenue[i].MonthClose
                                      + ".");
                    Console.WriteLine("Here are the active reservations for this space:");

                    foreach (Reservation reservation in reservations)
                    {
                        Console.WriteLine(reservation.StartDate + " - " + reservation.EndDate);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("To book this venue for open dates not currently reserved, please" +
                              " enter \"1\" below. Otherwise, enter \"0\" to exit to main menu.");
            Console.WriteLine();

            return spacesForVenue[spaceSelection - 1];
        }

        public DateTime GetStartDate(Space space)
        {
            Console.WriteLine("Great! We're happy to host! Please let us know the" +
                              " month you'd like your reservation to begin (Ex: 2, 11, etc.):");
            Console.WriteLine();

            int startMonth = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("_____________________________________");
            Console.WriteLine("Which day of that month (5, 21, etc)?");
            Console.WriteLine();

            int startDay = Convert.ToInt32(Console.ReadLine());

            DateTime startDate = new DateTime(2021, startMonth, startDay);
            return startDate;
        }

        public int GetLengthOfBooking()
        {            
            Console.WriteLine("And how many days - including the start day - would you like to " +
                              "book? (enter as numerical)");
            Console.WriteLine();

            int lengthOfBooking = Convert.ToInt32(Console.ReadLine());

            return lengthOfBooking;
        }

        public bool IsOpen(Space space, DateTime startDate, int lengthOfBooking)
        {
            bool result = false;

            DateTime endDate = startDate.AddDays(lengthOfBooking - 1);

            if (endDate.Month >= Convert.ToInt32(space.MonthClose))
            {
                Console.WriteLine("Sorry, this timeframe doesn't compute. Please select a" +
                                  " different space or select a different timeframe for your" +
                                  " booking. Thank you!");
                Console.WriteLine();
                Console.WriteLine("_____________________________________");
            }
            if (startDate.Month >= Convert.ToInt32(space.MonthOpen) &&
                endDate.Month <= Convert.ToInt32(space.MonthClose))
            {
                result = true;
                return result;
            }
            else
            {
                Console.WriteLine("Sorry, this space is closed during that span of time. " +
                                  "Please select a different space or select a different " +
                                  "timeframe for your booking. Thank you!");
                Console.WriteLine();
                Console.WriteLine("_____________________________________");
            }
            return result;
        }

        public void QuitProgram()
        {
            Console.WriteLine();
            Console.WriteLine("_____________________________________");
            Console.WriteLine("Thanks for Using Exelsior Venues' booking matrix! Goodbye.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            done = true;
        }
    }
}
