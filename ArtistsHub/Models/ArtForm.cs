using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistsHub.Models
{
    public class ArtForm
    {
        [Key]
        public int ArtFormID { get; set; }

        public string ArtFormName { get; set; }

        // An art-form belongs to one discipline.
        // A discipline can have many art-forms.
        [ForeignKey("Discipline")]
        public int DisciplineID { get; set; }
        public virtual Discipline Discipline { get; set; }

        //An art-form can belong to many artists.
        public ICollection<Artist> Artists { get; set; }
    }

    public class ArtFormDto
    {
        public int ArtFormID { get; set; }

        public string ArtFormName { get; set; }

        public string DisciplineName { get; set; }

    }
}