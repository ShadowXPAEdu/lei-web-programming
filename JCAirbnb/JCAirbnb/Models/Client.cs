using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JCAirbnb.Models
{
    public class Client
    {
        public string Id { get; set; }

        [ForeignKey("Id")]
        public virtual IdentityUser User { get; set; }

        public virtual List<Review> Reviews { get; set; }
    }
}
