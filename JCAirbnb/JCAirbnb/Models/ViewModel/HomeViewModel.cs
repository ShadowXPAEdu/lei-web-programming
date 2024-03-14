using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Property> Properties { get; set; }

        public int Page { get; set; }

        public bool HasPrevPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}
