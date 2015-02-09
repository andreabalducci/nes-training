using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookings.Client.Support;

namespace Bookings.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var b = new Boostrapper())
            {
                b.Run();
            }
        }
    }
}
