using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_KURS", Schema = "FS242_LFENTZAHN")]
    public class Kurs
    {
        [Key]
        [Column("ID")]
        [Required]
        public int Id { get; set; }

        [Column("TITEL")]
        [StringLength(100)]
        [Required]
        public string Titel { get; set; }

        [Column("BESCHREIBUNG")]
        [StringLength(500)]
        [Required]
        public string? Beschreibung { get; set; }

        [Column("MIN_ALTER")]
        [Required]
        public int MinAlter { get; set; }

        [Column("GESCHLECHT")]
        [StringLength(20)]
        [Required]
        public string Geschlecht { get; set; }
        
        [Column("DAUER")]
        [Required]
        public int Dauer { get; set; }
    }
}
