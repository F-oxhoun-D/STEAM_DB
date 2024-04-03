namespace STEAM_DB;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Registration { get; set; } = null!;

    public string Password { get; set; } = null!;

    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
