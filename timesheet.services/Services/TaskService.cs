using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Models;
using timesheet.services.Contracts;

namespace timesheet.services.Services
{
    public class TaskService : ITaskService
    {
        private string _baseurl = "https://localhost:44391/api/v1/";
        public TaskService()
        {

        }
        public async Task<List<TaskModel>> GetAllTasks()
        {
            using (HttpClient client = new HttpClient())
            {
                List<TaskModel> tasks = new List<TaskModel>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/task/gettasks");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tasks = JsonConvert.DeserializeObject<List<TaskModel>>(json);
                }
                return tasks;
            }
        }
    }
}
