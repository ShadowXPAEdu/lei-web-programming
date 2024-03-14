using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models.Configuration
{
    public class PropertiesConfig
    {
        public string Title { get; set; }
        public string PropertyType { get; set; }
        public string Location { get; set; }
        public int BasePrice { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public Divisions Divisions { get; set; }
        public List<Photo> Photos { get; set; }
        public Ratings Ratings { get; set; }
    }
}
