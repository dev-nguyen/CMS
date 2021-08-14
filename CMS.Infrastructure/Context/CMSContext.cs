using System;
using CMS.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CMS.Infrastructure
{
    public partial class CMSContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public CMSContext()
        {
        }

        public CMSContext(DbContextOptions<CMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<HyperlinkValue> HyperlinkValues { get; set; }
        public virtual DbSet<Item> Items { get; set; }
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
                optionsBuilder.UseSqlServer("Server=.;Database=CMS;uid=sa;pwd=Admin@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.CategoryAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Category_AspNetUsers");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.CategoryEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_Category_AspNetUsers1");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("date");

                entity.Property(e => e.Modified).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.GroupAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Group_AspNetUsers");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.GroupEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_Group_AspNetUsers1");
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

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.ItemAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Item_AspNetUsers");

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CatalogId)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.ItemEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_Item_AspNetUsers1");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Item_Group");
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

                entity.HasOne(d => d.Catagory)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_ReferenceValue_Category");

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

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.VariableAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_Variable_AspNetUsers");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.VariableEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_Variable_AspNetUsers1");

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

                entity.HasOne(d => d.AuthorNavigation)
                    .WithMany(p => p.VariableTypeAuthorNavigations)
                    .HasForeignKey(d => d.Author)
                    .HasConstraintName("FK_VariableType_AspNetUsers");

                entity.HasOne(d => d.EditorNavigation)
                    .WithMany(p => p.VariableTypeEditorNavigations)
                    .HasForeignKey(d => d.Editor)
                    .HasConstraintName("FK_VariableType_AspNetUsers1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
