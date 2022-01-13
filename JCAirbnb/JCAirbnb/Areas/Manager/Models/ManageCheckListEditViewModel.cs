using JCAirbnb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManageCheckListEditViewModel
    {
        [Display(Name = "Check list")]
        public CheckList CheckList { get; set; }

        [Display(Name = "Items")]
        public List<CheckListItem> CheckListItems { get; set; }

        public bool AddingItem { get; set; }

        public string CheckListItemDescription { get; set; }

        public string CheckListItemId { get; set; }
    }
}
