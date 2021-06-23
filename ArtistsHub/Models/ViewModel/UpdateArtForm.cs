using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistsHub.Models.ViewModel
{
    public class UpdateArtForm
    {
        //This viewmodel is a class which stores information that we need to present to /ArtForm/Update/{}

        //the existing artform information

        public ArtFormDto SelectedArtForm { get; set; }

        // all discipline to choose from while updating.

        public IEnumerable<DisciplineDto> DisciplinesOptions { get; set; }

    }
}