using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace cis499.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoryRecipe> CategoryRecipes { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<HomePage> HomePages { get; set; }

    public virtual DbSet<Invoive> Invoives { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RequestRecipe> RequestRecipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Useraccount> Useraccounts { get; set; }

    public virtual DbSet<Virtualbank> Virtualbanks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\sqlexpress;Initial Catalog=cis499;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryRecipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC078DE20247");

            entity.ToTable("CategoryRecipe");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactU__3214EC07879A3D88");

            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HomePage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HomePage__3214EC07FB9823AE");

            entity.ToTable("HomePage");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ImagePath)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text1)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text2)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text3)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text4)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Text5)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Invoive>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoive__3214EC07866CEE99");

            entity.ToTable("Invoive");

            entity.HasOne(d => d.RequestRecipe).WithMany(p => p.Invoives)
                .HasForeignKey(d => d.RequestRecipeId)
                .HasConstraintName("Recipe_Requestfk");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Invoives)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("UserIds_Requestfk");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recipe__3214EC07640A0C78");

            entity.ToTable("Recipe");

            entity.Property(e => e.AddedDate).HasColumnType("date");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Steps)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.CategoryRecipe).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryRecipeId)
                .HasConstraintName("CategoryIds_fk");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("UserId_Recipefk");
        });

        modelBuilder.Entity<RequestRecipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestR__3214EC072FE4CB21");

            entity.ToTable("RequestRecipe");

            entity.Property(e => e.Ingredient)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PreparationTime)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RecipeName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.RequestDate).HasColumnType("date");
            entity.Property(e => e.State)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Steps)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.ChefAccount).WithMany(p => p.RequestRecipeChefAccounts)
                .HasForeignKey(d => d.ChefAccountId)
                .HasConstraintName("ChefId_Requestfk");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.RequestRecipeUserAccounts)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("UserId_Requestfk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07D7D50262");

            entity.ToTable("Role");

            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Testimon__3214EC078A3D712B");

            entity.ToTable("Testimonial");

            entity.Property(e => e.AddedDate).HasColumnType("date");
            entity.Property(e => e.State)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TestMessage)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.UserAccount).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserAccountId)
                .HasConstraintName("UserId_Testfk");
        });

        modelBuilder.Entity<Useraccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Useracco__3214EC072FAC9A1A");

            entity.ToTable("Useraccount");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Imagepath)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Useraccounts)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("Roleid");
        });

        modelBuilder.Entity<Virtualbank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__virtualb__3214EC0726AC5B79");

            entity.ToTable("virtualbank");

            entity.Property(e => e.Balance)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Cvv)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
