using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Developer
{
    public int DeveloperId { get; set; }

    public string Developername { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
