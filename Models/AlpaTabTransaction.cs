using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlpaTabApi.Models
{
    public class AlpaTabTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey(nameof(AlpaTabUser))]
        public string NickName { get; set; }

        // [Required]
        // [ForeignKey("AlpaTabUserForeignKey")]
        // public AlpaTabUser User { get; set; }

        [Required]
        public double BalanceBeforeTransaction { get; set; }

        [Required]
        public double Amount { get; set; } // positive if user owes the club 

        [Required]
        public string TransactionType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Timestamp { get; set; } 

        public string Description { get; set; }
    }
}
