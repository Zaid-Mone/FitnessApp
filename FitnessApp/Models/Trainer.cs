using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Models
{
    public class Trainer
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("PersonId")]
        public string PersonId { get; set; }
        public Person Person { get; set; }
        [NotMapped]
        public List<Member> Members { get; }
    }

}
