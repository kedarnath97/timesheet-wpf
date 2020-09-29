using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Models;

namespace timesheet.services.Contracts
{
    public interface ITimeSheetEntryService
    {
        Task<List<TimeSheetRecord>> GetAllTimeSheetData();
        Task<List<TimeSheetRecord>> GetTimeSheetDataForUser(string userCode);
        Task<List<TimeSheetRecord>> GetTimeSheetDataForPeriod(string userCode, string begin, string end);
        Task<bool> AddUpdateTimeSheetEntry(List<TimeSheetRecord> timeSheetRecords);
    }
}
