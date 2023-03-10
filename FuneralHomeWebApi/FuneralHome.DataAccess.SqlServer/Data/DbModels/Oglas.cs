﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FuneralHome.DataAccess.SqlServer.Data.DbModels
{
    [Index("SmrtniSlucajId", Name = "IX_Oglas", IsUnique = true)]
    public partial class Oglas
    {
        [Key]
        public int Id { get; set; }
        public int SmrtniSlucajId { get; set; }
        public int OsmrtnicaId { get; set; }
        public byte[] SlikaPok { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Opis { get; set; }
        public bool ObjavaNaStranici { get; set; }

        [ForeignKey("OsmrtnicaId")]
        [InverseProperty("Oglas")]
        public virtual Osmrtnica Osmrtnica { get; set; }
        [ForeignKey("SmrtniSlucajId")]
        [InverseProperty("Oglas")]
        public virtual SmrtniSlucaj SmrtniSlucaj { get; set; }
    }
}