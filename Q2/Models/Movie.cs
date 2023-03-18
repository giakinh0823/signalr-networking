using System;
using System.Collections.Generic;

namespace Q2.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Genres = new HashSet<Genre>();
            Stars = new HashSet<Star>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? ProducerId { get; set; }
        public int? DirectorId { get; set; }

        public virtual Director? Director { get; set; }
        public virtual Producer? Producer { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Star> Stars { get; set; }
    }
}
