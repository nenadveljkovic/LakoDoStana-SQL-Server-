﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LakoDoStana.Models
{
    public class Oglas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OglasId { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Grad { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string Adresa { get; set; }
        [Required]
        [Range(0,9999)]
        public int Cena { get; set; }
        [Required]
        public int Kvadratura { get; set; }
        [Required]
        public int BrojSoba { get; set; }
        [Required]
        [Range(0, 1)]
        public int TipObjekta { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Opis { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime DatumObjavljivanja { get; set; }
        [Required]
        public int BrojPregleda { get; set; }
        [Required]
        [Range(0, 1)]
        public int TipOglasa { get; set; }
    }
}
