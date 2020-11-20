namespace GymRoom.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GymDbContext : DbContext
    {
        public GymDbContext()
            : base("name=GymDbContext")
        {
        }

        public virtual DbSet<GYMER> GYMERs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
