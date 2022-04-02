using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Model
{
    public class Motivo : ExternalEntity
    {
        public String Descricao { get; set; }

        public static implicit operator string(Motivo v)
        {
            throw new NotImplementedException();
        }
    }
}
