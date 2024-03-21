using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models
{
    public class Attendee
    {
        [Key]
        public int AttendeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //an attendee can register for multiple Receptions
        public ICollection<Reception> Receptions { get; set; }
    }


    public class AttendeeDto
    {
        public int AttendeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}