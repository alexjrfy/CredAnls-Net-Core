using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace ApplicationCore.Model
{
    public class ExternalEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int IdExterno { get; set; }
        public bool Bloqueado { get; set; }
    }
}
