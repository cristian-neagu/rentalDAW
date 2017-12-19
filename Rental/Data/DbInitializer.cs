using Rental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {

            if (context.Cars.Any())
            {
                return;   // DB has been seeded
            }

            var cars = new Car[]
            {
            new Car{Make="Carson",Model="Alexander",LicenseNumber="2005-09-01"},
            new Car{Make="Meredith",Model="Alonso",LicenseNumber="2002-09-01"},
            new Car{Make="Arturo",Model="Anand",LicenseNumber="2003-09-01"},
            new Car{Make="Gytis",Model="Barzdukas",LicenseNumber="2002-09-01"},
            new Car{Make="Yan",Model="Li",LicenseNumber="2002-09-01"},
            new Car{Make="Peggy",Model="Justice",LicenseNumber="2001-09-01"},
            new Car{Make="Laura",Model="Norman",LicenseNumber="2003-09-01"},
            new Car{Make="Nino",Model="Olivetto",LicenseNumber="2005-09-01"}
            };
            foreach (Car s in cars)
            {
                context.Cars.Add(s);
            }
            context.SaveChanges();

        }
    }
}
