using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtistsHub.Models.ViewModel
{
    public class DetailArtForm
    {
        public ArtFormDto selectedArtForm { get; set; }
        public IEnumerable<ArtistDto> relatedArtist { get; set; }
    }
}