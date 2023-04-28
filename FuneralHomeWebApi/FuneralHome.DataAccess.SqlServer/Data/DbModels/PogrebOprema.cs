﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FuneralHome.DataAccess.SqlServer.Data.DbModels
{
    public partial class PogrebOprema
    {
        [Key]
        public int PogrebId { get; set; }
        [Key]
        public int OpremaId { get; set; }
        public int Kolicina { get; set; }

        [ForeignKey("OpremaId")]
        [InverseProperty("PogrebOprema")]
        public virtual Oprema Oprema { get; set; }
        [ForeignKey("PogrebId")]
        [InverseProperty("PogrebOprema")]
        public virtual Pogreb Pogreb { get; set; }
    }
}