using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LakoDoStana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LakoDoStana.Pages
{
    public class SpisakKorisnikaModel : PageModel
    {
        public Korisnik LogovaniKorisnik { get; set; }
        [BindProperty(SupportsGet = true)]
        public Administrator Admin { get; set; }
        public List<Korisnik> ListaKorisnika;
        private readonly LDSContext context;
        [BindProperty(Name = "username", SupportsGet = true)]
        public string username { get; set; }

        public List<Poruka> poruke;
        public SpisakKorisnikaModel(LDSContext con)
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
            Admin = LogovaniKorisnik as Administrator;
            ListaKorisnika = context.Korisnici.Where(x=>x is Posetilac || x is Oglasivac ).ToList();
        }
        public async Task<IActionResult> OnGetObrisiAsync(int id,string Username)
        { 
            var korisnik = await context.Korisnici.FindAsync(id);
            if (korisnik != null)
            {
                List<KorisnikOglas> lista = await context.KorisniciOglasi.Where(x => x.KorisnikId == korisnik.ID && x.TipVeze == "Postavio").Include(x=>x.Oglas).ToListAsync();
                if (lista != null)
                    foreach (KorisnikOglas ko in lista)
                    {
                        DirectoryInfo di = new DirectoryInfo(@"wwwroot\Pictures\" + ko.Oglas.OglasId);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                        Directory.Delete(@"wwwroot\Pictures\" + ko.Oglas.OglasId);
                        List<KorisnikOglas> vezanozaoglas = await context.KorisniciOglasi.Where(x => x.OglasId == ko.OglasId).ToListAsync();
                        if (vezanozaoglas != null)
                            foreach (KorisnikOglas k in vezanozaoglas)
                                context.KorisniciOglasi.Remove(k);
                        context.Oglasi.Remove(ko.Oglas);
                    }
                List<KorisnikOglas> lista2 = await context.KorisniciOglasi.Where(x => x.KorisnikId == korisnik.ID).ToListAsync();
                if (lista2 != null)
                    foreach (KorisnikOglas ko2 in lista2)
                        context.KorisniciOglasi.Remove(ko2);
                List<Poruka> p = await context.Poruke.Where(x => x.PrimalacId == korisnik.ID).ToListAsync();
                if (p != null)
                    foreach (Poruka por in p)
                        context.Poruke.Remove(por);
                Admin = context.Administratori.Where(x => x.Username == Username).FirstOrDefault();
                if (Admin != null)
                {
                    Admin.BrojObrisanihNaloga += 1;
                    context.Korisnici.Update(Admin);
                }
                context.Korisnici.Remove(korisnik);
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
    }
}