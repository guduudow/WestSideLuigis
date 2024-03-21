using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models
{
    public class Beer
    {
        [Key]
        public int BeerID { get; set; }
        public string BeerName { get; set; }
        public string BeerType { get; set; }
        public string BeerDescription { get; set; }
        public string BeerAlcoholContent { get; set; }
        public bool BeerHasPic { get; set; }
        public string PicExtension { get; set; }
        public string BreweryName { get; set; }
        //A beer belongs to one brewery
        //A brewery can have multiple beers
        [ForeignKey("Brewery")]
        public int BreweryID { get; set; }
        public virtual Brewery Brewery { get; set; }

    }

    public class BeerDto
    {
        public int BeerID { get; set; }
        public string BeerName { get; set; }
        public string BeerType { get; set; }
        public string BeerDescription { get; set; }
        public string BeerAlcoholContent { get; set; }
        public int BreweryID { get; set; }
        public string BreweryName { get; set; }
        public bool BeerHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}