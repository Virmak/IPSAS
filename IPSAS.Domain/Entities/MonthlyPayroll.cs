using System.Collections.Generic;

namespace IPSAS.Domain.Entities
{
    public class MonthlyPayroll
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public string AcademicYear { get; set; }
        public ICollection<PayrollRecord> Records { get; set; }
    }
}
