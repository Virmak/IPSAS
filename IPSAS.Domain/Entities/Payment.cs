using System;

namespace IPSAS.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string Bank { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Reference { get; set; }
    }
    public enum PaymentType
    {
        Cash,
        Check,
        BankTransfer,
    }
}
