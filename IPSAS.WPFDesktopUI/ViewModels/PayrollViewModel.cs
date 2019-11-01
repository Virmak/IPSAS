using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using IPSAS.WPFDesktopUI.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class PayrollViewModel : INotifyPropertyChanged
    {
        private IPSASDbContext context;
        private IList<PayrollRecordViewModel> _payrollRecords;
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
            var payroll = context.MonthlyPayrolls
                .Where(p => p.Month == month && p.AcademicYear == year)
                .Include(p => p.Records)
                .ThenInclude(r => (r as PayrollRecord).Teacher)
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

        public void AddTeacherPayrollRecord(Teacher teacher)
        {
            var x = _payroll.Records.FirstOrDefault(r => r.TeacherId == teacher.Id);

            var record = new PayrollRecord { Teacher = teacher, HoursCount = 0, Rate = teacher.Rate, Payroll = _payroll, Retenu = 0.15 };
            _payroll.Records.Add(record);
            if (_payroll.Records.FirstOrDefault(r => r.TeacherId == teacher.Id) == null)
            {
                context.PayrollRecords.Add(record);
                context.SaveChanges();
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
            }

            context.SaveChanges();


            MessageBox.Show("Fiche de pointage enregistré");
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

        public IList<PayrollRecordViewModel> PayrollRecords
        {
            get
            {
                if (_payrollRecords == null)
                {
                    _payrollRecords = _payroll.Records.Select(r => PayrollRecordViewModel.FromPayrollRecord(r)).ToList();
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
