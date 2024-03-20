using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public string PurchaseDate { get; set; } = null!;

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public Game Game { get; set; } = null!;

    public User? User { get; set; }
}
