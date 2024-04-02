using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models.ViewModels
{
    public class DetailsAttendee
    {
        public AttendeeDto SelectedAttendee { get; set; }

        public IEnumerable<ReceptionDto> RegEvents { get; set; }
    }
}