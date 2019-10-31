using IPSAS.Domain.Entities;
using IPSAS.Persistence;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace IPSAS.WPFDesktopUI.ViewModels
{
    public class TeacherViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string CIN { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string HomeInstitution { get; set; }
        public string Speciality { get; set; }
        public TeacherGrade Grade { get; set; }
        public TeacherStatus Status { get; set; }
        public ContractType ContractType { get; set; }


        public double Tarif
        {
            get
            {
                switch(Grade)
                {
                    case TeacherGrade.Docteur:
                        return 60;
                    case TeacherGrade.Engineer:
                        return 50;
                    case TeacherGrade.MaitreTechnologue:
                        return 45;
                    case TeacherGrade.Technologue:
                        return 40;
                }
                return 0;
            }
            set
            {

            }
        }


        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public static TeacherViewModel FromTeacher(Teacher teacher)
        {
            return new TeacherViewModel()
            {
                Id = teacher.Id,
                CIN = teacher.CIN,
                FullName = teacher.FirstName + " " + teacher.LastName,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Address = teacher.Address,
                Phone = teacher.Phone,
                HomeInstitution = teacher.HomeInstitution,
                Grade = teacher.Grade,
                Speciality = teacher.Speciality,
                Status = teacher.Status,
                ContractType = teacher.ContractType
            };
        }
    }
}
