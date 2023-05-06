using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.DTOs
{
    public class InsertInvoiceDTO
    {
  
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "User Pays")]
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
        public DateTime UserPayDate { get; set; }
        [Display(Name = "Member")]
        public string MemberId { get; set; }
        //public decimal GymBundlePrice { get; set; }

    }
}
