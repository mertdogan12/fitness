using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_TRAINER", Schema = "FS242_LFENTZAHN")]
    public class Trainer
    {

        [Key]
        [Column("ID")]
        [Required]
        public int Id { get; set; }

        [Column("VORNAME")]
        [StringLength(50)]
        [Required]
        public string Vorname { get; set; }

        [Column("NAME")]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("GESCHLECHT")]
        [StringLength(20)]
        [Required]
        public string geschlecht { get; set; }
    }
}
