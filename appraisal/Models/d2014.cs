namespace appraisal.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class d2014 : DbContext
    {
        public d2014()
            : base("name=d2014")
        {
        }

        public virtual DbSet<actlog> actlog { get; set; }
        public virtual DbSet<dep> dep { get; set; }
        public virtual DbSet<emp> emp { get; set; }
        public virtual DbSet<exm> exm { get; set; }
        public virtual DbSet<ots> ots { get; set; }
        public virtual DbSet<ts> ts { get; set; }
        public virtual DbSet<ImportTs> inportts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<actlog>()
                .Property(e => e.App)
                .IsUnicode(false);

            modelBuilder.Entity<actlog>()
                .Property(e => e.Pepo)
                .IsUnicode(false);

            modelBuilder.Entity<actlog>()
                .Property(e => e.Act)
                .IsUnicode(false);

            modelBuilder.Entity<actlog>()
                .Property(e => e.Ext)
                .IsUnicode(false);

            modelBuilder.Entity<dep>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<dep>()
                .HasMany(e => e.emp)
                .WithRequired(e => e.dep)
                .HasForeignKey(e => e.dept)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<emp>()
                .Property(e => e.eid)
                .IsUnicode(false);

            modelBuilder.Entity<emp>()
                .Property(e => e.cname)
                .IsUnicode(false);

            modelBuilder.Entity<emp>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<emp>()
                .HasMany(e => e.ts1)
                .WithRequired(e => e.emp1)
                .HasForeignKey(e => e.emp)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<emp>()
               .HasMany(e => e.ts2)
               .WithRequired(e => e.emp2)
               .HasForeignKey(e => e.boss)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<exm>()
                .Property(e => e.subject)
                .IsUnicode(false);

            modelBuilder.Entity<exm>()
                .Property(e => e.sn)
                .IsUnicode(false);

            modelBuilder.Entity<exm>()
                .HasMany(e => e.ts)
                .WithRequired(e => e.exm1)
                .HasForeignKey(e => e.exm)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ots>()
                .Property(e => e.Skey)
                .IsUnicode(false);

            modelBuilder.Entity<ots>()
                .Property(e => e.Vl)
                .IsUnicode(false);

            modelBuilder.Entity<ts>()
                .Property(e => e.suggest)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Name1)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Name2)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Depart)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Group)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Personal)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Job1)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Job2)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.MTitle)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.PTitle)
                .IsUnicode(false);

            modelBuilder.Entity<ImportTs>()
                .Property(e => e.Type)
                .IsUnicode(false);
        }
    }
}
