using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Models;
using timesheet.services.Contracts;

namespace timesheet.services.Services
{
    public class TimeSheetEntryService : ITimeSheetEntryService
    {
        private string _baseurl = "https://localhost:44391/api/v1/";
        public TimeSheetEntryService()
        {

        }
        public async Task<List<TimeSheetRecord>> GetAllTimeSheetData()
        {
            using (HttpClient client = new HttpClient())
            {
                List<TimeSheetRecord> timeSheetRecords = new List<TimeSheetRecord>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/timesheetentry/getAllTimeSheetData");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    timeSheetRecords = JsonConvert.DeserializeObject<List<TimeSheetRecord>>(json);
                }
                return timeSheetRecords;
            }
        }

        public async Task<List<TimeSheetRecord>> GetTimeSheetDataForUser(string userCode)
        {
            using (HttpClient client = new HttpClient())
            {
                List<TimeSheetRecord> timeSheetRecords = new List<TimeSheetRecord>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/timesheetentry/getTimeSheetDataForUser?usercode=" + userCode);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    timeSheetRecords = JsonConvert.DeserializeObject<List<TimeSheetRecord>>(json);
                }
                return timeSheetRecords;
            }
        }

        public async Task<List<TimeSheetRecord>> GetTimeSheetDataForPeriod(string userCode, string begin, string end)
        {
            using (HttpClient client = new HttpClient())
            {
                List<TimeSheetRecord> timeSheetRecords = new List<TimeSheetRecord>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/timesheetentry/getTimeSheetDataForPeriod?usercode=" + userCode + "&begin=" + begin + "&end=" + end);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    timeSheetRecords = JsonConvert.DeserializeObject<List<TimeSheetRecord>>(json);
                }
                return timeSheetRecords;
            }
        }

        public async Task<bool> AddUpdateTimeSheetEntry(List<TimeSheetRecord> timeSheetRecords)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var jsonObject = JsonConvert.SerializeObject(timeSheetRecords);
                    var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(_baseurl + "/timesheetentry/addUpdateTimeSheetEntry", content);
                    return response.IsSuccessStatusCode;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }
    }
}
