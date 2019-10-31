using IPSAS.Domain.Entities;
using System.ComponentModel;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class PayrollRecordViewModel : INotifyPropertyChanged
    {
        private int _hoursCount;


        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        public MonthlyPayroll Payroll { get; set; }
        public double Rate { get; set; }
        public int HoursCount { 
            get
            {
                return _hoursCount;
            }
            set
            {
                if (value != _hoursCount)
                {
                    _hoursCount = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(GrossPay)));
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Net)));
                }
            }
        }
        public double Retenu { get; set; }

        public double GrossPay
        {
            get
            {
                return HoursCount * Rate;
            }
            set
            {
            }
        }
        public double Net
        {
            get
            {
                return HoursCount * Rate * (1 - Retenu);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static PayrollRecordViewModel FromPayrollRecord(PayrollRecord t)
        {
            return new PayrollRecordViewModel
            {
                Id = t.Id,
                Teacher = t.Teacher,
                Payroll = t.Payroll,
                Rate = t.Rate,
                HoursCount = t.HoursCount,
                Retenu = t.Retenu
            };
        }
    }
}