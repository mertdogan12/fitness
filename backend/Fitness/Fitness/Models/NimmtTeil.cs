using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [PrimaryKey(nameof(UserId), nameof(KursTerminId))]
    [Table("FIT_NIMMT_TEIL", Schema = "FS242_LFENTZAHN")]
    public class NimmtTeil
    {
        [Column("USER_ID")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Column("KURS_TERMIN_ID")]
        [Required]
        public int KursTerminId { get; set; }

        [ForeignKey(nameof(KursTerminId))]
        public KursTermin? KursTermin { get; set; }
    }
}
