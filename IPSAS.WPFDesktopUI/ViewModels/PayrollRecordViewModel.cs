using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public PayrollRecord Record { get; set; }
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
                    if (PropertyChanged != null)
                    {
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(GrossPay)));
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Net)));
                    }
                    App.ServiceProvider.GetService<PayrollViewModel>().UpdatePayrollRecord(this);
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
        }
        public double Net
        {
            get
            {
                return HoursCount * Rate * (1 - Retenu);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SaveRecord()
        {
            var context = new IPSASDbContext();
            context.Attach(Record);
            context.Entry(Record).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        public static PayrollRecordViewModel FromPayrollRecord(PayrollRecord record)
        {
            return new PayrollRecordViewModel
            {
                Id = record.Id,
                Teacher = record.Teacher,
                Payroll = record.Payroll,
                Rate = record.Rate,
                HoursCount = record.HoursCount,
                Retenu = record.Retenu, 
                Record = record
            };
        }
    }
}