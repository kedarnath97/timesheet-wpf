using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using timesheet.core.Base;

namespace timesheet.wpf.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            CurrentViewModel = new EmployeeViewModel(this);
        }

        private BaseViewModel mCurrentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return mCurrentViewModel; }
            set
            {
                mCurrentViewModel = value;
                NotifyPropertyChanged("CurrentViewModel");
            }
        }
    }
}
