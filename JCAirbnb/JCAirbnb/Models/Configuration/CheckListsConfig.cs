using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models.Configuration
{
    public class CheckListsConfig
    {
        public List<CheckList> CheckLists { get; set; }
        public List<CheckListItem> CheckListItems { get; set; }
    }
}
