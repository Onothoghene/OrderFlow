namespace Application.DTOs.Payment
{
    public class PaymentVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal AmountPaid { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }
        public int PaymentOptionId { get; set; }
        public string PaymentOption { get; set; }
    }
}
