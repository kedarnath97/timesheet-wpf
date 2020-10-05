using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using timesheet.core.Base;
using timesheet.data.Models;
using timesheet.data.Services;
using timesheet.services.Services;
using timesheet.wpf.Commands;
using timesheet.wpf.DateTimeHelpers;
using timesheet.wpf.Transposed;

namespace timesheet.wpf.ViewModel
{
    public class TimeSheetViewModel : BaseViewModel
    {
        private MainViewModel _parentViewModel;
        private EmployeeService _employeeService;
        private TimeSheetEntryService _timeSheetEntryService;
        private TaskService _taskService;
        private List<TimeSheetRecord> mTimeSheetRawList;
        private List<TaskModel> _taskList;
        private DateTime _beginDay;
        private DateTime _forDay = DateTime.Today;
        public TimeSheetViewModel(MainViewModel parentViewModel, EmployeeService employeeService, TimeSheetEntryService timeSheetEntryService, TaskService taskService)
        {
            this._parentViewModel = parentViewModel;
            this._employeeService = employeeService;
            this._timeSheetEntryService = timeSheetEntryService;
            this._taskService = taskService;

            AddNewRecordCommand = new DelegateCommand(AddRecord);
            SaveAllCommand = new DelegateCommand(SaveAllRecords);
            PrevWeekNavigationCommand = new DelegateCommand(new Action(() => { NavigateWeek("Previous"); }));
            NextWeekNavigationCommand = new DelegateCommand(new Action(() => { NavigateWeek("Next"); }));
            GoBackCommand = new DelegateCommand(GotoEmployeeView);
            _ = Task.Factory.StartNew(() => { LoadEmployeeData(); });
            _ = Task.Factory.StartNew(() => { GetTaskDetails(); });
        }

        private void GotoEmployeeView()
        {
            _parentViewModel.CurrentViewModel = new EmployeeViewModel(_parentViewModel);
        }

        private List<Employee> mEmployeeList;
        public List<Employee> EmployeeList
        {
            get { return mEmployeeList; }
            set
            {
                mEmployeeList = value;
                NotifyPropertyChanged("EmployeeList");
            }
        }

        private Employee mSelectedEmployee;
        public Employee SelectedEmployee
        {
            get { return mSelectedEmployee; }
            set
            {
                mSelectedEmployee = value;
                NotifyPropertyChanged("SelectedEmployee");
                LoadEmployeeTaskData();
            }
        }

        private ObservableCollection<TimeSheetTransposed> mTimeSheetDataList;
        public ObservableCollection<TimeSheetTransposed> TimeSheetDataList
        {
            get { return mTimeSheetDataList; }
            set
            {
                mTimeSheetDataList = value;
                NotifyPropertyChanged("TimeSheetDataList");
                CalculateTotalRecordData();
            }
        }

        private ObservableCollection<TimeSheetTransposed> mTotalRecord;
        public ObservableCollection<TimeSheetTransposed> TotalRecord
        {
            get { return mTotalRecord; }
            set
            {
                mTotalRecord = value;
                NotifyPropertyChanged("TotalRecord");
            }
        }

        public ICommand AddNewRecordCommand { get; set; }
        public ICommand SaveAllCommand { get; set; }
        public ICommand PrevWeekNavigationCommand { get; set; }
        public ICommand NextWeekNavigationCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        private void NavigateWeek(string parameter)
        {
            if (parameter == "Previous")
            {
                ForDay = ForDay.AddDays(-7);
            }
            else if (parameter == "Next")
            {
                ForDay = ForDay.AddDays(7);
            }
            Task.Run(LoadEmployeeTaskData);
        }

        private async void SaveAllRecords()
        {
            List<TimeSheetRecord> timeSheetRecords = ConvertBackTransposedData();
            if (timeSheetRecords.Count == 0)
            {
                return;
            }

            var result = await _timeSheetEntryService.AddUpdateTimeSheetEntry(timeSheetRecords);
            if (result)
            {
                LoadEmployeeTaskData();
                MessageBox.Show("Successfully saved/updated records");
            }
        }

