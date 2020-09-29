using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Contracts;
using timesheet.data.Models;

namespace timesheet.data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private string _baseurl = "https://localhost:44391/api/v1/";
        public EmployeeService()
        {

        }
        public async Task<List<Employee>> GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                List<Employee> employees = new List<Employee>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/employee/getall");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }
                return employees;
            }
        }
       
    }
}
