﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class PropertyCommodity
    {
        [Key]
        public string Id { get; set; }

        public bool Included { get; set; } = true;

        public Commodity Commodity { get; set; }
    }
}
