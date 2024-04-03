using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace STEAM_DB;

public partial class SteamContext : DbContext
{
    public SteamContext()
    {
    }

    public SteamContext(DbContextOptions<SteamContext> options)
        : base(options)
    {
    }

    // файл логгирования (логгирование помогает нам контролировать все операции в приложении)
    private readonly StreamWriter logStream = new("mylog.txt", true);

    public DbSet<Developer> Developers { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // логгирование информации методом LogTo и запись её в файл;
        // LogLavel.Information - уровень сообщений, позволяющий просто отследить поток выполнения приложения
        optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
    }

    public override void Dispose()
    {
        // освобождаем неуправляемые ресурсы (подключения к файлам, базам данных, сетевые подключения и т.д.)
        base.Dispose();
        logStream.Dispose();
        // подавляем финализацию
        GC.SuppressFinalize(this);
    }

    public override async ValueTask DisposeAsync()
    {
        // освобождаем неуправляемые ресурсы
        await base.DisposeAsync();
        await logStream.DisposeAsync();
        // подавляем финализацию
        GC.SuppressFinalize(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.DeveloperId).HasName("developers_pkey");

            entity.ToTable("developers");

            entity.Property(e => e.DeveloperId).HasColumnName("developer_id");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Developername)
                .HasMaxLength(255)
                .HasColumnName("developername");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("games_pkey");

            entity.ToTable("games");

            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DeveloperId).HasColumnName("developer_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.DeveloperId)
                .HasConstraintName("games_developer_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.Games)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("games_genre_id_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.Genrename)
                .HasMaxLength(255)
                .HasColumnName("genrename");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("purchases_pkey");

            entity.ToTable("purchases");

            entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.PurchaseDate)
                .HasMaxLength(15)
                .HasColumnName("date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("purchases_game_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("purchases_user_id_fkey");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("ratings_pkey");

            entity.ToTable("ratings");

            entity.Property(e => e.RatingId).HasColumnName("rating_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("ratings_game_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("ratings_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Registration)
                .HasMaxLength(15)
                .HasColumnName("registration");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("wishlist_pkey");

            entity.ToTable("wishlist");

            entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");
            entity.Property(e => e.GameId).HasColumnName("game_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("wishlist_game_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("wishlist_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
