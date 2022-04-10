using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Business.Model
{
    public class ExternalEntity: Entity
    {
        
        public int IdExterno { get; set; }
        public bool Bloqueado { get; set; }
    }
}
