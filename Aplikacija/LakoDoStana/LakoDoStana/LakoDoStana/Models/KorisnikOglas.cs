using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LakoDoStana.Models
{
    public class KorisnikOglas
    {
        public int KorisnikOglasId { get; set; }
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
        public int OglasId { get; set; }
        public Oglas Oglas { get; set; }
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }
        [Required]
        public string TipVeze { get; set; }
    }
}
