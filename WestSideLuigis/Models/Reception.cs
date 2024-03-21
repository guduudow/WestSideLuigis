using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models
{
    public class Reception
    {
        [Key]
        public int ReceptionID { get; set; }
        public string ReceptionName { get; set; }
        public string ReceptionLocation { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal ReceptionPrice { get; set; }
        public string ReceptionDescription { get; set; }

        //an Reception can have many attendees
        public ICollection<Attendee> Attendees { get; set; }
    }

    public class ReceptionDto
    {
        public int ReceptionID { get; set; }
        public string ReceptionName { get; set; }
        public string ReceptionLocation { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal ReceptionPrice { get; set; }
        public string ReceptionDescription { get; set; }
    }
}