using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LakoDoStana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace LakoDoStana.Pages
{
    public class InboxModel : PageModel
    {
        public Korisnik LogovaniKorisnik { get; set; }

        [BindProperty(Name = "iD", SupportsGet = true)]
        public int iD { get; set; }

        [BindProperty(Name = "porukaiD", SupportsGet = true)]
        public int porukaiD { get; set; }

        public List<Poruka> poruke;

        [BindProperty]
        public Poruka otvorenaPoruka { get; set; }

        public readonly LDSContext context;

        [BindProperty]
        public string TextPoruke { get; set; }

        public InboxModel(LDSContext con)
        {
            context = con;
        }
        public async Task OnGet(int iD, int porukaiD)
        {
            while(LogovaniKorisnik == null)
                LogovaniKorisnik = context.Korisnici.Where(x => x.ID == iD).FirstOrDefault();
            poruke = new List<Poruka>();
            poruke = context.Poruke.Where(x => x.PrimalacId == LogovaniKorisnik.ID).Include(x => x.Posiljalac).ToList();
            poruke.Reverse();
            otvorenaPoruka = context.Poruke.Where(x => x.PorukaId == porukaiD).FirstOrDefault();
            if (otvorenaPoruka != null && otvorenaPoruka.Seen == false)
            {
                otvorenaPoruka.Seen = true;
                context.Poruke.Update(otvorenaPoruka);
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
                RedirectToPage(new { iD = LogovaniKorisnik.ID, porukaiD = otvorenaPoruka.PorukaId });
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            while(LogovaniKorisnik == null)
                LogovaniKorisnik = context.Korisnici.Where(x => x.ID == iD).FirstOrDefault();
            if (porukaiD != 0)
                otvorenaPoruka = context.Poruke.Where(x => x.PorukaId == porukaiD).FirstOrDefault();
            poruke = new List<Poruka>();
            poruke = context.Poruke.Where(x => x.PrimalacId == LogovaniKorisnik.ID).Include(x => x.Posiljalac).ToList();
            poruke.Reverse();

            Poruka P = new Poruka();
            P.PosiljalacId = LogovaniKorisnik.ID;
            P.Tekst = TextPoruke;
            DateTime dt = DateTime.Parse(DateTime.Today.ToString("F"));
            P.DatumSlanja = dt;
            P.Seen = false;
            P.PrimalacId = otvorenaPoruka.PosiljalacId;

            context.Poruke.Add(P);
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
            return RedirectToPage(new { iD = LogovaniKorisnik.ID, porukaiD = 0 });
        }
    }
}