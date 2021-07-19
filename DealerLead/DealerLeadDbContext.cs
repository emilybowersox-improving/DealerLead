﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace DealerLead
{
    public class DealerLeadDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=.;Database=DealerLead;Trusted_Connection=True;");

        public DbSet<SupportedState> SupportedState { get; set; }

        public DbSet<SupportedMake> SupportedMake { get; set; }
    }
}