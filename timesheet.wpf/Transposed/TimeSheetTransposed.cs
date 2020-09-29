using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Models;
using timesheet.services.Models;

namespace timesheet.wpf.Transposed
{
    public class TimeSheetTransposed : ModelBase
    {
        private TaskModel mTask;

        public TaskModel Task
        {
            get { return mTask; }
            set
            {
                mTask = value;
                NotifyUpdatePropertyHandler("Task");
            }
        }

        public List<TaskModel> TaskList { get; set; }

        private double mSunday;

        public double Sunday
        {
            get { return mSunday; }
            set
            {
                mSunday = value;
                NotifyUpdatePropertyHandler("Sunday");

            }
        }

        private double mMonday;

        public double Monday
        {
            get { return mMonday; }
            set
            {
                mMonday = value;
                NotifyUpdatePropertyHandler("Monday");
            }
        }

        private double mTuesday;

        public double Tuesday
        {
            get { return mTuesday; }
            set { mTuesday = value; NotifyUpdatePropertyHandler("Tuesday"); }
        }
        private double mWednesday;

        public double Wednesday
        {
            get { return mWednesday; }
            set
            {
                mWednesday = value;
                NotifyUpdatePropertyHandler("Wednesday");
            }
        }

        private double mThursday;

        public double Thursday
        {
            get { return mThursday; }
            set
            {
                mThursday = value;
                NotifyUpdatePropertyHandler("Thursday");
            }
        }


        private double mFriday;

        public double Friday
        {
            get { return mFriday; }
            set
            {
                mFriday = value;
                NotifyUpdatePropertyHandler("Friday");
            }
        }

        private double mSaturday;

        public double Saturday
        {
            get { return mSaturday; }
            set
            {
                mSaturday = value;
                NotifyUpdatePropertyHandler("Saturday");
            }
        }
    }
}
