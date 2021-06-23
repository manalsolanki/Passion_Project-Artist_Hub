using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistsHub.Models.ViewModel
{
    public class DetailDiscipline
    {
        //the selected discipline 
        public DisciplineDto SelectedDiscipline { get; set; }

        //all of the related art forms to that particular Discipline
        public IEnumerable<ArtFormDto> RelatedArtForm { get; set; }

    }
}