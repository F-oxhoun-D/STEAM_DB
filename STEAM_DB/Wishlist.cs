using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
