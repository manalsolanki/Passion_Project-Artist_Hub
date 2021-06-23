using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistsHub.Models.ViewModel
{
    public class DetailArtist
    {
        public ArtistDto selectedArtist { get; set; }
        public IEnumerable<ArtFormDto> relatedArtForms { get; set; }
    }
}