using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PonyMusic.Models
{
    public class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        //public IEnumerable<Track> Tracks { get; set; }
        public string Link { get; set; }
    }
}