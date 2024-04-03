namespace STEAM_DB;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public decimal Price { get; set; }

    public int? DeveloperId { get; set; }

    public int? GenreId { get; set; }

    public Developer? Developer { get; set; }

    public Genre? Genre { get; set; }

    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
