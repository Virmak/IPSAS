using IPSAS.WPFDesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IPSAS.WPFDesktopUI.Types
{
    class PayrollRecordList : ObservableCollection<PayrollRecordViewModel>
    {
        public PayrollRecordList() : base()
        {

        }
    }
}
