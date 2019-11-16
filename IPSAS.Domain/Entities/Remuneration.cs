using System;
using System.Collections.Generic;
using System.Text;

namespace IPSAS.Domain.Entities
{
    public class Remuneration
    {
        public int Id { get; set; }
        public TeacherGrade Grade { get; set; }
        public double HourlyRate { get; set; }
        public double Salary { get; set; }
    }
}
