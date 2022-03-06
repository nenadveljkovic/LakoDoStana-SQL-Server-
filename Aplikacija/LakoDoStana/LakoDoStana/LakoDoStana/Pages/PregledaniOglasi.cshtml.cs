using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LakoDoStana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LakoDoStana.Pages
{
    public class PregledaniOglasiModel : PageModel
    {
        public Korisnik LogovaniKorisnik { get; set; }
        public List<KorisnikOglas> lista;
        public List<Oglas> ListaOglasa;
        private readonly LDSContext context;
        [BindProperty(Name = "username", SupportsGet = true)]
        public string username { get; set; }

        public List<Poruka> poruke;

        public string[] slike;
        public PregledaniOglasiModel(LDSContext con)
        {
            context = con;
        }
        public void OnGet(string username)
        {
            poruke = new List<Poruka>();
            while (LogovaniKorisnik == null)
                LogovaniKorisnik = context.Korisnici.Where(x => x.Username == username).FirstOrDefault();
            poruke = context.Poruke.Where(x => x.PrimalacId == LogovaniKorisnik.ID).Include(x => x.Posiljalac).ToList();
            poruke.Reverse();
            lista = context.KorisniciOglasi.Where(x => x.KorisnikId == LogovaniKorisnik.ID && x.TipVeze == "Pregledao").Include(x => x.Oglas).ToList();
            if (lista != null)
            {
                ListaOglasa = new List<Oglas>();
                foreach (KorisnikOglas ko in lista)
                    ListaOglasa.Add(ko.Oglas);
            }

            slike = new string[100];
            for (int t = 0; t < 100; t++)
            {
                string[] putanje = Directory.GetFiles(@"wwwroot\Pictures");
                string pom = Path.GetFileName(putanje.First());
                slike[t] = ("Pictures/" + pom);
            }
            foreach (Oglas o in ListaOglasa)
            {
                string[] putanje;
                if (Directory.Exists(@"wwwroot\Pictures\" + o.OglasId))
                {
                    putanje = Directory.GetFiles(@"wwwroot\Pictures\" + o.OglasId);
                    {
                        if (putanje.Count() != 0)
                        {
                            string pom = Path.GetFileName(putanje.First());
                            slike[o.OglasId] = ("Pictures/" + o.OglasId + "/" + pom);
                        }
                        else
                        {
                            putanje = Directory.GetFiles(@"wwwroot\Pictures");
                            string pom = Path.GetFileName(putanje.First());
                            slike[o.OglasId] = ("Pictures/" + pom);
                        }
                    }
                }
                else
                {
                    putanje = Directory.GetFiles(@"wwwroot\Pictures");
                    string pom = Path.GetFileName(putanje.First());
                    slike[o.OglasId] = ("Pictures/" + pom);
                }
            }
        }
        public async Task<IActionResult> OnGetObrisiAsync(int id, string Username)
        {
            while (LogovaniKorisnik == null)
                LogovaniKorisnik = context.Korisnici.Where(x => x.Username == username).FirstOrDefault();
            var pregled = context.KorisniciOglasi.Where(x=>x.KorisnikId == LogovaniKorisnik.ID && x.OglasId == id && x.TipVeze == "Pregledao").FirstOrDefault();
            if (pregled != null)
            {
                context.KorisniciOglasi.Remove(pregled);
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("Greška pri radu sa bazom!\n" + ex.Message);
                }
                catch (Exception exe)
                {
                    throw new Exception("Greška!" + exe.Message);
                }
            }
            return base.RedirectToPage(new { username = Username });
        }

        /*public string VratiSliku(Oglas o)
        {
            string[] putanje = Directory.GetFiles(@"wwwroot\Pictures\" + o.OglasId);
            if (putanje.Count() != 0)
            {
                string pom = Path.GetFileName(putanje.First());
                return "Pictures/" + o.OglasId + "/" + pom;
            }
            else
            {
                putanje = Directory.GetFiles(@"wwwroot\Pictures");
                string pom = Path.GetFileName(putanje.First());
                return "Pictures/" + pom;
            }
        }*/

        public string IzracunajVreme(Poruka p)
        {
            int sekunde = Convert.ToInt32((Convert.ToDateTime(DateTime.Today.ToString("F")) - Convert.ToDateTime(p.DatumSlanja.ToString("F"))).TotalSeconds);
            int minuti = sekunde / 60;
            int sati = minuti / 60;
            int dani = sati / 24;
            if (dani != 0)
                return dani + "d ago";
            else
                return "Today";
        }

        public string VratiTipObjekta(Oglas o)
        {
            if (o.TipObjekta == 0)
                return "Kuća";
            else
                return "Stan";
        }

        /*public string VratiUsername(Oglas o)
        {
            KorisnikOglas ko = context.KorisniciOglasi.Where(x => x.OglasId == o.OglasId && x.TipVeze == "Postavio").FirstOrDefault();
            Korisnik k = context.Korisnici.Where(x => x.ID == ko.KorisnikId).FirstOrDefault();
            return k.Username;
        }*/

        public string VratiTipOglasa(Oglas o)
        {
            if (o.TipOglasa == 0)
                return "Traži se cimer!";
            else
                return "Traže se stanari!";
        }

        public string VratiDatum(Oglas o)
        {
            return o.DatumObjavljivanja.ToString("dd/MM/yy");
        }
    }
}