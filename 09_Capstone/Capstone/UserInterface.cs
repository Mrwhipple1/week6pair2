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
                            List<Reservation> reservations = reservationDAO.GetReservations(spacesForVenue, spaceSelection);
                            //ShowReservations(spacesForVenue, spaceSelection, reservations);
                            if (spaceSelection == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            int bookOrNot = Convert.ToInt32(Console.ReadLine());
                            if (bookOrNot == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            int startMonth = StartMonthOpenCheck();
                            if (startMonth == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            int endMonth = EndMonthOpenCheck();
                            if (endMonth == 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine("_____________________________________");
                                break;
                            }
                            Space space = GetSpace(spaces, spaceSelection);
                            bool open = IsOpen(space, startMonth, endMonth);
                            if (open == false)
                            {
                                break;
                            }
                            DateTime startDate = GetStartDate(startMonth);
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

        public void ShowReservations(List<Space> spacesForVenue, int spaceSelection, List<Reservation> reservations)
        {
            for (int i = 0; i < spacesForVenue.Count; i++)
            {
                if (i == spaceSelection - 1)
                {
                    string spaceName = spacesForVenue[i].Name;

                    Console.WriteLine();
                    Console.WriteLine("_____________________________________");
                    Console.WriteLine("Here are the active reservations for " +
                                      spaceName + " this year.");

                    foreach (Reservation reservation in reservations)
                    {
                        Console.WriteLine(reservation.StartDate + "-" +reservation.EndDate);
                    }
                }
            }
        }

        public int StartMonthOpenCheck()
        {
            try
            {
                int startMonth;

                Console.WriteLine();
                Console.WriteLine("_____________________________________");
                Console.WriteLine("Please enter the month during which your event begins " +
                                  "(Ex: 1, 6, 10, 12, etc.) or select \"0\" to return to the" +
                                  " main menu:");
                Console.WriteLine();
                startMonth = Convert.ToInt32(Console.ReadLine());

                return startMonth;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return -1; ;
        }

        public int EndMonthOpenCheck()
        {
            try
            {
                int endMonth;

                Console.WriteLine("Please enter the month during which your event ends " +
                                  "(Ex: 1, 6, 10, 12, etc.) or select \"0\" to return to the" +
                                  " main menu:");
                Console.WriteLine();
                endMonth = Convert.ToInt32(Console.ReadLine());

                return endMonth;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong, try again later.");
                Console.WriteLine(ex.Message);
                QuitProgram();
            }
            return -1;
        }

        public Space GetSpace(List<Space> spaces, int spaceSelection)
        {
            return spaces[spaceSelection - 1];
        }

        public bool IsOpen(Space space, int startMonth,
                                 int endMonth)
        {
            bool result = false;

            if (startMonth > endMonth)
            {
                Console.WriteLine("Sorry, this timeframe doesn't compute. Please select a" +
                                  " different space or select a different timeframe for your" +
                                  " booking. Thank you!");
                Console.WriteLine();
                Console.WriteLine("_____________________________________");
            }
            if (startMonth >= Convert.ToInt32(space.MonthOpen) &&
                endMonth <= Convert.ToInt32(space.MonthClose))
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

        public DateTime GetStartDate(int startMonth)
        {
            Console.WriteLine("Great! We're open during that time and happy to host! Please " +
                              "let us know the day of the month you'd like your reservation" +
                              " to begin.");
            int startDayInt = Convert.ToInt32(Console.ReadLine());

            string startDateString = "2021-" + startMonth + "-" + startDayInt;

            DateTime startDate = Convert.ToDateTime(startDateString);

            return startDate;
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
