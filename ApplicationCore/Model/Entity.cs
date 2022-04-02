using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Model
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
