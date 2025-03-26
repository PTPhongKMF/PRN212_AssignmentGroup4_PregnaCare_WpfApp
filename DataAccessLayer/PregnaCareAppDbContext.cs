using System;
using System.Collections.Generic;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer;

public partial class PregnaCareAppDbContext : DbContext {
    public PregnaCareAppDbContext() {
    }

    public PregnaCareAppDbContext(DbContextOptions<PregnaCareAppDbContext> options)
        : base(options) {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogTag> BlogTags { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<FetalGrowthRecord> FetalGrowthRecords { get; set; }

    public virtual DbSet<GrowthAlert> GrowthAlerts { get; set; }

    public virtual DbSet<GrowthMetric> GrowthMetrics { get; set; }

    public virtual DbSet<MembershipPlan> MembershipPlans { get; set; }

    public virtual DbSet<MembershipPlanFeature> MembershipPlanFeatures { get; set; }

    public virtual DbSet<MotherInfo> MotherInfos { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PregnancyRecord> PregnancyRecords { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<ReminderType> ReminderTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMembershipPlan> UserMembershipPlans { get; set; }

    public virtual DbSet<UserReminder> UserReminders { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString() {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"]!;

        return strConn;
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Blog>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3214EC075C87F429");

            entity.ToTable("Blog");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FeaturedImageUrl).HasDefaultValue("");
            entity.Property(e => e.Heading)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.Property(e => e.PageTitle)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UrlHandle).HasDefaultValue("");
            entity.Property(e => e.ViewCount).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Blog__UserId__6166761E");
        });

        modelBuilder.Entity<BlogTag>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__BlogTag__3214EC0795EF58E8");

            entity.ToTable("BlogTag");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__BlogId__625A9A57");

            entity.HasOne(d => d.Tag).WithMany(p => p.BlogTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BlogTag__TagId__634EBE90");
        });

        modelBuilder.Entity<Comment>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Comment__3214EC072A6AA8F8");

            entity.ToTable("Comment");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CommentText).HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__BlogId__6442E2C9");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__65370702");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserId__662B2B3B");
        });

        modelBuilder.Entity<Feature>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Feature__3214EC0795A2BEDB");

            entity.ToTable("Feature");

            entity.HasIndex(e => e.FeatureName, "UQ__Feature__55ABBB71879A44CA").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.FeatureName)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<FetalGrowthRecord>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__FetalGro__3214EC0741B0ED38");

            entity.ToTable("FetalGrowthRecord");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Note).HasDefaultValue("");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PregnancyRecord).WithMany(p => p.FetalGrowthRecords)
                .HasForeignKey(d => d.PregnancyRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FetalGrow__Pregn__671F4F74");
        });

        modelBuilder.Entity<GrowthAlert>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__GrowthAl__3214EC07471ACB67");

            entity.ToTable("GrowthAlert");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AlertDate).HasColumnType("datetime");
            entity.Property(e => e.AlertFor)
                .HasMaxLength(20)
                .HasDefaultValue("Fetal");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsResolved).HasDefaultValue(false);
            entity.Property(e => e.Issue).HasDefaultValue("");
            entity.Property(e => e.Recommendation).HasDefaultValue("");
            entity.Property(e => e.Severity)
                .HasMaxLength(20)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.FetalGrowthRecord).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.FetalGrowthRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__Fetal__681373AD");

            entity.HasOne(d => d.User).WithMany(p => p.GrowthAlerts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GrowthAle__UserI__690797E6");
        });

        modelBuilder.Entity<GrowthMetric>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__GrowthMe__3214EC07E5BABC14");

            entity.ToTable("GrowthMetric");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MembershipPlan>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC077A3DB194");

            entity.ToTable("MembershipPlan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.PlanName)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<MembershipPlanFeature>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Membersh__3214EC07B71C7BA0");

            entity.ToTable("MembershipPlanFeature");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Feature).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.FeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Featu__69FBBC1F");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.MembershipPlanFeatures)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Membe__6AEFE058");
        });

        modelBuilder.Entity<MotherInfo>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__MotherIn__3214EC07E06F2A85");

            entity.ToTable("MotherInfo");

            entity.HasIndex(e => e.PregnancyRecordId, "UQ__MotherIn__06D3067EE4A33BAD").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BloodType)
                .HasMaxLength(2)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.HealthStatus).HasDefaultValue("");
            entity.Property(e => e.MotherName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.Notes).HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PregnancyRecord).WithOne(p => p.MotherInfo)
                .HasForeignKey<MotherInfo>(d => d.PregnancyRecordId)
                .HasConstraintName("FK__MotherInf__Pregn__6BE40491");
        });

        modelBuilder.Entity<Notification>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07507E7198");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Message).HasDefaultValue("");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<PregnancyRecord>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Pregnanc__3214EC075CAFF8C5");

            entity.ToTable("PregnancyRecord");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.BabyGender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            entity.Property(e => e.BabyName)
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.PregnancyRecords)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Pregnancy__UserI__6CD828CA");
        });

        modelBuilder.Entity<Reminder>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC070F30DE78");

            entity.ToTable("Reminder");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ReminderDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Active");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ReminderType).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.ReminderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reminder__Remind__6DCC4D03");
        });

        modelBuilder.Entity<ReminderType>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Reminder__3214EC07F64ABE18");

            entity.ToTable("ReminderType");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Role>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07C282DE6C");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Tag>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC07EDC9A3F5");

            entity.ToTable("Tag");

            entity.HasIndex(e => e.Name, "UQ__Tag__737584F6467E43FF").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC071B7BE00F");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D105346140B8E2").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(60);
            entity.Property(e => e.FullName)
                .HasMaxLength(60)
                .HasDefaultValue("");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .HasDefaultValue("");
            entity.Property(e => e.ImageUrl).HasDefaultValue("");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserMembershipPlan>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__UserMemb__3214EC0709001913");

            entity.ToTable("UserMembershipPlan");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ActivatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Price)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MembershipPlan).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.MembershipPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__Membe__6EC0713C");

            entity.HasOne(d => d.User).WithMany(p => p.UserMembershipPlans)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMembe__UserI__6FB49575");
        });

        modelBuilder.Entity<UserReminder>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__UserRemi__3214EC07FD11587D");

            entity.ToTable("UserReminder");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Reminder).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.ReminderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__Remin__70A8B9AE");

            entity.HasOne(d => d.User).WithMany(p => p.UserReminders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRemin__UserI__719CDDE7");
        });

        modelBuilder.Entity<UserRole>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC07396D113A");

            entity.ToTable("UserRole");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRole__RoleId__72910220");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRole__UserId__73852659");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
