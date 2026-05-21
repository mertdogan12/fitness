using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_NIMMT_TEIL")]
    public class NimmtTeil
    {
        [Key]
        [Column("USER_ID")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Key]
        [Column("KURS_TERMIN_ID")]
        [Required]
        public int KursTerminId { get; set; }

        [ForeignKey(nameof(KursTerminId))]
        public KursTermin? KursTermin { get; set; }
    }
}
