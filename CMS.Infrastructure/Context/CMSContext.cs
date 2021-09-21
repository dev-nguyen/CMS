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
        public virtual DbSet<DateTimeValue> DateTimeValues { get; set; }
        public virtual DbSet<FloatValue> FloatValues { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<NumberValue> NumberValues { get; set; }
        public virtual DbSet<OptionSource> OptionSources { get; set; }
        public virtual DbSet<OptionValue> OptionValues { get; set; }
        public virtual DbSet<ReferenceSource> ReferenceSources { get; set; }
        public virtual DbSet<ReferenceSourceType> ReferenceSourceTypes { get; set; }
        public virtual DbSet<ReferenceValue> ReferenceValues { get; set; }
        public virtual DbSet<StringValue> StringValues { get; set; }
        public virtual DbSet<TextValue> TextValues { get; set; }
        public virtual DbSet<TrueFalseValue> TrueFalseValues { get; set; }
        public virtual DbSet<Variable> Variables { get; set; }
        public virtual DbSet<VariableSet> VariableSets { get; set; }
        public virtual DbSet<VariableSetDetail> VariableSetDetails { get; set; }
        public virtual DbSet<VariableType> VariableTypes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=Test;uid=sa;pwd=Admin@123").EnableSensitiveDataLogging();
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

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.CategoryAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Category_AspNetUsers");

                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.CategoryEditors)
                    .HasForeignKey(d => d.EditorId)
                    .HasConstraintName("FK_Category_AspNetUsers1");

                entity.HasOne(d => d.VariableSet)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.VariableSetId)
                    .HasConstraintName("FK_Category_VariableSet");
            });

            modelBuilder.Entity<DateTimeValue>(entity =>
            {
                entity.ToTable("DateTimeValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.DateTimeValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_DateTimeValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.DateTimeValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_DateTimeValue_Variable");
            });

            modelBuilder.Entity<FloatValue>(entity =>
            {
                entity.ToTable("FloatValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.FloatValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_FloatValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.FloatValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_FloatValue_Variable");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.ItemAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Item_AspNetUsers");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.ItemEditors)
                    .HasForeignKey(d => d.EditorId)
                    .HasConstraintName("FK_Item_AspNetUsers1");
            });

            modelBuilder.Entity<NumberValue>(entity =>
            {
                entity.ToTable("NumberValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.NumberValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_NumberValue_Item1");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.NumberValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_NumberValue_Variable1");
            });

            modelBuilder.Entity<OptionSource>(entity =>
            {
                entity.ToTable("OptionSource");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.OptionSources)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_OptionSource_Variable");
            });

            modelBuilder.Entity<OptionValue>(entity =>
            {
                entity.ToTable("OptionValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OptionValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_OptionValue_Item");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.OptionValues)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK_OptionValue_OptionSource");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.OptionValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_OptionValue_Variable");
            });

            modelBuilder.Entity<ReferenceSource>(entity =>
            {
                entity.ToTable("ReferenceSource");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.ReferenceSources)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK_ReferenceSource_Category");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.ReferenceSources)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_ReferenceSource_Variable");
            });

            modelBuilder.Entity<ReferenceSourceType>(entity =>
            {
                entity.ToTable("ReferenceSourceType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<ReferenceValue>(entity =>
            {
                entity.ToTable("ReferenceValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ReferenceValue_Item");

                entity.HasOne(d => d.ReferenceSource)
                    .WithMany(p => p.ReferenceValues)
                    .HasForeignKey(d => d.ReferenceSourceId)
                    .HasConstraintName("FK_ReferenceValue_ReferenceSource");

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

            modelBuilder.Entity<TrueFalseValue>(entity =>
            {
                entity.ToTable("TrueFalseValue");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Value)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TrueFalseValues)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_TrueFalseValue_Item");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.TrueFalseValues)
                    .HasForeignKey(d => d.VariableId)
                    .HasConstraintName("FK_TrueFalseValue_Variable");
            });

            modelBuilder.Entity<Variable>(entity =>
            {
                entity.ToTable("Variable");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.VariableAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Variable_AspNetUsers");

                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.VariableEditors)
                    .HasForeignKey(d => d.EditorId)
                    .HasConstraintName("FK_Variable_AspNetUsers1");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.SourceId)
                    .HasConstraintName("FK_Variable_Category");

                entity.HasOne(d => d.SourceType)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.SourceTypeId)
                    .HasConstraintName("FK_Variable_ReferenceSourceType");

                entity.HasOne(d => d.VariableType)
                    .WithMany(p => p.Variables)
                    .HasForeignKey(d => d.VariableTypeId)
                    .HasConstraintName("FK_Variable_VariableType");
            });

            modelBuilder.Entity<VariableSet>(entity =>
            {
                entity.ToTable("VariableSet");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.VariableSetAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_VariableSet_AspNetUsers");

                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.VariableSetEditors)
                    .HasForeignKey(d => d.EditorId)
                    .HasConstraintName("FK_VariableSet_AspNetUsers1");
            });

            modelBuilder.Entity<VariableSetDetail>(entity =>
            {
                entity.HasKey(e => new { e.VariableSetId, e.VariableId });

                entity.ToTable("VariableSetDetail");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.VariableSetDetails)
                    .HasForeignKey(d => d.VariableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VariableSetDetail_Variable");

                entity.HasOne(d => d.VariableSet)
                    .WithMany(p => p.VariableSetDetails)
                    .HasForeignKey(d => d.VariableSetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VariableSetDetail_VariableSet");
            });

            modelBuilder.Entity<VariableType>(entity =>
            {
                entity.ToTable("VariableType");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.VariableTypeAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_VariableType_AspNetUsers");

                entity.HasOne(d => d.Editor)
                    .WithMany(p => p.VariableTypeEditors)
                    .HasForeignKey(d => d.EditorId)
                    .HasConstraintName("FK_VariableType_AspNetUsers1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
