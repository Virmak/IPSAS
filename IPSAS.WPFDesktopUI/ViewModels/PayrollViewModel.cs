using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Z.EntityFramework.Plus;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class PayrollViewModel : INotifyPropertyChanged
    {
        private IPSASDbContext context;
        private ObservableCollection<PayrollRecordViewModel> _payrollRecords;
        private MonthlyPayroll _payroll = null;
        private ObservableCollection<string> _academicYears;
        private ObservableCollection<string> _months;
        private int _selectedMonth;
        private string _selectedAcademicYear;

        public event PropertyChangedEventHandler PropertyChanged;

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
            CreatePayroll(currentMonth, academicYear);

            _academicYears = new ObservableCollection<string>();
            for (var i = 2019; i < currentYear + 4; i++)
            {
                _academicYears.Add(i + "/" + (i + 1));
            }

            _selectedAcademicYear = academicYear;

            _months = new ObservableCollection<string>(new string[] {
                "", "Janvier", "Février", "Mars", "Avril", "Mai", "Juin", 
                "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" });
            _selectedMonth = currentMonth;

            context.SaveChanges();
        }

        public void CreatePayroll(int month, string year)
        {

            MonthlyPayroll payroll;

            var payrollRecords = context.PayrollRecords
                .Where(p => p.Payroll.Month == month && p.Payroll.AcademicYear == year && p.Teacher.Status == TeacherStatus.Vacataire)
                .Include(p => p.Payroll)
                .ToList();

            if (payrollRecords != null && payrollRecords.Count > 0)
            {
                payroll = payrollRecords[0].Payroll;
            }
            else
            {
                payroll = context.MonthlyPayrolls
                    .Where(p => p.Month == month && p.AcademicYear == year)
                    .FirstOrDefault();
                if (payroll == null)
                {
                    payroll = new MonthlyPayroll
                    {
                        Month = month,
                        AcademicYear = year
                    };

                    payroll.Records = new List<PayrollRecord>();
                    context.MonthlyPayrolls.Add(payroll);
                }
            }

            //var payroll = context.MonthlyPayrolls
            //    .Where(p => p.Month == month && p.AcademicYear == year)
            //    .Include(p => p.Records)
            //    .ThenInclude(r => (r as PayrollRecord).Teacher)
            //    .FirstOrDefault();


            foreach (var teacher in context.Teachers.Where(t => t.Status == TeacherStatus.Vacataire).ToList())
            {
                var record = context.PayrollRecords.FirstOrDefault(pr => pr.TeacherId == teacher.Id && pr.PayrollId == payroll.Id);
                if (record == null)
                {
                    record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
                    context.PayrollRecords.Add(record);
                }
                payroll.Records.Add(record);
            }

            _payroll = payroll;
        }

        public void AddTeacherPayrollRecord(object t)
        {
            var teacher = t as Teacher;

            var record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
            if (_payroll.Records == null)
            {
                _payroll.Records = new List<PayrollRecord>();
            }
            _payroll.Records.Add(record);
            if (_payroll.Records.FirstOrDefault(r => r.TeacherId == teacher.Id) == null)
            {
                context.PayrollRecords.Add(record);
                context.SaveChanges();
            }
            
            if (teacher.Status == TeacherStatus.Permanent)
            {
                return;
            }
            if (_payrollRecords == null)
            {
                _payrollRecords = new ObservableCollection<PayrollRecordViewModel>();
            }
            _payrollRecords.Add(PayrollRecordViewModel.FromPayrollRecord(record));
            
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(PayrollRecords)));
            }
        }

        public List<PayrollRecord> CreateMissingPayrollRecords(MonthlyPayroll payroll)
        {
            var records = new List<PayrollRecord>();
            foreach (var teacher in context.Teachers.Where(t => t.Status == TeacherStatus.Vacataire).ToList())
            {
                var record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
                if (!payroll.Records.Select(prVM => prVM.Teacher.Id).Contains(teacher.Id))
                {
                    records.Add(record);
                    context.PayrollRecords.Add(record);
                }
            }
            return records;
        }

        public void UpdatePayrollRecord(PayrollRecordViewModel payrollRecordVM)
        {
            if (_payrollRecords == null) return;
            var pr = _payrollRecords.FirstOrDefault(pr => pr.Id == payrollRecordVM.Id);
            if (pr != null)
            {
                pr.HoursCount = payrollRecordVM.HoursCount;
                pr.Record.HoursCount = payrollRecordVM.HoursCount;
            } 
            else
            {
                _payroll.Records.Add(payrollRecordVM.Record);
                _payrollRecords.Add(payrollRecordVM);
            }
        }

        public void SaveChanges()
        {
            foreach (var recordVM in _payrollRecords.Distinct())
            {
                context.Attach(recordVM.Record);
                context.Entry(recordVM.Record).State = EntityState.Modified;

                App.ServiceProvider.GetService<PayslipViewModel>().UpdatePayrollRecord(recordVM.Record);
            }

            context.SaveChanges();


            MessageBox.Show("Fiche de pointage enregistré");
        }

        public void AddTeacherDisptach(object teacher)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new ParameterizedThreadStart(AddTeacherPayrollRecord), teacher);
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


        public ObservableCollection<string> AcademicYears
        {
            get
            {
                return _academicYears;
            }
            set
            {
                _academicYears = value;
            }
        }

        public ObservableCollection<PayrollRecordViewModel> PayrollRecords
        {
            get
            {
                if (_payrollRecords == null)
                {
                    if (_payroll.Records != null)
                    {
                        _payrollRecords = new ObservableCollection<PayrollRecordViewModel>(
                            _payroll.Records.Select(r => PayrollRecordViewModel.FromPayrollRecord(r)).ToList());
                    }
                }
                return _payrollRecords;
            }
            set
            {
                if (_payrollRecords != value)
                {
                    _payrollRecords = value;
                }

            }
        }

        public MonthlyPayroll SelectedPayroll
        {
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
