using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models.ViewModels
{
    public class UpdateBeer
    {
        //This viewmodel is a class which stores information that we need to show to /Beer/UIpdate/{id}

        //the existing beer info

        public Beer SelectedBeer { get; set; }

        //Need to include all breweries available when updating the beer

        public IEnumerable<BreweryDto> BreweryOptions { get; set; }
    }
}