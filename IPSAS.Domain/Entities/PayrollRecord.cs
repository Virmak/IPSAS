using System.ComponentModel.DataAnnotations.Schema;

namespace IPSAS.Domain.Entities
{
    public class PayrollRecord
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int PayrollId { get; set; }
        public MonthlyPayroll Payroll { get; set; }
        public double Rate { get; set; }
        public int HoursCount { get; set; }
        public double Retenu { get; set; }
        [NotMapped]
        public double GrossPay { 
            get
            {
                return HoursCount * Rate;
            }
        }
        [NotMapped]
        public double Net { 
            get
            {
                return HoursCount * Rate * (1 - Retenu);
            }
        }
    }
}
