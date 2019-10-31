using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class PayrollViewModel
    {
        private IPSASDbContext context;
        private IList<PayrollRecordViewModel> _payrollRecords;
        private MonthlyPayroll _payroll = null;
        private ObservableCollection<string> _academicYears;
        private ObservableCollection<string> _months;
        private int _selectedMonth;
        private string _selectedAcademicYear;


        public PayrollViewModel(IPSASDbContext context)
        {
            this.context = context;
            Init();
        
        }

        public void Init()
        {
            var currentMonth = DateTime.Today.Month;
            var currentYear = DateTime.Today.Year;
            string academicYear;

            if (currentMonth < 9)
            {
                academicYear = (currentYear - 1) + "/" + currentYear;
            }
            else
            {
                academicYear = currentYear + "/" + (currentYear + 1);
            }


            var payroll = context.MonthlyPayrolls
                .Where(p => p.Month == currentMonth && p.AcademicYear == academicYear)
                .Include(p => p.Records)
                .ThenInclude(r => (r as PayrollRecord).Teacher)
                .FirstOrDefault();

            if (payroll == null)
            {
                payroll = new MonthlyPayroll
                {
                    Month = currentMonth,
                    AcademicYear = academicYear
                };

                payroll.Records = new List<PayrollRecord>();
                foreach (var teacher in context.Teachers)
                {
                    var record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
                    payroll.Records.Add(record);
                    context.PayrollRecords.Add(record);
                }
                context.MonthlyPayrolls.Add(payroll);
            }

            _payroll = payroll;
            

            _academicYears = new ObservableCollection<string>();
            for (var i = 2019; i < currentYear + 4; i++)
            {
                _academicYears.Add(i + "/" + (i + 1));
            }

            _selectedAcademicYear = "2019/2020";
            if (currentMonth >= 9)
            {
                _selectedAcademicYear = currentYear + "/" + (currentYear + 1);
            } 
            else
            {
                _selectedAcademicYear = (currentYear - 1) + "/" + currentYear;
            }
            _months = new ObservableCollection<string>(new string[] { "", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" });
            _selectedMonth = currentMonth;

            context.SaveChanges();


        }

        internal void AddTeacherRecord(string cin)
        {
            var teacher = context.Teachers.FirstOrDefault(t => t.CIN == cin);
            var record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
            _payroll.Records.Add(record);
            context.PayrollRecords.Add(record);
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            var r = _payroll.Records;

            MessageBox.Show("save edit");
        }


        public ObservableCollection<string> Months
        {
            get
            {
                return _months;
            }
            set
            {
                if (value != _months)
                {
                    _months = value;
                }
            }
        }


        public ObservableCollection<string> AcademicYears { 
            get
            {
                return _academicYears;
            }
            set
            {
                _academicYears = value;
            }
        }

        public IList<PayrollRecordViewModel> PayrollRecords
        {
            get
            {
                return _payroll.Records.Select(r => PayrollRecordViewModel.FromPayrollRecord(r)).ToList();
            }
            set
            {
                throw new Exception("Cannot set payrollrecords");
                /*
                if (_payrollRecords != value)
                {
                    _payroll.Records = value;
                }*/
            }
        }

        public MonthlyPayroll SelectedPayroll {
            get
            {
                return _payroll;
            }
            set 
            { 
                if (value != _payroll)
                {
                    _payroll = value;
                }
            } 
        }

        public int SelectedMonth 
        {
            get
            {
                return _selectedMonth;
            }
            set
            {
                if (value != _selectedMonth)
                {
                    _selectedMonth = value;
                    SelectedPayroll = context.MonthlyPayrolls.SingleOrDefault(p => p.Month == _selectedMonth && p.AcademicYear == _selectedAcademicYear);
                }
            }
        }
        public string SelectedAcademicYear 
        {
            get
            {
                return _selectedAcademicYear;
            }
            set
            {
                if (value != _selectedAcademicYear)
                {
                    _selectedAcademicYear = value;
                    SelectedPayroll = context.MonthlyPayrolls.SingleOrDefault(p => p.Month == _selectedMonth && p.AcademicYear == _selectedAcademicYear);
                }
            }
        }
    }
}
