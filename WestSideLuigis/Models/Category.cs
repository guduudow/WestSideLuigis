using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}