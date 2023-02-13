using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EveMiningFleet.Entities.DbSet;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EveMiningFleet.Entities
{
    public partial class EveMiningFleetContext : DbContext
    {
        // https://docs.microsoft.com/fr-fr/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-5.0
        // dotnet ef migrations add "nom de la migration" --project EveMiningFleet.Entities --startup-project EveMiningFleet.API

        public EveMiningFleetContext()
        {
        }

        public EveMiningFleetContext(DbContextOptions<EveMiningFleetContext> options)
            : base(options)
        {

        }


        public virtual DbSet<Character> characters { get; set; }
        public virtual DbSet<Fleet> fleets { get; set; }
        public virtual DbSet<FleetCharacter> fleetCharacters { get; set; }
        public virtual DbSet<FleetGroup> fleetGroups { get; set; }
        public virtual DbSet<FleetGroupCharacter> fleetGroupCharacters { get; set; }
        public virtual DbSet<FleetTaxes> fleetTaxes { get; set; }
        public virtual DbSet<InvTypeMaterial> invTypeMaterials { get; set; }
        public virtual DbSet<LastMiningLog> lastMiningLogs { get; set; }
        public virtual DbSet<MiningLog> miningLogs { get; set; }
        public virtual DbSet<Ore> ores { get; set; }
        public virtual DbSet<DataPrice> dataPrices { get; set; }
        public virtual DbSet<Corporation> corporations { get; set; }
        public virtual DbSet<Alliance> alliances { get; set; }
        public virtual DbSet<AlerteMessage> alerteMessages { get; set; }
        public virtual DbSet<UsageHistory> usageHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlerteMessage>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            });
            modelBuilder.Entity<Alliance>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            });
            modelBuilder.Entity<Character>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });
            modelBuilder.Entity<Corporation>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            });
            modelBuilder.Entity<DataPrice>(entity =>
            {
                entity.HasKey(e => new { e.TypeId})
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            });
            modelBuilder.Entity<Fleet>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Fleets)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FleetCharacter>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Fleetcharacters)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Fleet)
                    .WithMany(p => p.Fleetcharacters)
                    .HasForeignKey(d => d.FleetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FleetGroup>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Fleet)
                    .WithMany(p => p.Fleetgroups)
                    .HasForeignKey(d => d.FleetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FleetGroupCharacter>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Fleetgroupcharacters)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Fleetgroup)
                    .WithMany(p => p.Fleetgroupcharacters)
                    .HasForeignKey(d => d.FleetgroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<FleetTaxes>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Fleettaxes)
                    .HasForeignKey(d => d.CharacterId);

                entity.HasOne(d => d.Fleet)
                    .WithMany(p => p.Fleettaxes)
                    .HasForeignKey(d => d.FleetId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<InvTypeMaterial>(entity =>
            {
                entity.HasKey(e => new { e.TypeId, e.MaterialTypeId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });

            modelBuilder.Entity<LastMiningLog>(entity =>
            {
                entity.HasKey(e => new { e.CharacterId, e.OreId, e.Date })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.HasOne(d => d.Character)
                    .WithMany(p => p.Lastmininglogs)
                    .HasForeignKey(d => d.CharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_lastMiningLogs_characters_Character_Id");

                entity.HasOne(d => d.Ore)
                    .WithMany(p => p.Lastmininglogs)
                    .HasForeignKey(d => d.OreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_lastMiningLogs_ores_Ore_Id");
            });

            modelBuilder.Entity<MiningLog>(entity =>
            {
                entity.HasKey(e => new { e.FleetCharacterId, e.OreId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.FleetCharacter)
                    .WithMany(p => p.Mininglogs)
                    .HasForeignKey(d => d.FleetCharacterId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Ore)
                    .WithMany(p => p.Mininglogs)
                    .HasForeignKey(d => d.OreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Ore>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            });
            modelBuilder.Entity<UsageHistory>(entity =>
            {
                entity.HasKey(e => new { e.Id })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                entity.HasIndex(b => b.date);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }


}
