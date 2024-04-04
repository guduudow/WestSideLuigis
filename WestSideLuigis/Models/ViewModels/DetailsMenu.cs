using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models.ViewModels
{
    public class DetailsMenu
    {
        public MenuItemDto selectedmenu { get; set; }
        public MenuItemDto selectedcategory { get; internal set; }
        public IEnumerable<MenuItemDto> MenuItems { get; set; }
        public DetailsMenu()
        {
            // Initialize Menus property to an empty list
            MenuItems = new List<MenuItemDto>();
        }
    }
}