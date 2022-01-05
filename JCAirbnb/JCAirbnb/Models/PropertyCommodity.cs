using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class PropertyCommodity
    {
        [Key]
        public string Id { get; set; }

        public Commodity Commodity { get; set; }
    }
}
