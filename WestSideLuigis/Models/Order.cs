using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WestSideLuigis.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int TableNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        //An order can have many menu items
        public ICollection<MenuItem> MenuItems { get; set; }
        //We planned on adding BeerID, ItemID and AttendeeID to display for each order but ran into migration problems related to "FOREIGN KEY constraint 'FK_dbo.Orders_dbo.Beers_BeerID' on table 'Orders' may cause cycles or multiple cascade paths. Specify ON DELETE NO ACTION or ON UPDATE NO ACTION, or modify other FOREIGN KEY constraints." 
    }
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int TableNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
    }
}