using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public Game? Game { get; set; }

    public User? User { get; set; }
}
