using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ArtistsHub.Models
{
    public class Discipline
    {
        [Key]
        public int DisciplineID { get; set; }

        public string DisciplineName { get; set; }

        public string DisciplineDescription { get; set; }


    }
}