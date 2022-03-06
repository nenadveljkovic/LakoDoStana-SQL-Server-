using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LakoDoStana.Models
{
    public class LDSContext : DbContext
    {
        public LDSContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Poruka> Poruke { get; set; }
        public DbSet<Posetilac> Posetioci { get; set; }
        public DbSet<Oglasivac> Oglasivaci { get; set; }
        public DbSet<Administrator> Administratori { get; set; }
        public DbSet<Oglas> Oglasi { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<KorisnikOglas> KorisniciOglasi { get; set; }

    }
}
