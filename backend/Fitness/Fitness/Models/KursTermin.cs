using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_KURS_TERMIN", Schema = "FS242_LFENTZAHN")]
    public class KursTermin
    {
        [Key]
        [Column("ID")]
        [Required]
        public int Id { get; set; }

        [Column("KURS_ID")]
        [Required]
        public int KursId { get; set; }

        [ForeignKey(nameof(KursId))]
        public Kurs? Kurs { get; set; }

        [Column("TRAINER_ID")]
        [Required]
        public int TrainerID { get; set; }

        [Column("ANFANG", TypeName = "DATE")]
        public DateTime? Anfang { get; set; }

        [Column("MAX_TEILNEHMER")]
        [Required]
        public int MaxTeilnehmer { get; set; }
    }
}
