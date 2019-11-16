using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IPSASDbContext context;
        private double _hourlyRate;
        private double _salary;
        private int _selectedGrade;
        private ICommand _saveRenumeration;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsViewModel(IPSASDbContext context)
        {
            this.context = context;
            Init();
        }

        public void Init()
        {
            StatusList = new ObservableCollection<string>(Enum.GetNames(typeof(TeacherStatus)).ToList());
            GradesList = new ObservableCollection<string>(Enum.GetNames(typeof(TeacherGrade)).Select(t => {
                /** Add spaces between words **/
                for (var i = 1; i < t.Length; i++)
                {
                    if (t[i - 1] >= 'a' && t[i - 1] <= 'z' && t[i] >= 'A' && t[i] <= 'Z')
                    {
                        t = t.Insert(i, " ");
                    }
                }
                return t;
            }).ToList());
        }
        public ICommand SaveRenumerationCommand => _saveRenumeration ?? new CommandHandler(() =>
        {
            var remuneration = context.Remunerations.FirstOrDefault(r => r.Grade == (TeacherGrade)_selectedGrade);
            if (remuneration == null)
            {
                remuneration = new Remuneration() { Grade = (TeacherGrade)_selectedGrade, HourlyRate = _hourlyRate, Salary = _salary };
                context.Remunerations.Add(remuneration);
            }
            context.SaveChanges();
            return true;
        }, () => true);

        public ObservableCollection<string> StatusList { get; set; }
        public ObservableCollection<string> GradesList { get; set; }

        public int SelectedStatus { get; set; }

        public int SelectedGrade 
        {
            get
            {
                return _selectedGrade;
            }
            set
            {
                if (value != _selectedGrade)
                {
                    _selectedGrade = value;
                    var remuneration = context.Remunerations.FirstOrDefault(r => r.Grade == (TeacherGrade)_selectedGrade);
                    
                    if (remuneration != null)
                    {
                        HourlyRate = remuneration.HourlyRate;
                        Salary = remuneration.Salary;
                    }

                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedGrade)));
                }
            }
        }

        public double HourlyRate
        {
            get
            {
                return _hourlyRate;
            }
            set
            {
                if (value != _hourlyRate)
                {
                    _hourlyRate = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(HourlyRate)));
                }
            }
        }

        public double Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                if (value != _salary)
                {
                    _salary = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Salary)));
                }
            }
        }
    }
}
