using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels = FuneralHome.Domain.Models;

namespace FuneralHome.DTOs;

public class AddKupnja
{
    public Kupnja Kupnja { get; set; } = new Kupnja();
    public List<KupnjaOpremaUsluge> Oprema { get; set; } = new List<KupnjaOpremaUsluge>();
}

