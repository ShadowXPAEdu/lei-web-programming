using JCAirbnb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManageReservationsVerifyingViewModel
    {
        public Reservation Reservation { get; set; }

        public List<CheckListItem> ReservationCheckLists { get; set; }

        public List<CheckListItem> DeliveryCheckLists { get; set; }
    }
}
