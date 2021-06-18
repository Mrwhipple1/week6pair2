using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    class ReservationDAO
    {
        //Book a space for specific dates

        private string connectionString;

        public ReservationDAO (string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
