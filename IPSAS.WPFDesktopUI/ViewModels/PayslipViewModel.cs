using IPSAS.Application.UseCases;
using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class PayslipViewModel : INotifyPropertyChanged
    {
        private readonly IPSASDbContext _dbContext;
        public event PropertyChangedEventHandler PropertyChanged;
        private Teacher _selectedTeacher;
        private ObservableCollection<Teacher> _teachers;
        private ObservableCollection<Teacher> _allTeachers;
        private List<string> _monthsList;
        private List<string> _yearsList;
        private List<string> _paymentTypesList;
        private List<string> _gradesList;
        private List<string> _statusList;
        private List<string> _contractTypesList;
        private int _selectedMonth;
        private int _selectedYear;
        private MonthlyPayroll _payroll;
        private Payslip _payslip;
        private PayrollRecord _selectedPayrollRecord;

        private double _grossSalary;
        private int _hoursCount;
        private double _rate;
        private double _netSalary;
        private int _paymentType = -1;
        private DateTime _paymentDate = DateTime.Now;
        private string _bank;
        private string _paymentDetails;

        /** Search or Selected Teacher data**/
        private string _matricule;
        private string _firstName;
        private string _lastName;
        private int _grade = -1;
        private int _status = -1;
        private int _contractType = -1;
        private ICommand _resetSearchCommand;
        private ICommand _searchCommand;
        private ICommand _savePayslip;
        private ICommand _deletePayslipCommand;
        private ICommand _printPayslipCommand;

        public List<string> Months => _monthsList;
        public List<string> Years => _yearsList;
        public List<string> PaymentTypes => _paymentTypesList;
        public List<string> GradesList => _gradesList;
        public List<string> StatusList => _statusList;
        public List<string> ContractTypes => _contractTypesList;
        

        public PayslipViewModel(IPSASDbContext dbContext)
        {
            _dbContext = dbContext;
            Init();
        }

        public void Init()
        {
            _allTeachers = new ObservableCollection<Teacher>(_dbContext.Teachers.Include(t => t.Payslips));
            _teachers = _allTeachers;
            _selectedMonth = DateTime.Now.Month;
            _selectedYear = 0;

            _monthsList = new List<string>(new string[] { "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
                "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre" });
            _yearsList = new List<string>();
            for (var i = 2019; i < DateTime.Now.Year + 4; i++)
            {
                _yearsList.Add(i + "/" + (i + 1));
            }



            _payroll = _dbContext.MonthlyPayrolls.FirstOrDefault(p => p.Month == _selectedMonth && p.AcademicYear == _yearsList[_selectedYear]);

            _paymentTypesList = new List<string>(new string[] { "Espèces", "Chèque", "Virement bancaire" });

            _gradesList = Enum.GetNames(typeof(TeacherGrade)).ToList();
            _statusList = Enum.GetNames(typeof(TeacherStatus)).ToList();
            _contractTypesList = Enum.GetNames(typeof(ContractType)).ToList();


        }

        private void FilterTeachers()
        {
            if (!_teacherFilterEnabled)
            {
                return;
            }

            if (_matricule == "" && _firstName == "" && _lastName == ""
                && _grade == -1 && _status == -1 && _contractType == -1)
            {
                _teachers = _allTeachers;
            } 
            else
            {
                _teachers = new ObservableCollection<Teacher>(_allTeachers.Where(t =>
                {
                    var matTest = true;
                    var firstNTest = true;
                    var lastNTest = true;
                    var gradeTest = true;
                    var statusTest = true;
                    var contractTest = true;

                    if (!String.IsNullOrEmpty(_matricule))
                    {
                        matTest = t.Id.ToString().Contains(_matricule);
                    }
                    if (!String.IsNullOrEmpty(_firstName))
                    {
                        firstNTest = t.FirstName.ToLower().Contains(_firstName.ToLower());
                    }
                    if (!String.IsNullOrEmpty(_lastName))
                    {
                        lastNTest = t.LastName.ToLower().Contains(_lastName.ToLower());
                    }
                    if (_grade > -1)
                    {
                        gradeTest = t.Grade == (TeacherGrade)_grade;
                    }
                    if (_status > -1)
                    {
                        statusTest = t.Status == (TeacherStatus)_status;
                    }
                    if (_contractType > -1)
                    {
                        contractTest = t.ContractType == (ContractType)_contractType;
                    }
                    return matTest && firstNTest && lastNTest && gradeTest && statusTest && contractTest;
                }).ToList());
            }
           
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Teachers)));
        }

        public ICommand ResetSearchCommand => _resetSearchCommand ?? (_resetSearchCommand = new CommandHandler(
                    () =>
                    {
                        _teacherFilterEnabled = false;
                        Matricule = "";
                        FirstName = "";
                        LastName = "";
                        Grade = -1;
                        Status = -1;
                        ContractType = -1;
                        SelectedTeacher = null;
                        
                        ResetPayslipFields();
                        _teacherFilterEnabled = true;
                        FilterTeachers();
                        return true;
                    },
                    () => true ));
        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new CommandHandler(
                () =>
                {
                    FilterTeachers();
                    return true;
                },
                () => true ));
        public ICommand SavePayslipCommand => _savePayslip ?? (_savePayslip = new CommandHandler(
                () =>
                {
                    if (_selectedTeacher == null)
                    {
                        MessageBox.Show("Veuillez sélectionner un enseignant", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                    if (_paymentType != (int)Domain.Entities.PaymentType.Cash && _bank == "" && _paymentDetails == "")
                    {
                        MessageBox.Show("Veuillez entrer la référence / numéro du chèque", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }

                    var payslip = _dbContext.Payslips.FirstOrDefault(p => p.Teacher.Id == _selectedTeacher.Id && p.MonthlyPayroll.Id == _payroll.Id);

                    if (payslip == null)
                    {
                        var payment = new Payment { Bank = Bank, PaymentDate = PaymentDate, 
                            Amount = _netSalary, PaymentType = (PaymentType)PaymentType, Reference = PaymentDetails };

                        payslip = new Payslip
                        {
                            Teacher = _selectedTeacher,
                            MonthlyPayroll = _selectedPayrollRecord.Payroll,
                            Payment = payment,
                        };

                        _dbContext.Payslips.Add(payslip);
                    } 
                    else
                    {
                        payslip.Payment.PaymentDate = PaymentDate;
                        payslip.Payment.Bank = Bank;
                        payslip.Payment.Amount = _netSalary;
                        payslip.Payment.PaymentType = (PaymentType)PaymentType;
                        payslip.Payment.Reference = PaymentDetails;
                    }


                    _selectedPayrollRecord.HoursCount = _hoursCount;
                    GrossPay = _selectedPayrollRecord.GrossPay;
                    NetSalary = _selectedPayrollRecord.Net;

                    _dbContext.Entry(_selectedPayrollRecord).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    MessageBox.Show("Fiche de de paie enregistré avec succès", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    return true;
                },
                () => true ));

        public ICommand DeletePayslipCommand => _deletePayslipCommand ?? (_deletePayslipCommand = new CommandHandler(
                    () =>
                    {
                        var response = MessageBox.Show(
                            "Voulez-vous vraiment supprimer la fiche de la paie de  " + _selectedTeacher.FullName, 
                            "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) ;
                        if (response == MessageBoxResult.Yes)
                        {
                            var payslip = _dbContext.Payslips.FirstOrDefault(p => p.Teacher.Id == _selectedTeacher.Id && p.MonthlyPayroll.Id == _payroll.Id);
                            if (payslip != null)
                            {
                                DeletePayslip(payslip);
                                ResetPayslipFields();
                            }
                        }

                        return true;
                    },
                    () => true));
        public ICommand PrintPayslipCommand => _printPayslipCommand ?? (_printPayslipCommand = new CommandHandler(
                    () =>
                    {
                        if (_selectedTeacher == null)
                        {
                            MessageBox.Show("Veuillez sélectinner un enseignant");
                            return false;
                        } else if (_payslip == null || _selectedPayrollRecord == null)
                        {
                            MessageBox.Show("Veuillez enregistrer la fiche de paie avant de l'imprimmer");
                            return false;
                        }
                        PayslipPdfGenerator payslipPdf = new PayslipPdfGenerator();
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "Fichier PDF|*.pdf";
                        saveFileDialog1.Title = "Emplacement";
                        saveFileDialog1.ShowDialog();

                        // If the file name is not an empty string open it for saving.
                        if (saveFileDialog1.FileName != "")
                        {
                            try
                            {
                                payslipPdf.Generate(_selectedTeacher, _payslip, _selectedPayrollRecord, saveFileDialog1.FileName);
                            } 
                            catch(Exception e)
                            {
                                MessageBox.Show("Veuillez fermer le fichier PDF et réessayez");
                            } 
                        }


                        return true;
                    },
        () => true));

        public void DeletePayslip(Payslip payslip)
        {
            _dbContext.Remove(payslip);
            _dbContext.SaveChanges();
        }

        internal void AddTeacher(Teacher teacher)
        {
            _allTeachers.Add(teacher);
        }

        public Payslip Payslip
        {
            get
            {
                return _payslip;
            }
            set
            {
                if (value != _payslip)
                {
                    _payslip = value;

                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Payslip)));
            }
        }
        public double GrossPay 
        { 
            get
            {
                return _grossSalary;
            }
            set 
            {
                if (value != _grossSalary)
                {
                    _grossSalary = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(GrossPay)));
                }
            }
        }
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
                    if (_selectedTeacher.Status == TeacherStatus.Vacataire)
                    {
                        GrossPay = _selectedTeacher.Rate * _hoursCount;
                        NetSalary = GrossPay * 0.85;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(HoursCount)));
                }
            }
        }

        public bool HoursCountEnabled 
        {
            get
            {
                return _selectedTeacher?.Status == TeacherStatus.Vacataire;
            }
        }
        public double HourlyRate 
        {
            get
            {
                return _rate;
            }
            set 
            {
                if (value != _rate)
                {
                    _rate = value;
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HourlyRate)));
            }
        }
        public double NetSalary
        {
            get
            {
                return _netSalary;
            }
            set 
            {
                if (value != _netSalary)
                {
                    _netSalary = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NetSalary)));
                }
            }
        }

        public int PaymentType
        {
            get => (int)_paymentType;
            set
            {
                if (value != (int)_paymentType)
                {
                    _paymentType = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PaymentType)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PaymentRefName)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PaymentDetailsVisibility)));

                    if (_paymentType == (int)Domain.Entities.PaymentType.Cash)
                    {
                        PaymentDetails = null;
                        Bank = null;
                    }
                }
            }
        }
        public string PaymentDetails 
        {
            get
            {
                return _paymentDetails;
            }
            set
            {
                if (_paymentDetails != value)
                {
                    _paymentDetails = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PaymentDetails)));

                }
            }
        }
        public DateTime PaymentDate 
        {
            get
            {
                return _paymentDate;
            }
            set
            {
                if (value != _paymentDate)
                {
                    _paymentDate = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(PaymentDate)));
                }
            }
        }

        public string Bank 
        {
            get
            {
                return _bank;
            }
            set
            {
                if (value != _bank)
                {
                    _bank = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Bank)));
                }
            }
        }

        public string PaymentRefName 
        { 
            get
            {
                return PaymentType == (int)Domain.Entities.PaymentType.Check ? "N° du chèque" : "Référence";
            }
            set { }
        }

        public Visibility PaymentDetailsVisibility
        {
            get
            {
                return _paymentType > (int)Domain.Entities.PaymentType.Cash ? Visibility.Visible : Visibility.Collapsed;
            }
            set { }
        }

        private bool _teacherFilterEnabled = true;

        public string Matricule 
        {
            get
            {
                return _matricule;
            }
            set
            {
                if (_matricule != value)
                {
                    _matricule = value;
                    FilterTeachers();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Matricule)));
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
                    FilterTeachers();
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
                    FilterTeachers();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastName)));
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
                if (value != _grade)
                {
                    _grade = value;
                    FilterTeachers();
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
                if (value != _status)
                {
                    _status = value;
                    FilterTeachers();
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
                if (value != _contractType)
                {
                    _contractType = value;
                    FilterTeachers();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContractType)));
                }
            }
        }

        public bool ContractTypeEnabled
        {
            get
            {
                if (_selectedTeacher == null)
                {
                    return true;
                } 
                else
                {
                    return _selectedTeacher.Status == TeacherStatus.Permanent;
                }
            }
            set
            {

            }
        }

        internal void UpdatePayrollRecord(PayrollRecord record)
        {
            if (_selectedPayrollRecord.Id == record.Id)
            {
                _selectedPayrollRecord = record;

                GrossPay = _selectedPayrollRecord.GrossPay;
                NetSalary = _selectedPayrollRecord.Net;
                HoursCount = _selectedPayrollRecord.HoursCount;

            }
        }

        public int SelectedMonth { 
            get
            {
                return _selectedMonth - 1;
            }
            set
            { 
                if (_selectedMonth != value)
                {
                    _selectedMonth = value + 1;

                    _payroll = _dbContext.MonthlyPayrolls.FirstOrDefault(p => p.Month == _selectedMonth && p.AcademicYear == _yearsList[_selectedYear]);

                    if (_payroll == null)
                    {
                        ResetPayslipFields();
                    }
                    else
                    {
                        _selectedPayrollRecord = _selectedTeacher?.PayrollRecords
                            .FirstOrDefault(pr => pr.Payroll.Month == _selectedMonth && pr.Payroll.AcademicYear == _yearsList[_selectedYear]);
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedMonth)));
                }
            }
        }

        public int SelectedYear 
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;

                    _payroll = _dbContext.MonthlyPayrolls.FirstOrDefault(p => p.Month == _selectedMonth && p.AcademicYear == _yearsList[_selectedYear]);

                    if (_payroll == null)
                    {
                        ResetPayslipFields();
                    }

                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedYear)));
                }
            }
        }

        public ObservableCollection<Teacher> Teachers
        {
            get
            {
                return _teachers;
            }
            set
            {
                if (value != _teachers)
                {
                    _teachers = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Teachers)));
                }
            }
        }

        public Teacher SelectedTeacher 
        {   get
            {
                return _selectedTeacher;
            }
            set
            {
                if (value != _selectedTeacher)
                {
                    _selectedTeacher = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedTeacher)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ContractTypeEnabled)));

                    if (_selectedTeacher == null)
                    {
                        return;
                    }
                    
                    if (_selectedTeacher.Payslips != null)
                    {
                        Payslip = _dbContext.Payslips.Include(p => p.Payment)
                            .FirstOrDefault(p => p.Teacher.Id == _selectedTeacher.Id
                                && p.MonthlyPayroll.Month == _selectedMonth && p.MonthlyPayroll.AcademicYear == _yearsList[_selectedYear]);
                       
                        if (Payslip != null && Payslip.Payment != null)
                        {
                            PaymentType = Payslip.Payment != null ? (int)Payslip.Payment.PaymentType : -1;
                            PaymentDate = Payslip.Payment.PaymentDate;
                            PaymentDetails = Payslip.Payment.Reference;
                            Bank = Payslip.Payment.Bank;
                        } else
                        {
                            ResetPayslipFields();
                        }
                    } 
                    else
                    {
                        ResetPayslipFields();
                    }


                    if (_payroll != null)
                    {
                        _selectedPayrollRecord = _dbContext.PayrollRecords
                            .FirstOrDefault(pr => pr.PayrollId == _payroll.Id && pr.TeacherId == _selectedTeacher.Id);
                        if (_selectedPayrollRecord == null)
                        {
                            _selectedPayrollRecord = new PayrollRecord()
                            {
                                Teacher = _selectedTeacher,
                                HoursCount = 0,
                                Payroll = _payroll,
                                PayrollId = _payroll.Id,
                                TeacherId = _selectedTeacher.Id
                            };
                        }
                    } 
                    else
                    {
                        var payrollVM = App.ServiceProvider.GetService<PayrollViewModel>();
                        payrollVM.CreatePayroll(_selectedMonth, _yearsList[_selectedYear]);
                        _payroll = payrollVM.SelectedPayroll;
                    }

                    PaymentType = Payslip != null && Payslip.Payment != null ? (int)Payslip.Payment.PaymentType : -1;
                    HourlyRate = _selectedTeacher.Rate;

                    if (_selectedPayrollRecord != null)
                    {
                        GrossPay = _selectedPayrollRecord.GrossPay;
                        NetSalary = _selectedPayrollRecord.Net;
                        HoursCount = _selectedPayrollRecord.HoursCount;
                    }
                    else
                    {
                        GrossPay = _selectedTeacher.GrossPay;
                    }

                    _teacherFilterEnabled = false;

                    Matricule = _selectedTeacher.Id.ToString();
                    FirstName = _selectedTeacher.FirstName;
                    LastName = _selectedTeacher.LastName;
                    Grade = (int)_selectedTeacher.Grade;
                    Status = (int)_selectedTeacher.Status;

                    if (_selectedTeacher.Status == TeacherStatus.Vacataire)
                    {
                        ContractType = -1;
                    } 
                    else
                    {
                        ContractType = (int)_selectedTeacher.ContractType;
                    }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(HoursCountEnabled)));
                    
                    _teacherFilterEnabled = true;
                }
            }
        }
        public void ResetPayslipFields()
        {
            HoursCount = 0;
            HourlyRate = 0;
            GrossPay = 0;
            PaymentDate = DateTime.Now;
            PaymentType = -1;
            PaymentDetails = "";
            Bank = "";
            NetSalary = 0;
        }
    }
}