        private List<TimeSheetRecord> ConvertBackTransposedData()
        {
            List<TimeSheetRecord> timeSheetRecords = new List<TimeSheetRecord>();
            if (TimeSheetDataList == null || !TimeSheetDataList.Any())
                return timeSheetRecords;
            foreach (var item in TimeSheetDataList)
            {
                var task = item.Task;

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Sunday,
                    RecordDate = GetDate("Sunday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Monday,
                    RecordDate = GetDate("Monday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Tuesday,
                    RecordDate = GetDate("Tuesday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Wednesday,
                    RecordDate = GetDate("Wednesday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Thursday,
                    RecordDate = GetDate("Thursday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Friday,
                    RecordDate = GetDate("Friday")
                });

                timeSheetRecords.Add(new TimeSheetRecord
                {
                    EmployeeCode = SelectedEmployee.Code,
                    TaskId = task.Id,
                    TaskHours = item.Saturday,
                    RecordDate = GetDate("Saturday")
                });
            }
            return timeSheetRecords;
        }

        private DateTime GetDate(string day)
        {
            switch (day)
            {
                case "Sunday": return _beginDay;
                case "Monday": return _beginDay.AddDays(1);
                case "Tuesday": return _beginDay.AddDays(2);
                case "Wednesday": return _beginDay.AddDays(3);
                case "Thursday": return _beginDay.AddDays(4);
                case "Friday": return _beginDay.AddDays(5);
                case "Saturday": return _beginDay.AddDays(6);
                default: return _beginDay;
            }
        }

        private async void GetTaskDetails()
        {
            _taskList = await _taskService.GetAllTasks();
        }

        private void AddRecord()
        {
            TimeSheetDataList = TimeSheetDataList ?? new ObservableCollection<TimeSheetTransposed>();
            TimeSheetDataList.Insert(TimeSheetDataList.Count, new TimeSheetTransposed { TaskList = _taskList });
        }


        private async void LoadEmployeeTaskData()
        {
            if (TimeSheetDataList != null)
            {
                TimeSheetDataList = new ObservableCollection<TimeSheetTransposed>();
            }
            try
            {
                Tuple<DateTime, DateTime> weekDates = DateExtensions.GetWeekDates(ForDay);
                _beginDay = weekDates.Item1;
                var userTaskData = await _timeSheetEntryService.GetTimeSheetDataForPeriod(SelectedEmployee.Code, weekDates.Item1.ToString("yyyyMMdd"), weekDates.Item2.ToString("yyyyMMdd"));
                mTimeSheetRawList = userTaskData;
                if (mTimeSheetRawList != null && mTimeSheetRawList.Any())
                    TransposeToWeeklyFormat();
                CalculateTotalRecordData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TransposeToWeeklyFormat()
        {
            var tempList = new List<TimeSheetTransposed>();
            foreach (var taskId in mTimeSheetRawList.Select(p => p.TaskId).Distinct())
            {
                TimeSheetTransposed timeSheetTransposed = new TimeSheetTransposed
                {
                    Task = _taskList.Where(a => a.Id == taskId).FirstOrDefault(),
                    TaskList = _taskList
                };
                foreach (var item in mTimeSheetRawList.Where(a => a.TaskId == taskId))
                {
                    switch (item.RecordDate.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            timeSheetTransposed.Sunday = item.TaskHours;
                            break;
                        case DayOfWeek.Monday:
                            timeSheetTransposed.Monday = item.TaskHours;
                            break;
                        case DayOfWeek.Tuesday:
                            timeSheetTransposed.Tuesday = item.TaskHours;
                            break;
                        case DayOfWeek.Wednesday:
                            timeSheetTransposed.Wednesday = item.TaskHours;
                            break;
                        case DayOfWeek.Thursday:
                            timeSheetTransposed.Thursday = item.TaskHours;
                            break;
                        case DayOfWeek.Friday:
                            timeSheetTransposed.Friday = item.TaskHours;
                            break;
                        case DayOfWeek.Saturday:
                            timeSheetTransposed.Saturday = item.TaskHours;
                            break;
                        default:
                            break;
                    }
                }
                tempList.Add(timeSheetTransposed);
            }

            TimeSheetDataList = new ObservableCollection<TimeSheetTransposed>(tempList);
        }

        private async void LoadEmployeeData()
        {
            EmployeeList = await _employeeService.GetEmployees();
            SelectedEmployee = EmployeeList == null ? null : EmployeeList.FirstOrDefault();
        }
        public void CalculateTotalRecordData()
        {
            TotalRecord = new ObservableCollection<TimeSheetTransposed>
            {
                new TimeSheetTransposed
                {
                    Task=new TaskModel{ Name="Total",Id=0},
                    Monday=TimeSheetDataList==null || !TimeSheetDataList.Any()?0: TimeSheetDataList.Sum(e=>e.Monday),
                    Tuesday=TimeSheetDataList==null || !TimeSheetDataList.Any()?0: TimeSheetDataList.Sum(e=>e.Tuesday),
                    Wednesday=TimeSheetDataList==null || !TimeSheetDataList.Any()?0: TimeSheetDataList.Sum(e=>e.Wednesday),
                    Thursday=TimeSheetDataList==null || !TimeSheetDataList.Any()?0: TimeSheetDataList.Sum(e=>e.Thursday),
                    Friday=TimeSheetDataList==null || !TimeSheetDataList.Any()? 0:TimeSheetDataList.Sum(e=>e.Friday),
                    Saturday=TimeSheetDataList==null || !TimeSheetDataList.Any()? 0:TimeSheetDataList.Sum(e=>e.Saturday),
                    Sunday=TimeSheetDataList==null || !TimeSheetDataList.Any()? 0:TimeSheetDataList.Sum(e=>e.Sunday),
                }
            };
        }

        public DateTime ForDay
        {
            get { return _forDay; }
            set
            {
                _forDay = value;
                NotifyPropertyChanged("ForDay");
            }
        }

    }
}
