using blog_api_dev.Models.Article;
using blog_api_dev.Models.Tech;
using blog_api_dev.Models.Tech_Article;
using blog_api_dev.Models.User;
using Microsoft.EntityFrameworkCore;

namespace blog_api_dev.Broken
{
  public partial class DbContextDatabase : DbContext
    {
        public DbContextDatabase() {}
        public DbContextDatabase(DbContextOptions<DbContextDatabase> options) : base (options) {}

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Technology> Technology { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Tech_Article> Tech_Article { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connString = "User Id=tudqtatuzrrioq;Password=2445d84d3fe171087584faf292ac10daa7aa3b7db5918462a7c70780f5bfccf0;" +
                "Host=ec2-54-165-178-178.compute-1.amazonaws.com;" +
                "Port=5432;Database=dep1lqkfgjvmu0;Pooling=true;SSL Mode=Prefer;TrustServerCertificate=True;";

                // string connString = "Server=localhost;" +
                //                     "Port=5599;" +
                //                     "Database=dep1lqkfgjvmu0;" +
                //                     "User Id=postgres;" +
                //                     "Password=2022BLOGdoDev;";

                optionsBuilder.UseNpgsql(connString);
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "pt_BR.UTF-8");

            modelBuilder.Entity<User>(entity => {
                entity.ToTable("User");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name");
                entity.Property(e => e.nickname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nickname");
                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password");
                entity.Property(e => e.password_key)
                    .IsRequired()
                    .HasColumnName("password_key");
            });
            modelBuilder.Entity<Technology>(entity => {
                entity.ToTable("Technology");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");
                entity.Property(e => e.url_image)
                    .IsRequired()
                    .HasColumnName("url_image");
            });
            modelBuilder.Entity<Article>(entity => {
                entity.ToTable("Article");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("title");
                entity.Property(e => e.notion_id)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("notion_id");
                entity.Property(e => e.datePublication)
                    .IsRequired()
                    .HasColumnName("datePublication");
            });
            modelBuilder.Entity<Tech_Article>(entity => {
                entity.ToTable("Tech_Article");
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.article_id)
                    .IsRequired()
                    .HasColumnName("article_id");
                entity.Property(e => e.tech_id)
                    .IsRequired()
                    .HasColumnName("tech_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}