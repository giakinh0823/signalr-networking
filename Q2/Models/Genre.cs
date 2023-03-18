using System;
using System.Collections.Generic;

namespace Q2.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
