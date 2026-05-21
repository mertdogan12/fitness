using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_USER", Schema = "FS242_LFENTZAHN")]
    public class User
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

        [Column("USERALTER")]
        [Required]
        public int Alter { get; set; }

        [Column("GESCHLECHT")]
        [StringLength(20)]
        [Required]
        public string Geschlecht { get; set; }
    }
}
