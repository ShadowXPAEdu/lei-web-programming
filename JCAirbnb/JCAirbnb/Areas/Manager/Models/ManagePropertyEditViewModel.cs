﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Areas.Manager.Models
{
    public class ManagePropertyEditViewModel
    {
        [Display(Name = "Property")]
        public JCAirbnb.Models.Property Property { get; set; }

        [Display(Name = "PropertyCommodities")]
        public List<JCAirbnb.Models.PropertyCommodity> PropertyCommodities { get; set; }

        [Display(Name = "Commodities")]
        public List<JCAirbnb.Models.Commodity> Commodities { get; set; }

        [Display(Name = "AddingCommodity")]
        public bool AddingCommodity { get; set; }
    }
}
