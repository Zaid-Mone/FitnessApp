namespace FitnessApp.DTOs
{
    public class MemberInvoiceGet
    {
        public decimal TotalAmount { get; set; }
        public string MemberId { get; set; }
        public decimal? RemainingValue { get; set; }
    }
}
