﻿using BabyKlockan_3.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyKlockan_3.Data;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<ContractionModel>? Contractions { get; set; }
}
