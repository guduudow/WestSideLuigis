using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WestSideLuigis.Models.ViewModels
{
    public class DetailsReception
    {
        public ReceptionDto SelectedReception { get; set; }
        public IEnumerable<AttendeeDto> RegAttendees { get; set; }

        public IEnumerable<AttendeeDto> PotenialAttendees { get; set; }
    }
}