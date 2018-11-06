using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SPLoyaltyUser.Models
{
    public partial class LoyaltyContext : DbContext
    {
        public LoyaltyContext()
        {
        }

        public LoyaltyContext(DbContextOptions<LoyaltyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClientCards> ClientCards { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ClientsPublic> ClientsPublic { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(local)\\EXPRESS2017;Database=Loyalty;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientCards>(entity =>
            {
                entity.HasIndex(e => new { e.IsPrimary, e.ClientId })
                    .HasName("IX_ClientCards_IsPrimaryForClientID");

                entity.Property(e => e.CardNumber).IsUnicode(false);

                entity.Property(e => e.MsreplTranVersion).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientCards)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_ClientCards_Clients");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasIndex(e => e.CardOrAccountNumber)
                    .HasName("UK_CardOrAccountnumber")
                    .IsUnique();

                entity.HasIndex(e => e.Lname)
                    .HasName("I_LName");

                entity.HasIndex(e => new { e.Id, e.Fname, e.Lname, e.BirthDate })
                    .HasName("I_FName");

                entity.HasIndex(e => new { e.Id, e.CardOrAccountNumber, e.Fname, e.Lname, e.BirthDate, e.LegalEntityId, e.AccountInactiveCodeId })
                    .HasName("I_AccountInactiveCodeId");

                entity.HasIndex(e => new { e.Id, e.CardOrAccountNumber, e.Fname, e.Lname, e.BirthDate, e.LegalEntityId, e.ReplacedId, e.AccountInactiveCodeId })
                    .HasName("I_Replaced");

                entity.Property(e => e.CardOrAccountNumber).IsUnicode(false);

                entity.Property(e => e.Cvv).IsUnicode(false);

                entity.Property(e => e.MsreplTranVersion).HasDefaultValueSql("(newid())");

                entity.Property(e => e.RecordAddedBy).IsUnicode(false);

                entity.Property(e => e.SerialTrack3).IsUnicode(false);

                entity.HasOne(d => d.Replaced)
                    .WithMany(p => p.InverseReplaced)
                    .HasForeignKey(d => d.ReplacedId)
                    .HasConstraintName("Clients_Clients_FK1");
            });

            modelBuilder.Entity<ClientsPublic>(entity =>
            {
                entity.HasIndex(e => e.ClientsId)
                    .HasName("ClientsPublic_UC1")
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .HasName("I_Email");

                entity.HasIndex(e => new { e.Phone, e.PhoneCountryPrefixId })
                    .HasName("I_Phone");

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.MsreplTranVersion).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.Property(e => e.PhoneOther).IsUnicode(false);

                entity.Property(e => e.RecordModifiedBy).IsUnicode(false);

                entity.Property(e => e.Zip).IsUnicode(false);

                entity.HasOne(d => d.Clients)
                    .WithOne(p => p.ClientsPublic)
                    .HasForeignKey<ClientsPublic>(d => d.ClientsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Clients_ClientsPublic_FK1");
            });
        }
    }
}
