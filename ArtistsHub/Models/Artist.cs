using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistsHub.Models
{
    public class Artist
    {
        [Key]
        public int ArtistID { get; set; }
        public string ArtistFirstName { get; set; }
        public string ArtistLastName { get; set; }
        public string ArtistOccupation { get; set; }
        public string ArtistPhoneNumber { get; set; }
        public string ArtistEmail { get; set; }
        public string ArtistDescription { get; set; }

        public bool ArtistHasProfilePic { get; set; }
        public string ProfilePicExtension { get; set; }


        //An artist can have many art-forms.
        public ICollection<ArtForm> ArtForms { get; set; }
    }

    public class ArtistDto
    {
        public int ArtistID { get; set; }
        public string ArtistFirstName { get; set; }
        public string ArtistLastName { get; set; }
        public string ArtistOccupation { get; set; }
        public string ArtistPhoneNumber { get; set; }
        public string ArtistEmail { get; set; }
        public string ArtistDescription { get; set; }

        public bool ArtistHasProfilePic { get; set; }
        public string ProfilePicExtension { get; set; }

    }
}