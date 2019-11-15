using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace IPSAS.Domain.Entities
{
    public class Payslip
    {
        public int Id { get; set; }
        public MonthlyPayroll MonthlyPayroll { get; set; }
        public Teacher Teacher { get; set; }
        public Payment Payment { get; set; }

        [NotMapped] 
        public int HoursCount 
        {
            get
            {
                if (Teacher.Status == TeacherStatus.Permanent)
                {
                    return 0;
                }
                else 
                {
                    var payrollRecord = MonthlyPayroll.Records.FirstOrDefault(pr => pr.TeacherId == Teacher.Id);
                    return payrollRecord != null ? payrollRecord.HoursCount : 0;
                }
            }
            set { }
        }
    }

    
}
