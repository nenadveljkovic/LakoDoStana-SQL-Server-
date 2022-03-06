using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LakoDoStana.Models
{
    public abstract class Korisnik
    {

        public int ID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Ime { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Prezime { get; set; }
        [Required]
        public string Email { get; set; }
        [Display(Name = "Datum rodjenja")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DatumRodjenja { get; set; }
        [Display(Name = "Datum kreiranja naloga")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DatumKreiranjaNaloga { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Password { get; set; }
        [Required]
        public char Pol { get; set; }

        public Korisnik()
        {

        }

    }
}
