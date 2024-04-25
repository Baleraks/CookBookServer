using System;
using System.Collections.Generic;
using CookBookBase.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookBase;

public partial class CookBookDbContext : DbContext
{
    public CookBookDbContext()
    {
    }

    public CookBookDbContext(DbContextOptions<CookBookDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Ingridient> Ingridients { get; set; }

    public virtual DbSet<Ingridienttoqauntity> Ingridienttoqauntities { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Qauntity> Qauntitys { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Recipetoingridient> Recipetoingridients { get; set; }

    public virtual DbSet<Recipetoqauntity> Recipetoqauntities { get; set; }

    public virtual DbSet<Recipetotag> Recipetotags { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Step> Steps { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=CookBookDb;Username=postgres;Password=Balakras2");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_comments");

            entity.ToTable("comments");

            entity.HasIndex(e => e.Firstcommentid, "branch_fk");

            entity.HasIndex(e => e.RecId, "commented_fk");

            entity.HasIndex(e => e.Id, "comments_pk").IsUnique();

            entity.HasIndex(e => e.UseId, "owns5_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Commenttext)
                .HasMaxLength(1024)
                .HasColumnName("commenttext");
            entity.Property(e => e.Firstcommentid).HasColumnName("firstcommentid");
            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.UseId).HasColumnName("use_id");

            entity.HasOne(d => d.Firstcomment).WithMany(p => p.InverseFirstcomment)
                .HasForeignKey(d => d.Firstcommentid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_comments_branch_comments");

            entity.HasOne(d => d.Rec).WithMany(p => p.Comments)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_comments_recipetoc_recipes");

            entity.HasOne(d => d.Use).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_comments_usertocom_users");
        });

        modelBuilder.Entity<Ingridient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_ingridients");

            entity.ToTable("ingridients");

            entity.HasIndex(e => e.Id, "ingridients_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ingridienname)
                .HasMaxLength(64)
                .HasColumnName("ingridienname");
            entity.Property(e => e.Ingridientcalories).HasColumnName("ingridientcalories");
        });

        modelBuilder.Entity<Ingridienttoqauntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_ingridienttoqauntity");

            entity.ToTable("ingridienttoqauntity");

            entity.HasIndex(e => e.IngId, "ingridienttoqauntity2_fk");

            entity.HasIndex(e => e.QauId, "ingridienttoqauntity_fk");

            entity.HasIndex(e => e.Id, "ingridienttoqauntity_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngId).HasColumnName("ing_id");
            entity.Property(e => e.QauId).HasColumnName("qau_id");

            entity.HasOne(d => d.Ing).WithMany(p => p.Ingridienttoqauntities)
                .HasForeignKey(d => d.IngId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_ingridie_ingridien_ingridie");

            entity.HasOne(d => d.Qau).WithMany(p => p.Ingridienttoqauntities)
                .HasForeignKey(d => d.QauId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_ingridie_ingridien_qauntity");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_likes");

            entity.ToTable("likes");

            entity.HasIndex(e => e.UseId, "likes2_fk");

            entity.HasIndex(e => e.Id, "likes_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.UseId).HasColumnName("use_id");

            entity.HasOne(d => d.Rec).WithMany(p => p.LikesNavigation)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_likes_likes_recipes");

            entity.HasOne(d => d.Use).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_likes_likes2_users");
        });

        modelBuilder.Entity<Qauntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_qauntitys");

            entity.ToTable("qauntitys");

            entity.HasIndex(e => e.Id, "qauntitys_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type)
                .HasMaxLength(64)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_recipes");

            entity.ToTable("recipes");

            entity.HasIndex(e => e.UseId, "owns1_fk");

            entity.HasIndex(e => e.Id, "recipes_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cokingtime).HasColumnName("cokingtime");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Recipecalories).HasColumnName("recipecalories");
            entity.Property(e => e.Recipeimagepath)
                .HasMaxLength(225)
                .HasColumnName("recipeimagepath");
            entity.Property(e => e.Recipename)
                .HasMaxLength(64)
                .HasColumnName("recipename");
            entity.Property(e => e.Reportsnum).HasColumnName("reportsnum");
            entity.Property(e => e.UseId).HasColumnName("use_id");

            entity.HasOne(d => d.Use).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipes_recipetou_users");
        });

        modelBuilder.Entity<Recipetoingridient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_recipetoingridient");

            entity.ToTable("recipetoingridient");

            entity.HasIndex(e => e.Id, "owns3_pk").IsUnique();

            entity.HasIndex(e => e.IngId, "owns6_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngId).HasColumnName("ing_id");
            entity.Property(e => e.RecId).HasColumnName("rec_id");

            entity.HasOne(d => d.Ing).WithMany(p => p.Recipetoingridients)
                .HasForeignKey(d => d.IngId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetoi_ingridie");

            entity.HasOne(d => d.Rec).WithMany(p => p.Recipetoingridients)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetoi_recipes");
        });

        modelBuilder.Entity<Recipetoqauntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_recipetoqauntity");

            entity.ToTable("recipetoqauntity");

            entity.HasIndex(e => e.RtoiId, "recipetoqauntity1_fk");

            entity.HasIndex(e => e.ItoqId, "recipetoqauntity2_fk");

            entity.HasIndex(e => e.Id, "recipetoqauntity_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItoqId).HasColumnName("itoq_id");
            entity.Property(e => e.RtoiId).HasColumnName("rtoi_id");

            entity.HasOne(d => d.Itoq).WithMany(p => p.Recipetoqauntities)
                .HasForeignKey(d => d.ItoqId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetoq_ingridie");

            entity.HasOne(d => d.Rtoi).WithMany(p => p.Recipetoqauntities)
                .HasForeignKey(d => d.RtoiId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetoq_recipeto");
        });

        modelBuilder.Entity<Recipetotag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_recipetotag");

            entity.ToTable("recipetotag");

            entity.HasIndex(e => e.Id, "owns4_pk").IsUnique();

            entity.HasIndex(e => e.TagId, "owns7_fk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.TagId).HasColumnName("tag_id");

            entity.HasOne(d => d.Rec).WithMany(p => p.Recipetotags)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetot_recipes");

            entity.HasOne(d => d.Tag).WithMany(p => p.Recipetotags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_recipeto_recipetot_tags");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_reports");

            entity.ToTable("reports");

            entity.HasIndex(e => e.UseId, "reports2_fk");

            entity.HasIndex(e => e.Id, "reports_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.UseId).HasColumnName("use_id");

            entity.HasOne(d => d.Rec).WithMany(p => p.Reports)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reports_reports_recipes");

            entity.HasOne(d => d.Use).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reports_reports2_users");
        });

        modelBuilder.Entity<Step>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_steps");

            entity.ToTable("steps");

            entity.HasIndex(e => e.RecId, "owns2_fk");

            entity.HasIndex(e => e.Id, "steps_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecId).HasColumnName("rec_id");
            entity.Property(e => e.Stepimagepath)
                .HasMaxLength(225)
                .HasColumnName("stepimagepath");
            entity.Property(e => e.Steptext)
                .HasMaxLength(1024)
                .HasColumnName("steptext");
            entity.Property(e => e.Steptime).HasColumnName("steptime");

            entity.HasOne(d => d.Rec).WithMany(p => p.Steps)
                .HasForeignKey(d => d.RecId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_steps_recipetos_recipes");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tags");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Id, "tags_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tagname)
                .HasMaxLength(64)
                .HasColumnName("tagname");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_users");

            entity.ToTable("users");

            entity.HasIndex(e => e.Id, "users_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hashpassword)
                .HasMaxLength(225)
                .HasColumnName("hashpassword");
            entity.Property(e => e.Isadmin).HasColumnName("isadmin");
            entity.Property(e => e.Nick)
                .HasMaxLength(64)
                .HasColumnName("nick");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
