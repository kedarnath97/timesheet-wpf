using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timesheet.data.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public double WeeklyTotalEffort { get; set; }
        public double AverageEffort { get; set; }
    }
}
