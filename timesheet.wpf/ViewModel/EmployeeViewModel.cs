using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using timesheet.core.Base;
using timesheet.core.Singleton;
using timesheet.data.Models;
using timesheet.data.Services;
using timesheet.services.Services;
using timesheet.wpf.Commands;

namespace timesheet.wpf.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        private EmployeeService _employeeService;
        private TimeSheetEntryService _timeSheetService;
        private TaskService _taskService;


        public EmployeeViewModel(MainViewModel mainViewModel)
        {
            mParentViewModel = mainViewModel;
            SubmitCommand = new DelegateCommand(() => ShowEditView());
            _employeeService = (EmployeeService)SingletonInstances.GetService(typeof(EmployeeService));
            _timeSheetService = (TimeSheetEntryService)SingletonInstances.GetService(typeof(TimeSheetEntryService));
            _taskService = (TaskService)SingletonInstances.GetService(typeof(TaskService));
            Task.Run(new Action(OnLoaded));
        }

        private void ShowEditView()
        {
            mParentViewModel.CurrentViewModel = new TimeSheetViewModel(mParentViewModel, _employeeService, _timeSheetService, _taskService);
        }

        private async void OnLoaded()
        {
            await Load();
        }

        private async Task Load()
        {
            var employeeList = await this._employeeService.GetEmployees();
            var effortList = await this._timeSheetService.GetAllTimeSheetData();
            var weekBegin = DateTimeHelpers.DateExtensions.GetStartOfWeek(DateTime.Today, DayOfWeek.Sunday);
            var thisWeekEfforts = effortList.Where(a => a.RecordDate >= weekBegin && a.RecordDate <= DateTime.Today);
            foreach (var employee in employeeList)
            {
                try
                {
                    var thisWeekEffort = thisWeekEfforts.Where(a => a.EmployeeCode == employee.Code).Sum(p => p.TaskHours);
                    var avgTotalEffort = effortList.Where(a => a.EmployeeCode == employee.Code).Average(p => p.TaskHours);
                    if (double.IsInfinity(avgTotalEffort) || double.IsNaN(avgTotalEffort))
                        avgTotalEffort = 0;
                    employee.WeeklyTotalEffort = thisWeekEffort;
                    employee.AverageEffort = avgTotalEffort;
                }
                catch
                {
                    continue;
                }
            }

            Employees = new ObservableCollection<Employee>(employeeList);
        }

        private MainViewModel mParentViewModel;

        public ICommand SubmitCommand { get; set; }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                NotifyPropertyChanged("Employees");
            }
        }

    }
}
