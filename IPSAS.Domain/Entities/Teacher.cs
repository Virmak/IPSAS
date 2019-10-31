using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPSAS.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string CIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string HomeInstitution { get; set; }
        public string Speciality { get; set; }
        public TeacherGrade Grade { get; set; }
        public TeacherStatus Status { get; set; }
        public ContractType ContractType { get; set; }
        public ICollection<PayrollRecord> PayrollRecords { get; set; }
        [NotMapped]
        public double Rate
        {
            get
            {
                switch (Grade)
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
        }
        public string FullName { get { return FirstName + " " + LastName; } }
    }

    public enum TeacherGrade
    {
        Docteur,
        Technologue,
        MaitreTechnologue,
        Engineer,
        PES
    }

    public enum TeacherStatus
    {
        Permanent,
        Vacataire
    }
    
    public enum ContractType
    {
        CDI,
        CDD
    }


}
