using System;
using System.Collections.Generic;

namespace Q2.Models
{
    public partial class Star
    {
        public Star()
        {
            Movies = new HashSet<Movie>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public bool? Male { get; set; }
        public DateTime? Dob { get; set; }
        public string? Description { get; set; }
        public string? Nationality { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
