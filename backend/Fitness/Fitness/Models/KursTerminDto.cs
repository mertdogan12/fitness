using System;

namespace Fitness.Models
{
    public class KursTerminDto
    {
        public int Id { get; set; }
        public int KursId { get; set; }
        public Kurs? Kurs { get; set; }
        public int TrainerID { get; set; }
        public DateTime? Anfang { get; set; }
        public int MaxTeilnehmer { get; set; }

        public int TeilnehmerAnzahl { get; set; }
    }
}