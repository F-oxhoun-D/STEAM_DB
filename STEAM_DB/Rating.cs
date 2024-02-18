using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Rating
{
    public int RatingId { get; set; }

    public int Rating1 { get; set; }

    public string? Comment { get; set; }

    public int? GameId { get; set; }

    public int? UserId { get; set; }

    public virtual Game? Game { get; set; }

    public virtual User? User { get; set; }
}
