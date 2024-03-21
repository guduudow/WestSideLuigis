using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models
{
    public class Brewery
    {
        [Key]
        public int BreweryID { get; set; }
        public string BreweryName { get; set; }
        public string BreweryLocation { get; set; }
        public bool BreweryHasPic { get; set; }
        public string PicExtension { get; set; }
    }
    public class BreweryDto
    {
        public int BreweryID { get; set; }
        public string BreweryName { get; set; }
        public string BreweryLocation { get; set; }
        public bool BreweryHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}