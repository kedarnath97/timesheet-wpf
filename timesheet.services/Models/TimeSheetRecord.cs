using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timesheet.data.Models
{
    public class TimeSheetRecord
    {
        public int EntryId { get; set; }
        public DateTime RecordDate { get; set; }
        public string EmployeeCode { get; set; }
        public int TaskId { get; set; }
        public double TaskHours { get; set; }
        public string Comments { get; set; }
    }
}
