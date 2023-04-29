using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Invoice
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "User Pays")]
        [Required]
        public decimal Userpays { get; set; }
        public bool IsFullyPaid { get; set; } = false;
        /// <summary>
        ///     if  IsFullyPaid == True 
        ///     RemainingValue = TotalAmount - Userpays
        /// </summary>
        /// 
        [Display(Name = "Remaining Value")]
        public decimal RemainingValue { get; set; }
        [Display(Name = "User Pay Date")]
        [DataType(DataType.Date)]
        public DateTime UserPayDate { get; set; }
        [ForeignKey("MemberId")]
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        public string SerialNumber { get; set; }
    }

}
