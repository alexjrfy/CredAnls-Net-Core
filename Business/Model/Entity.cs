using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Business.Model
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
