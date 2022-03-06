using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LakoDoStana.Models
{
    public class Poruka
    {
        public int PorukaId { get; set; }
        public int PosiljalacId { get; set; }
        public Korisnik Posiljalac { get; set; }
        public int PrimalacId { get; set; }
        [Display(Name = "Datum slanja poruke")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DatumSlanja { get; set; }
        [Required]
        public string Tekst { get; set; }

        [Required]
        public bool Seen { get; set; }
    }
}
