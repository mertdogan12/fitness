using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness.Models
{
    [Table("FIT_USER", Schema = "FS242_LFENTZAHN")]
    public class User
    {
        private string _vorname = String.Empty;
        private string _name = String.Empty;
        
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("VORNAME")]
        [StringLength(50)]
        [Required]
        public string Vorname 
        {
            get => _vorname;
            set => _vorname = value?.Trim() ?? String.Empty;
        }

        [Column("NAME")]
        [StringLength(50)]
        [Required]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim() ?? String.Empty;
        }

        [Column("USERALTER")]
        [Required]
        public int Alter { get; set; }

        [Column("GESCHLECHT")]
        [StringLength(20)]
        [Required]
        public string Geschlecht { get; set; }
    }
}
