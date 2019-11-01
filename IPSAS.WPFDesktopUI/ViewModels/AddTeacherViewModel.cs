using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    class AddTeacherViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        private readonly IPSASDbContext dbContext;
        public AddTeacherViewModel(IPSASDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void LoadTeacher(int teacherId)
        {
            var teacher = dbContext.Teachers.Single(t => t.Id == teacherId);
            TeacherId = teacher.Id.ToString();
            CIN = teacher.CIN;
            FirstName = teacher.FirstName;
            LastName = teacher.LastName;
            Address = teacher.Address;
            Phone = teacher.Phone.ToString();
            InitInstitute = teacher.HomeInstitution;
            Speciality = teacher.Speciality;
            Grade = (int)teacher.Grade;
            Status = (int)teacher.Status;

            if (teacher.Status == TeacherStatus.Permanent)
            {
                ContractType = (int)teacher.ContractType;
            }
            
        }

        private ICommand _addTeacherCommand;
        public ICommand AddTeacherCommand
        {
            get
            {
                return _addTeacherCommand ?? (_addTeacherCommand = new CommandHandler(
                () => {

                    if (String.IsNullOrEmpty(_CIN)
                    || String.IsNullOrEmpty(_firstName)
                    || String.IsNullOrEmpty(_lastName)
                    || String.IsNullOrEmpty(_address)
                    || String.IsNullOrEmpty(_phone)
                    || _grade == -1
                    || String.IsNullOrEmpty(_initInstitute)
                    || String.IsNullOrEmpty(_speciality)
                    || _status == -1)
                    {
                        MessageBox.Show("Veuillez saisir toutes les informations", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    if ((TeacherStatus)_status == TeacherStatus.Permanent && _contractType == -1)
                    {
                        MessageBox.Show("Veuillez choisir le type du contrat", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    try
                    {
                        var teacher = new Teacher
                        {
                            CIN = _CIN,
                            FirstName = _firstName,
                            LastName = _lastName,
                            Address = _address,
                            Grade = (TeacherGrade)_grade,
                            Phone = int.Parse(_phone),
                            HomeInstitution = _initInstitute,
                            Speciality = _speciality,
                            Status = (TeacherStatus)_status,
                        };
                        if (teacher.Status == TeacherStatus.Permanent)
                        {
                            teacher.ContractType = (ContractType)_contractType;
                        }

                        try
                        {
                            dbContext.Teachers.Add(teacher);
                            dbContext.SaveChanges();
                            try
                            {

                                App.ServiceProvider.GetService<TeachersListViewModel>().LoadTeachers();
                                App.PayrollViewModel.AddTeacherPayrollRecord(teacher);
                            }
                            catch(Exception e)
                            {
                                MessageBox.Show("error");
                            }
                            MessageBox.Show("Enseignant ajouté");
                        }
                        catch (DbUpdateException e)
                        {
                            dbContext.Teachers.Remove(teacher);
                            MessageBox.Show("Le numéro de la CIN est déja enregistré", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    } 
                    catch(FormatException e)
                    {
                        MessageBox.Show("Veillez entrez un numéro de téléphone valide");
                    }

                    
                    return null;
                }, 
                () => {
                    return true; 
                }));
            }
        }

        private string _CIN;
        private string _teacherId;
        private string _firstName;
        private string _lastName;
        private string _address;
        private string _phone;
        private string _initInstitute;
        private string _speciality;
        private int _grade;
        private int _status = -1;
        private int _contractType = -1;
        private bool _contractTypeEnabled = false;
        public string CIN
        {
            get { return _CIN; }
            set
            {
                if (_CIN != value)
                {
                    _CIN = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(CIN)));
                }
            }
        }
        public string TeacherId
        {
            get
            {
                return _teacherId;
            }
            set
            {
                if (_teacherId != value)
                {
                    _teacherId = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(TeacherId)));
                }
            }
        }
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Address)));
                }
            }
        }
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Phone)));
                }
            }
        }

        public string InitInstitute
        {
            get
            {
                return _initInstitute;
            }
            set
            {
                if (_initInstitute != value)
                {
                    _initInstitute = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(InitInstitute)));
                }
            }
        }


        public string Speciality
        {
            get
            {
                return _speciality;
            }
            set
            {
                if (_speciality != value)
                {
                    _speciality = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Speciality)));
                }
            }
        }

        public int Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                if (_grade != value)
                {
                    _grade = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Grade)));
                }
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    if (_status != (int)TeacherStatus.Permanent)
                    {
                        _contractType = -1;
                        _contractTypeEnabled = false;
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContractType)));
                    } else
                    {
                        _contractTypeEnabled = true;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContractTypeEnabled)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
                }
            }
        }

        public int ContractType
        {
            get
            {
                return _contractType;
            }
            set
            {
                if (_contractType != value)
                {
                    _contractType = value;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContractType)));
            }
        }

        public bool ContractTypeEnabled
        {
            get
            {
                return _contractTypeEnabled;
            }
            set
            {
            }
        }
    }
}
