using System;
using System.Collections.Generic;
using System.Text;

namespace IPSAS.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public int Matricule { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string HomeInstitution { get; set; }
        public TeacherGrade Grade { get; set; }
        public TeacherSpeciality Speciality { get; set; }

    }
}
