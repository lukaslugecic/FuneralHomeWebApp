﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FuneralHome.DataAccess.SqlServer.Data.DbModels
{
    [Index("SmrtniSlucajId", Name = "IX_Pogreb", IsUnique = true)]
    public partial class Pogreb
    {
        [Key]
        public int Id { get; set; }
        public int SmrtniSlucajId { get; set; }
        //public DateOnly DatumPogreba { get; set; }
        public DateTime DatumPogreba { get; set; }
        public bool Kremacija { get; set; }
        public int? UrnaId { get; set; }
        public int? LijesId { get; set; }
        public int? CvijeceId { get; set; }
        public int? NadgrobniZnakId { get; set; }
        public int? GlazbaId { get; set; }
        public bool Snimanje { get; set; }
        public bool Branitelj { get; set; }
        public bool Golubica { get; set; }
        [Column(TypeName = "numeric(18, 2)")]
        public decimal UkupnaCijena { get; set; }

        [ForeignKey("CvijeceId")]
        [InverseProperty("Pogreb")]
        public virtual Cvijece Cvijece { get; set; }
        [ForeignKey("GlazbaId")]
        [InverseProperty("Pogreb")]
        public virtual Glazba Glazba { get; set; }
        [ForeignKey("LijesId")]
        [InverseProperty("Pogreb")]
        public virtual Lijes Lijes { get; set; }
        [ForeignKey("NadgrobniZnakId")]
        [InverseProperty("Pogreb")]
        public virtual NadgrobniZnak NadgrobniZnak { get; set; }
        [ForeignKey("SmrtniSlucajId")]
        [InverseProperty("Pogreb")]
        public virtual SmrtniSlucaj SmrtniSlucaj { get; set; }
        [ForeignKey("UrnaId")]
        [InverseProperty("Pogreb")]
        public virtual Urna Urna { get; set; }
    }
}