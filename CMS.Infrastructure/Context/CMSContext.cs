using System;
using CMS.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CMS.Infrastructure
{
    public partial class CMSContext : DbContext
    {
        public CMSContext()
        {
        }

        public CMSContext(DbContextOptions<CMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Catalogs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<HyperlinkValue> HyperlinkValues { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemGroup> ItemGroups { get; set; }
        public virtual DbSet<MoneyValue> MoneyValues { get; set; }
        public virtual DbSet<NumberValue> NumberValues { get; set; }
        public virtual DbSet<ReferenceValue> ReferenceValues { get; set; }
        public virtual DbSet<StringValue> StringValues { get; set; }
        public virtual DbSet<TextValue> TextValues { get; set; }
        public virtual DbSet<Variable> Variables { get; set; }
        public virtual DbSet<VariableGroup> VariableGroups { get; set; }
        public virtual DbSet<VariableType> VariableTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;database=CMS;uid=sa;pwd=Admin@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Catalog");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<HyperlinkValue>(entity =>
            {
                entity.ToTable("HyperlinkValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Text).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.HyperlinkValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_HyperlinkValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.HyperlinkValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_HyperlinkValue_Variable");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Item_Catalog");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Item_Group");
            });

            modelBuilder.Entity<ItemGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ItemGroup");
            });

            modelBuilder.Entity<MoneyValue>(entity =>
            {
                entity.ToTable("MoneyValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnType("money");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.MoneyValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_MoneyValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.MoneyValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_MoneyValue_Variable");
            });

            modelBuilder.Entity<NumberValue>(entity =>
            {
                entity.ToTable("NumberValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.NumberValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_NumberValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.NumberValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_NumberValue_Attribute");
            });

            modelBuilder.Entity<ReferenceValue>(entity =>
            {
                entity.ToTable("ReferenceValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_ReferenceValue_Catalog");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ReferenceValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_ReferenceValue_Variable");
            });

            modelBuilder.Entity<StringValue>(entity =>
            {
                entity.ToTable("StringValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value).HasMaxLength(50);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.StringValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_StringValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.StringValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_StringValue_Variable");
            });

            modelBuilder.Entity<TextValue>(entity =>
            {
                entity.ToTable("TextValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TextValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_TextValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.TextValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_TextValue_Variable");
            });

            modelBuilder.Entity<Variable>(entity =>
            {
                entity.ToTable("Variable");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.VariableType)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.VariableTypeId)
                    .HasConstraintName("FK_Variable_VariableType");
            });

            modelBuilder.Entity<VariableGroup>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.VariableId })
                    .HasName("PK_AttributeGroup");

                entity.ToTable("VariableGroup");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.VariableGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VariableGroup_Group");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.VariableGroups)
                    .HasForeignKey(d => d.VariableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VariableGroup_Variable");
            });

            modelBuilder.Entity<VariableType>(entity =>
            {
                entity.ToTable("VariableType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
