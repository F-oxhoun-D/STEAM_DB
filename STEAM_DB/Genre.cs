using System;
using System.Collections.Generic;

namespace STEAM_DB;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Genrename { get; set; } = null!;

    public ICollection<Game> Games { get; set; } = new List<Game>();
}
