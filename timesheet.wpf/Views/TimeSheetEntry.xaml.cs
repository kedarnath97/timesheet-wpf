using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using timesheet.wpf.ViewModel;

namespace timesheet.wpf.Views
{
    /// <summary>
    /// Interaction logic for TimeSheetEntry.xaml
    /// </summary>
    public partial class TimeSheetEntry : UserControl
    {
        public TimeSheetEntry()
        {
            InitializeComponent();
        }

        private void CellChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
            var dataContext = this.DataContext as TimeSheetViewModel;
            if (dataContext == null)
                return;
            dataContext.CalculateTotalRecordData();
            TotalGrid.UpdateLayout();
        }
    }
}
