using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LakoDoStana.Models
{
    public class Administrator:Korisnik
    {
        public int BrojObrisanihNaloga { get; set; }
        public IList<Poruka> PrimljenePoruke { get; set; }
        public Administrator()
            :base()
        {

        }

        public Administrator(Korisnik korisnik)
            : base()
        {
            this.ID = korisnik.ID;
            this.Ime = korisnik.Ime;
            this.Prezime = korisnik.Prezime;
            this.Email = korisnik.Email;
            this.DatumRodjenja = korisnik.DatumRodjenja;
            this.DatumKreiranjaNaloga = korisnik.DatumKreiranjaNaloga;
            this.Username = korisnik.Username;
            this.Password = korisnik.Password;
            this.Pol = korisnik.Pol;
        }
    }
}
