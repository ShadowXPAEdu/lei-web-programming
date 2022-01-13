using JCAirbnb.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManageCheckListDetailsViewModel
    {
        [Display(Name = "Check list")]
        public CheckList CheckList { get; set; }

        [Display(Name = "Items")]
        public List<CheckListItem> CheckListItems { get; set; }
    }
}
