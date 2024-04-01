using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models.ViewModels
{
    public class DetailsBrewery
    {
        public Brewery SelectedBrewery { get; set; }
        public IEnumerable<BeerDto> RelatedBeers { get; set; }
    }
}