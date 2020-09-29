using System.Collections.Generic;
using System.Threading.Tasks;
using timesheet.data.Models;

namespace timesheet.services.Contracts
{
    interface ITaskService
    {
        Task<List<TaskModel>> GetAllTasks();

    }
}
