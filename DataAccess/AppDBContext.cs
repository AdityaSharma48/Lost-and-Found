using Microsoft.EntityFrameworkCore;
using Lost_and_Found.Models;
using System;

namespace Lost_and_Found.DataAccess
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<SignModal> SignUp { get; set; }
        public DbSet<ReportLostModal> LostItems { get; set; }
        public DbSet<ReportUserModal> Reports { get; set; }
    }
}
